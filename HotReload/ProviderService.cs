using Microsoft.Extensions.Options;

namespace HotReload;

public interface IProviderService
{
    IEnumerable<Provider> Get();
}

/// <summary>
/// The options provider service class shows the usual way we use options.
/// IOptions<T> reads its values at start and then remain the same for the entire lifetime of the app
/// Even if reloadOnChange is set to true IOptions will not be updated
/// </summary>
public class OptionsProviderService : IProviderService
{
    private readonly ILogger<OptionsProviderService> _logger;
    private readonly IOptions<ProviderServiceOptions> _options;

    public OptionsProviderService(ILogger<OptionsProviderService> logger, IOptions<ProviderServiceOptions> options) =>
        (_logger, _options) = (logger, options);

    public IEnumerable<Provider> Get() => _options.Value?.Providers ?? new List<Provider>();
}

/// <summary>
/// The options snapshot provider service class shows how to use options that are re evaluated during different scopes (requests)
/// IOptionsSnapshot<T> will contain updated values
/// Service needs to be either Scoped or Transient
/// </summary>
public class OptionsSnapshotProviderService : IProviderService
{
    private readonly ILogger<OptionsSnapshotProviderService> _logger;
    private readonly IOptionsSnapshot<ProviderServiceOptions> _options;

    public OptionsSnapshotProviderService(ILogger<OptionsSnapshotProviderService> logger, IOptionsSnapshot<ProviderServiceOptions> options) =>
        (_logger, _options) = (logger, options);

    public IEnumerable<Provider> Get() => _options.Value?.Providers ?? new List<Provider>();
}

/// <summary>
/// The options monitor provider service class shows how to use options that receive updates when used in Singleton instances
/// IOptionsMonitor offers a `CurrentValue` field and an OnChange callback
/// </summary>
public class OptionsMonitorProviderService : IProviderService
{
    private readonly ILogger<OptionsMonitorProviderService> _logger;
    private readonly IOptionsMonitor<ProviderServiceOptions> _options;

    public OptionsMonitorProviderService(ILogger<OptionsMonitorProviderService> logger, IOptionsMonitor<ProviderServiceOptions> options) =>
        (_logger, _options) = (logger, options);

    public IEnumerable<Provider> Get() => _options.CurrentValue?.Providers ?? new List<Provider>();
}