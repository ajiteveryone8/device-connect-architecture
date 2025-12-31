namespace DeviceConnect.Codecs;

/// <summary>
/// Implement framing for your device protocol (length-prefix, delimiter, CRC, etc).
/// </summary>
public interface IFrameCodec
{
    bool TryDecode(ReadOnlySpan<byte> input, out int consumed, out ReadOnlyMemory<byte> frame);
    byte[] Encode(ReadOnlySpan<byte> payload);
}
