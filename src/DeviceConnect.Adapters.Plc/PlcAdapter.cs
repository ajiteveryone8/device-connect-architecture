namespace DeviceConnect.Adapters.Plc;

using DeviceConnect.Abstractions;
using DeviceConnect.Contracts;
using DeviceConnect.Core;

/// <summary>
/// Skeleton PLC adapter. Add Modbus/S7/OPC UA/EtherNet/IP client libs as needed.
/// </summary>
public sealed class PlcAdapter : IDeviceAdapter
{
    public string Id => "plc";

    public bool CanHandle(DeviceProfile profile) => profile.AdapterId == Id;

    public IDeviceSession CreateSession(DeviceProfile profile, IDeviceBus bus, IClock clock)
        => new PlcSession(profile, bus, clock);
}
