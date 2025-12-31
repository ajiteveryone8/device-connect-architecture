namespace DeviceConnect.Core;

using DeviceConnect.Abstractions;
using DeviceConnect.Contracts;

public sealed class SessionManager
{
    private readonly DeviceRegistry _registry;
    private readonly IDeviceBus _bus;
    private readonly IClock _clock;

    private readonly Dictionary<string, IDeviceSession> _sessions = new();

    public SessionManager(DeviceRegistry registry, IDeviceBus bus, IClock clock)
    {
        _registry = registry;
        _bus = bus;
        _clock = clock;
    }

    public IDeviceSession Start(DeviceProfile profile)
    {
        if (_sessions.ContainsKey(profile.DeviceId))
            throw new InvalidOperationException($"Session already exists for {profile.DeviceId}");

        var adapter = _registry.GetAdapter(profile.AdapterId);
        if (!adapter.CanHandle(profile))
            throw new InvalidOperationException($"Adapter '{profile.AdapterId}' cannot handle the profile.");

        var session = adapter.CreateSession(profile, _bus, _clock);
        _sessions[profile.DeviceId] = session;
        return session;
    }

    public async Task StopAsync(string deviceId, CancellationToken ct)
    {
        if (_sessions.TryGetValue(deviceId, out var s))
        {
            await s.StopAsync(ct);
            await s.DisposeAsync();
            _sessions.Remove(deviceId);
        }
    }
}
