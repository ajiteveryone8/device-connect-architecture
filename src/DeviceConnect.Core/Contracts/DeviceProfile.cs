namespace DeviceConnect.Contracts;

public sealed record DeviceProfile(
    string DeviceId,
    string AdapterId,
    string DisplayName,
    IReadOnlyDictionary<string, string> Settings
);
