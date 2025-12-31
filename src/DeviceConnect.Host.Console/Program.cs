using DeviceConnect.Abstractions;
using DeviceConnect.Contracts;
using DeviceConnect.Core;
using DeviceConnect.Adapters.Serial;
using DeviceConnect.Adapters.Wifi;
using DeviceConnect.Adapters.Bluetooth;
using DeviceConnect.Adapters.Plc;

var bus = new InMemoryDeviceBus();
var registry = new DeviceRegistry();
var clock = new SystemClock();

registry.RegisterAdapter(new SerialAdapter());
registry.RegisterAdapter(new WifiAdapter());
registry.RegisterAdapter(new BluetoothAdapter());
registry.RegisterAdapter(new PlcAdapter());

var sessions = new SessionManager(registry, bus, clock);

var demoDevices = new[]
{
    new DeviceProfile("dev-serial-01", "serial", "Serial Demo", new Dictionary<string,string>
    {
        ["port"] = "COM3",
        ["baud"] = "115200"
    }),
    new DeviceProfile("dev-wifi-01", "wifi", "Wi‑Fi Demo", new Dictionary<string,string>
    {
        ["host"] = "192.168.1.50",
        ["port"] = "9000",
        ["transport"] = "tcp"
    }),
    new DeviceProfile("dev-ble-01", "ble", "Bluetooth Demo", new Dictionary<string,string>
    {
        ["mac"] = "AA:BB:CC:DD:EE:FF"
    }),
    new DeviceProfile("dev-plc-01", "plc", "PLC Demo", new Dictionary<string,string>
    {
        ["protocol"] = "modbus-tcp",
        ["host"] = "192.168.1.10",
        ["unitId"] = "1"
    }),
};

using var cts = new CancellationTokenSource();
Console.CancelKeyPress += (_, e) => { e.Cancel = true; cts.Cancel(); };

Console.WriteLine("Starting sessions...");
var running = new List<IDeviceSession>();

foreach (var p in demoDevices)
{
    var s = sessions.Start(p);
    running.Add(s);
    await s.StartAsync(cts.Token);
}

_ = Task.Run(async () =>
{
    await foreach (var t in bus.TelemetryStream(cts.Token))
        Console.WriteLine($"[TEL] {t.DeviceId} {t.Stream} {t.TimestampUtc:O} {string.Join(", ", t.Fields.Select(kv => $"{kv.Key}={kv.Value}"))}");
}, cts.Token);

_ = Task.Run(async () =>
{
    await foreach (var e in bus.EventStream(cts.Token))
        Console.WriteLine($"[EVT] {e.DeviceId} {e.Type} {e.TimestampUtc:O} {e.Message}");
}, cts.Token);

_ = Task.Run(async () =>
{
    await foreach (var a in bus.AckStream(cts.Token))
        Console.WriteLine($"[ACK] {a.DeviceId} cmd={a.CommandId} ok={a.Success} err={a.ErrorCode}");
}, cts.Token);

// Send a demo command
await Task.Delay(1000, cts.Token);
var cmd = new DeviceCommand(Guid.NewGuid().ToString("N"), "dev-serial-01", "Ping",
    new Dictionary<string, object?> { ["payload"] = "hello" }, TimeSpan.FromSeconds(2));

await running[0].SendAsync(cmd, cts.Token);

Console.WriteLine("Running. Press Ctrl+C to stop.");
try
{
    while (!cts.IsCancellationRequested)
        await Task.Delay(500, cts.Token);
}
catch (OperationCanceledException) { }

Console.WriteLine("Stopping...");
foreach (var s in running)
    await s.StopAsync(CancellationToken.None);
