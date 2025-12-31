namespace DeviceConnect.Adapters.Serial;

using DeviceConnect.Abstractions;
using DeviceConnect.Contracts;
using DeviceConnect.Core;

/// <summary>
/// Skeleton adapter. Plug in System.IO.Ports or a native serial library as needed.
/// </summary>
public sealed class SerialAdapter : IDeviceAdapter
{
    public string Id => "serial";

    public bool CanHandle(DeviceProfile profile) => profile.AdapterId == Id;

    public IDeviceSession CreateSession(DeviceProfile profile, IDeviceBus bus, IClock clock)
        => new SerialSession(profile, bus, clock);
}
