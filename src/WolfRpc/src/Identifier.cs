using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Security.Cryptography;
using System.Text;

using WolfRpc.Hashing;

namespace WolfRpc;


public class Identifier
{
    private readonly ulong value = 0;
    public ulong Value => value;

    public Identifier(ulong value)
    {
        this.value = value;
    }

    public override int GetHashCode()
    {
        return this.value.GetHashCode();
    }

    public override bool Equals(object obj)
    {
        var other = obj as Identifier;
        return other == null ? false : other.value == this.value;
    }

    public static ulong From(string value)
    {
        var hash = XxHash64.Hash(Encoding.Default.GetBytes(value));
        return BitConverter.ToUInt64(hash, 0);
    }

    public static Identifier Create(string value)
    {
        return new Identifier(From(value));
    }
}
