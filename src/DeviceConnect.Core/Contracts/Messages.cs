namespace DeviceConnect.Contracts;

public enum MessageKind
{
    Telemetry,
    Command,
    Ack,
    Event
}

public sealed record DeviceTelemetry(
    string DeviceId,
    string Stream,
    DateTimeOffset TimestampUtc,
    IReadOnlyDictionary<string, object?> Fields
);

public sealed record DeviceCommand(
    string CommandId,
    string DeviceId,
    string Name,
    IReadOnlyDictionary<string, object?> Parameters,
    TimeSpan Timeout
);

public sealed record DeviceAck(
    string CommandId,
    string DeviceId,
    bool Success,
    string? ErrorCode,
    string? ErrorMessage,
    DateTimeOffset TimestampUtc
);

public sealed record DeviceEvent(
    string DeviceId,
    string Type,
    string Message,
    DateTimeOffset TimestampUtc,
    IReadOnlyDictionary<string, object?>? Data = null
);
