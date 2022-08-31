using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Text;

namespace WolfRpc.Hashing;

public sealed partial class XxHash64
{
    private struct State
    {
        private const ulong Prime64_1 = 0x9E3779B185EBCA87;
        private const ulong Prime64_2 = 0xC2B2AE3D27D4EB4F;
        private const ulong Prime64_3 = 0x165667B19E3779F9;
        private const ulong Prime64_4 = 0x85EBCA77C2B2AE63;
        private const ulong Prime64_5 = 0x27D4EB2F165667C5;

        private ulong _acc1;
        private ulong _acc2;
        private ulong _acc3;
        private ulong _acc4;
        private readonly ulong _smallAcc;
        private bool _hadFullStripe;

        internal State(ulong seed)
        {
            _acc1 = seed + unchecked(Prime64_1 + Prime64_2);
            _acc2 = seed + Prime64_2;
            _acc3 = seed;
            _acc4 = seed - Prime64_1;

            _smallAcc = seed + Prime64_5;
            _hadFullStripe = false;
        }

        internal void ProcessStripe(ReadOnlySpan<byte> source)
        {
            Debug.Assert(source.Length >= StripeSize);
            source = source.Slice(0, StripeSize);

            _acc1 = ApplyRound(_acc1, source);
            _acc2 = ApplyRound(_acc2, source.Slice(sizeof(ulong)));
            _acc3 = ApplyRound(_acc3, source.Slice(2 * sizeof(ulong)));
            _acc4 = ApplyRound(_acc4, source.Slice(3 * sizeof(ulong)));

            _hadFullStripe = true;
        }

        private static ulong MergeAccumulator(ulong acc, ulong accN)
        {
            acc ^= ApplyRound(0, accN);
            acc *= Prime64_1;
            acc += Prime64_4;

            return acc;
        }

        private readonly ulong Converge()
        {
            ulong acc =
                BitOperations.RotateLeft(_acc1, 1) +
                BitOperations.RotateLeft(_acc2, 7) +
                BitOperations.RotateLeft(_acc3, 12) +
                BitOperations.RotateLeft(_acc4, 18);

            acc = MergeAccumulator(acc, _acc1);
            acc = MergeAccumulator(acc, _acc2);
            acc = MergeAccumulator(acc, _acc3);
            acc = MergeAccumulator(acc, _acc4);

            return acc;
        }

        private static ulong ApplyRound(ulong acc, ReadOnlySpan<byte> lane)
        {
            return ApplyRound(acc, BinaryPrimitives.ReadUInt64LittleEndian(lane));
        }

        private static ulong ApplyRound(ulong acc, ulong lane)
        {
            acc += lane * Prime64_2;
            acc = BitOperations.RotateLeft(acc, 31);
            acc *= Prime64_1;

            return acc;
        }

        internal readonly ulong Complete(long length, ReadOnlySpan<byte> remaining)
        {
            ulong acc = _hadFullStripe ? Converge() : _smallAcc;

            acc += (ulong)length;

            while (remaining.Length >= sizeof(ulong))
            {
                ulong lane = BinaryPrimitives.ReadUInt64LittleEndian(remaining);
                acc ^= ApplyRound(0, lane);
                acc = BitOperations.RotateLeft(acc, 27);
                acc *= Prime64_1;
                acc += Prime64_4;

                remaining = remaining.Slice(sizeof(ulong));
            }

            // Doesn't need to be a while since it can occur at most once.
            if (remaining.Length >= sizeof(uint))
            {
                ulong lane = BinaryPrimitives.ReadUInt32LittleEndian(remaining);
                acc ^= lane * Prime64_1;
                acc = BitOperations.RotateLeft(acc, 23);
                acc *= Prime64_2;
                acc += Prime64_3;

                remaining = remaining.Slice(sizeof(uint));
            }

            for (int i = 0; i < remaining.Length; i++)
            {
                ulong lane = remaining[i];
                acc ^= lane * Prime64_5;
                acc = BitOperations.RotateLeft(acc, 11);
                acc *= Prime64_1;
            }

            acc ^= (acc >> 33);
            acc *= Prime64_2;
            acc ^= (acc >> 29);
            acc *= Prime64_3;
            acc ^= (acc >> 32);

            return acc;
        }
    }
}

/// <summary>
///   Provides an implementation of the XxHash64 algorithm.
/// </summary>
public sealed partial class XxHash64 : NonCryptographicHashAlgorithm
{
    private const int HashSize = sizeof(ulong);
    private const int StripeSize = 4 * sizeof(ulong);

    private readonly ulong _seed;
    private State _state;
    private byte[]? _holdback;
    private long _length;

    /// <summary>
    ///   Initializes a new instance of the <see cref="XxHash64"/> class.
    /// </summary>
    /// <remarks>
    ///   The XxHash64 algorithm supports an optional seed value.
    ///   Instances created with this constructor use the default seed, zero.
    /// </remarks>
    public XxHash64()
        : this(0)
    {
    }

    /// <summary>
    ///   Initializes a new instance of the <see cref="XxHash64"/> class with
    ///   a specified seed.
    /// </summary>
    /// <param name="seed">
    ///   The hash seed value for computations from this instance.
    /// </param>
    public XxHash64(long seed)
        : base(HashSize)
    {
        _seed = (ulong)seed;
        Reset();
    }

    /// <summary>
    ///   Resets the hash computation to the initial state.
    /// </summary>
    public override void Reset()
    {
        _state = new State(_seed);
        _length = 0;
    }

