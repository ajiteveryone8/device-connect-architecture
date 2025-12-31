# How to add a new device

1. Pick an adapter:
   - Serial: `DeviceConnect.Adapters.Serial`
   - Wi‑Fi: `DeviceConnect.Adapters.Wifi`
   - Bluetooth: `DeviceConnect.Adapters.Bluetooth`
   - PLC: `DeviceConnect.Adapters.Plc`

2. Add a `DeviceProfile` setting keys (examples):
   - Serial: port, baud, parity
   - Wi‑Fi: host, port, transport
   - BLE: mac, serviceUuid, rxCharUuid, txCharUuid
   - PLC: protocol, host, rack/slot or unitId

3. Implement transport I/O in the session:
   - Open connection in `RunAsync()`
   - Decode frames (optional) with `Core/Codecs`
   - Publish telemetry via `bus.PublishTelemetry(...)`
   - Publish connection events
   - Implement `SendAsync(...)` for commands

4. Add tests:
   - Simulate payloads and assert telemetry/events are produced.
