using DeviceConnect.Core;
using DeviceConnect.Contracts;
using Xunit;

public sealed class BusTests
{
    [Fact]
    public void InMemoryBus_Publish_DoesNotThrow()
    {
        var bus = new InMemoryDeviceBus();
        bus.PublishEvent(new DeviceEvent("d1", "test", "ok", DateTimeOffset.UtcNow));
        bus.PublishTelemetry(new DeviceTelemetry("d1", "s1", DateTimeOffset.UtcNow, new Dictionary<string, object?> { ["x"] = 1 }));
        bus.PublishAck(new DeviceAck("c1", "d1", true, null, null, DateTimeOffset.UtcNow));
    }
}
