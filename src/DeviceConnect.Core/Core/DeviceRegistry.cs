namespace DeviceConnect.Core;

using DeviceConnect.Abstractions;
using DeviceConnect.Contracts;

public sealed class DeviceRegistry
{
    private readonly Dictionary<string, IDeviceAdapter> _adapters = new();

    public void RegisterAdapter(IDeviceAdapter adapter)
        => _adapters[adapter.Id] = adapter;

    public IDeviceAdapter GetAdapter(string adapterId)
        => _adapters.TryGetValue(adapterId, out var a)
            ? a
            : throw new InvalidOperationException($"Adapter not found: {adapterId}");

    public IEnumerable<IDeviceAdapter> Adapters => _adapters.Values;
}
