namespace DeviceConnect.Abstractions;

using DeviceConnect.Contracts;

public interface IDeviceSession : IAsyncDisposable
{
    DeviceProfile Profile { get; }
    SessionState State { get; }

    Task StartAsync(CancellationToken ct);
    Task StopAsync(CancellationToken ct);

    /// <summary>Send a command to the device via the adapter.</summary>
    Task SendAsync(DeviceCommand command, CancellationToken ct);
}
