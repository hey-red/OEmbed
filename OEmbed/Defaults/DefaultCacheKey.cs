using System;
using System.Buffers;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

using HeyRed.OEmbed.Abstractions;

namespace HeyRed.OEmbed.Defaults;

/// <summary>
///     The source taken from
///     https://github.com/SixLabors/ImageSharp.Web/blob/main/src/ImageSharp.Web/Caching/SHA256CacheHash.cs
/// </summary>
public class DefaultCacheKey : ICacheKey
{
    private const int HASH_LENGTH = 64;

    public string CreateKey(string value)
    {
        int byteCount = Encoding.ASCII.GetByteCount(value);

        // Allocating a buffer from the pool is ~27% slower than stackalloc so use that for short strings
        if (byteCount < 257)
        {
            return HashValue(value, HASH_LENGTH, stackalloc byte[byteCount]);
        }

        byte[]? buffer = null;
        try
        {
            buffer = ArrayPool<byte>.Shared.Rent(byteCount);
            return HashValue(value, HASH_LENGTH, buffer.AsSpan(0, byteCount));
        }
        finally
        {
            if (buffer != null)
            {
                ArrayPool<byte>.Shared.Return(buffer);
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static string HashValue(ReadOnlySpan<char> value, uint length, Span<byte> bufferSpan)
    {
        using var hashAlgorithm = SHA256.Create();
        Encoding.ASCII.GetBytes(value, bufferSpan);

        // Hashed output maxes out at 32 bytes @ 256bit/8 so we're safe to use stackalloc
        Span<byte> hash = stackalloc byte[32];
        hashAlgorithm.TryComputeHash(bufferSpan, hash, out int _);

        // Length maxes out at 64 since we throw if options is greater
        return HexEncoder.Encode(hash.Slice(0, (int)(length / 2)));
    }
}

/// <summary>
///     The source taken from
///     https://github.com/SixLabors/ImageSharp.Web/blob/main/src/ImageSharp.Web/Caching/HexEncoder.cs
///     Provides methods for encoding byte arrays into hexidecimal strings.
/// </summary>
internal static class HexEncoder
{
    // LUT's that provide the hexidecimal representation of each possible byte value.
    private static readonly char[] HexLutBase = new[]
        { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f' };

    // The base LUT arranged in 16x each item order. 0 * 16, 1 * 16, .... F * 16
    private static readonly char[] HexLutHi = Enumerable
        .Range(0, 256)
        .Select(x => HexLutBase[x / 0x10])
        .ToArray();

    // The base LUT repeated 16x.
    private static readonly char[] HexLutLo = Enumerable
        .Range(0, 256)
        .Select(x => HexLutBase[x % 0x10])
        .ToArray();

    /// <summary>
    ///     Converts a <see cref="ReadOnlySpan{Byte}" /> to a hexidecimal formatted <see cref="string" /> padded to 2 digits.
    /// </summary>
    /// <param name="bytes">The bytes.</param>
    /// <returns>The <see cref="string" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe string Encode(ReadOnlySpan<byte> bytes)
    {
        fixed (byte* bytesPtr = bytes)
        {
            return string.Create(bytes.Length * 2, (Ptr: (IntPtr)bytesPtr, bytes.Length), (chars, args) =>
            {
                var ros = new ReadOnlySpan<byte>((byte*)args.Ptr, args.Length);
                EncodeToUtf16(ros, chars);
            });
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void EncodeToUtf16(ReadOnlySpan<byte> bytes, Span<char> chars)
    {
        ref byte bytesRef = ref MemoryMarshal.GetReference(bytes);
        ref char charRef = ref MemoryMarshal.GetReference(chars);
        ref char hiRef = ref MemoryMarshal.GetReference<char>(HexLutHi);
        ref char lowRef = ref MemoryMarshal.GetReference<char>(HexLutLo);

        var index = 0;
        for (var i = 0; i < bytes.Length; i++)
        {
            byte byteIndex = Unsafe.Add(ref bytesRef, i);
            Unsafe.Add(ref charRef, index++) = Unsafe.Add(ref hiRef, byteIndex);
            Unsafe.Add(ref charRef, index++) = Unsafe.Add(ref lowRef, byteIndex);
        }
    }
}