    /// <summary>
    ///   Appends the contents of <paramref name="source"/> to the data already
    ///   processed for the current hash computation.
    /// </summary>
    /// <param name="source">The data to process.</param>
    public override void Append(ReadOnlySpan<byte> source)
    {
        // Every time we've read 32 bytes, process the stripe.
        // Data that isn't perfectly mod-32 gets stored in a holdback
        // buffer.

        int held = (int)_length & 0x1F;

        if (held != 0)
        {
            int remain = StripeSize - held;

            if (source.Length >= remain)
            {
                source.Slice(0, remain).CopyTo(_holdback.AsSpan(held));
                _state.ProcessStripe(_holdback);

                source = source.Slice(remain);
                _length += remain;
            }
            else
            {
                source.CopyTo(_holdback.AsSpan(held));
                _length += source.Length;
                return;
            }
        }

        while (source.Length >= StripeSize)
        {
            _state.ProcessStripe(source);
            source = source.Slice(StripeSize);
            _length += StripeSize;
        }

        if (source.Length > 0)
        {
            _holdback ??= new byte[StripeSize];
            source.CopyTo(_holdback);
            _length += source.Length;
        }
    }

    /// <summary>
    ///   Writes the computed hash value to <paramref name="destination"/>
    ///   without modifying accumulated state.
    /// </summary>
    protected override void GetCurrentHashCore(Span<byte> destination)
    {
        int remainingLength = (int)_length & 0x1F;
        ReadOnlySpan<byte> remaining = ReadOnlySpan<byte>.Empty;

        if (remainingLength > 0)
        {
            remaining = new ReadOnlySpan<byte>(_holdback, 0, remainingLength);
        }

        ulong acc = _state.Complete(_length, remaining);
        BinaryPrimitives.WriteUInt64BigEndian(destination, acc);
    }

    /// <summary>
    ///   Computes the XxHash64 hash of the provided data.
    /// </summary>
    /// <param name="source">The data to hash.</param>
    /// <returns>The XxHash64 hash of the provided data.</returns>
    /// <exception cref="ArgumentNullException">
    ///   <paramref name="source"/> is <see langword="null"/>.
    /// </exception>
    public static byte[] Hash(byte[] source)
    {
        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        return Hash(new ReadOnlySpan<byte>(source));
    }

    /// <summary>
    ///   Computes the XxHash64 hash of the provided data using the provided seed.
    /// </summary>
    /// <param name="source">The data to hash.</param>
    /// <param name="seed">The seed value for this hash computation.</param>
    /// <returns>The XxHash64 hash of the provided data.</returns>
    /// <exception cref="ArgumentNullException">
    ///   <paramref name="source"/> is <see langword="null"/>.
    /// </exception>
    public static byte[] Hash(byte[] source, long seed)
    {
        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        return Hash(new ReadOnlySpan<byte>(source), seed);
    }

    /// <summary>
    ///   Computes the XxHash64 hash of the provided data.
    /// </summary>
    /// <param name="source">The data to hash.</param>
    /// <param name="seed">The seed value for this hash computation. The default is zero.</param>
    /// <returns>The XxHash64 hash of the provided data.</returns>
    public static byte[] Hash(ReadOnlySpan<byte> source, long seed = 0)
    {
        byte[] ret = new byte[HashSize];
        StaticHash(source, ret, seed);
        return ret;
    }

    /// <summary>
    ///   Attempts to compute the XxHash64 hash of the provided data into the provided destination.
    /// </summary>
    /// <param name="source">The data to hash.</param>
    /// <param name="destination">The buffer that receives the computed hash value.</param>
    /// <param name="bytesWritten">
    ///   On success, receives the number of bytes written to <paramref name="destination"/>.
    /// </param>
    /// <param name="seed">The seed value for this hash computation. The default is zero.</param>
    /// <returns>
    ///   <see langword="true"/> if <paramref name="destination"/> is long enough to receive
    ///   the computed hash value (4 bytes); otherwise, <see langword="false"/>.
    /// </returns>
    public static bool TryHash(ReadOnlySpan<byte> source, Span<byte> destination, out int bytesWritten, long seed = 0)
    {
        if (destination.Length < HashSize)
        {
            bytesWritten = 0;
            return false;
        }

        bytesWritten = StaticHash(source, destination, seed);
        return true;
    }

    /// <summary>
    ///   Computes the XxHash64 hash of the provided data into the provided destination.
    /// </summary>
    /// <param name="source">The data to hash.</param>
    /// <param name="destination">The buffer that receives the computed hash value.</param>
    /// <param name="seed">The seed value for this hash computation. The default is zero.</param>
    /// <returns>
    ///   The number of bytes written to <paramref name="destination"/>.
    /// </returns>
    public static int Hash(ReadOnlySpan<byte> source, Span<byte> destination, long seed = 0)
    {
        if (destination.Length < HashSize)
            throw new ArgumentException("Destination too short", nameof(destination));

        return StaticHash(source, destination, seed);
    }

    private static int StaticHash(ReadOnlySpan<byte> source, Span<byte> destination, long seed)
    {
        int totalLength = source.Length;
        State state = new State((ulong)seed);

        while (source.Length >= StripeSize)
        {
            state.ProcessStripe(source);
            source = source.Slice(StripeSize);
        }

        ulong val = state.Complete((uint)totalLength, source);
        BinaryPrimitives.WriteUInt64BigEndian(destination, val);
        return HashSize;
    }
}
