using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace WolfRpc.Hashing;

internal static class BitOperations
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint RotateLeft(uint value, int offset)
        => (value << offset) | (value >> (32 - offset));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ulong RotateLeft(ulong value, int offset)
        => (value << offset) | (value >> (64 - offset));
}