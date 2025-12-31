namespace DeviceConnect.Core;

using System.Collections.Concurrent;
using DeviceConnect.Abstractions;
using DeviceConnect.Contracts;

public interface IDeviceBus
{
    void PublishTelemetry(DeviceTelemetry telemetry);
    void PublishEvent(DeviceEvent ev);
    void PublishAck(DeviceAck ack);

    IAsyncEnumerable<DeviceTelemetry> TelemetryStream(CancellationToken ct);
    IAsyncEnumerable<DeviceEvent> EventStream(CancellationToken ct);
    IAsyncEnumerable<DeviceAck> AckStream(CancellationToken ct);
}

/// <summary>
/// In-memory bus for demo/testing. Replace with Channel-based + persistence if needed.
/// </summary>
public sealed class InMemoryDeviceBus : IDeviceBus
{
    private readonly ConcurrentQueue<DeviceTelemetry> _telemetry = new();
    private readonly ConcurrentQueue<DeviceEvent> _events = new();
    private readonly ConcurrentQueue<DeviceAck> _acks = new();

    private readonly SemaphoreSlim _signal = new(0);

    public void PublishTelemetry(DeviceTelemetry telemetry)
    {
        _telemetry.Enqueue(telemetry);
        _signal.Release();
    }

    public void PublishEvent(DeviceEvent ev)
    {
        _events.Enqueue(ev);
        _signal.Release();
    }

    public void PublishAck(DeviceAck ack)
    {
        _acks.Enqueue(ack);
        _signal.Release();
    }

    public async IAsyncEnumerable<DeviceTelemetry> TelemetryStream([System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            await _signal.WaitAsync(ct);
            while (_telemetry.TryDequeue(out var t))
                yield return t;
        }
    }

    public async IAsyncEnumerable<DeviceEvent> EventStream([System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            await _signal.WaitAsync(ct);
            while (_events.TryDequeue(out var e))
                yield return e;
        }
    }

    public async IAsyncEnumerable<DeviceAck> AckStream([System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            await _signal.WaitAsync(ct);
            while (_acks.TryDequeue(out var a))
                yield return a;
        }
    }
}
