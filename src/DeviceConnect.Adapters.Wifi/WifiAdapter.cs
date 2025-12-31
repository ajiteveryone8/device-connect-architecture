namespace DeviceConnect.Adapters.Wifi;

using DeviceConnect.Abstractions;
using DeviceConnect.Contracts;
using DeviceConnect.Core;

/// <summary>
/// Skeleton Wi‑Fi adapter. Implement TCP/UDP/HTTP/WebSocket/MQTT depending on your device.
/// </summary>
public sealed class WifiAdapter : IDeviceAdapter
{
    public string Id => "wifi";

    public bool CanHandle(DeviceProfile profile) => profile.AdapterId == Id;

    public IDeviceSession CreateSession(DeviceProfile profile, IDeviceBus bus, IClock clock)
        => new WifiSession(profile, bus, clock);
}
