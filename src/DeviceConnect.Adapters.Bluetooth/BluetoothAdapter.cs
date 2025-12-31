namespace DeviceConnect.Adapters.Bluetooth;

using DeviceConnect.Abstractions;
using DeviceConnect.Contracts;
using DeviceConnect.Core;

/// <summary>
/// Skeleton Bluetooth adapter. Implement BLE GATT / notifications as needed.
/// </summary>
public sealed class BluetoothAdapter : IDeviceAdapter
{
    public string Id => "ble";

    public bool CanHandle(DeviceProfile profile) => profile.AdapterId == Id;

    public IDeviceSession CreateSession(DeviceProfile profile, IDeviceBus bus, IClock clock)
        => new BluetoothSession(profile, bus, clock);
}
