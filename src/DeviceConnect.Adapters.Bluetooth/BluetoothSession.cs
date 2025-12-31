namespace DeviceConnect.Adapters.Bluetooth;

using DeviceConnect.Abstractions;
using DeviceConnect.Contracts;
using DeviceConnect.Core;

public sealed class BluetoothSession : IDeviceSession
{
    private readonly IDeviceBus _bus;
    private readonly IClock _clock;
    private CancellationTokenSource? _cts;
    private Task? _loop;

    public DeviceProfile Profile { get; }
    public SessionState State { get; private set; } = SessionState.Disconnected;

    public BluetoothSession(DeviceProfile profile, IDeviceBus bus, IClock clock)
    {
        Profile = profile;
        _bus = bus;
        _clock = clock;
    }

    public Task StartAsync(CancellationToken ct)
    {
        if (State != SessionState.Disconnected) return Task.CompletedTask;

        State = SessionState.Connecting;
        _bus.PublishEvent(new(Profile.DeviceId, "connecting", "Opening Bluetooth session", _clock.UtcNow));

        _cts = CancellationTokenSource.CreateLinkedTokenSource(ct);
        _loop = Task.Run(() => RunAsync(_cts.Token), _cts.Token);
        return Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken ct)
    {
        if (State == SessionState.Disconnected) return;
        State = SessionState.Stopping;
        _cts?.Cancel();
        if (_loop != null) await _loop.WaitAsync(ct);
        State = SessionState.Disconnected;
        _bus.PublishEvent(new(Profile.DeviceId, "disconnected", "Bluetooth session stopped", _clock.UtcNow));
    }

    public Task SendAsync(DeviceCommand command, CancellationToken ct)
    {
        // TODO: write GATT characteristic / send command
        _bus.PublishAck(new(command.CommandId, Profile.DeviceId, true, null, null, _clock.UtcNow));
        return Task.CompletedTask;
    }

    private async Task RunAsync(CancellationToken ct)
    {
        State = SessionState.Connected;
        _bus.PublishEvent(new(Profile.DeviceId, "connected", "Bluetooth connected", _clock.UtcNow));

        while (!ct.IsCancellationRequested)
        {
            _bus.PublishTelemetry(new(Profile.DeviceId, "telemetry", _clock.UtcNow,
                new Dictionary<string, object?> { ["battery"] = Random.Shared.Next(60, 100) }));
            await Task.Delay(1000, ct);
        }
    }

    public async ValueTask DisposeAsync()
    {
        try { await StopAsync(CancellationToken.None); } catch { }
        _cts?.Dispose();
    }
}
