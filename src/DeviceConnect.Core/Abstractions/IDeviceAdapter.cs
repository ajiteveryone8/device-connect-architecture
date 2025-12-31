namespace DeviceConnect.Abstractions;

using DeviceConnect.Contracts;
using DeviceConnect.Core;

public interface IDeviceAdapter
{
    /// <summary>Unique adapter id (e.g., "serial", "wifi", "ble", "plc").</summary>
    string Id { get; }

    /// <summary>Returns true if this adapter can handle the given device profile.</summary>
    bool CanHandle(DeviceProfile profile);

    /// <summary>Create a connection session for a device profile.</summary>
    IDeviceSession CreateSession(DeviceProfile profile, IDeviceBus bus, IClock clock);
}
