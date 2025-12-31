namespace DeviceConnect.Codecs;

public sealed class DelimiterFrameCodec : IFrameCodec
{
    private readonly byte _delimiter;

    public DelimiterFrameCodec(byte delimiter = (byte)'\n')
    {
        _delimiter = delimiter;
    }

    public bool TryDecode(ReadOnlySpan<byte> input, out int consumed, out ReadOnlyMemory<byte> frame)
    {
        var idx = input.IndexOf(_delimiter);
        if (idx < 0)
        {
            consumed = 0;
            frame = default;
            return false;
        }

        consumed = idx + 1;
        frame = input[..idx].ToArray();
        return true;
    }

    public byte[] Encode(ReadOnlySpan<byte> payload)
    {
        var arr = new byte[payload.Length + 1];
        payload.CopyTo(arr);
        arr[^1] = _delimiter;
        return arr;
    }
}
