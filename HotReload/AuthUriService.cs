using Microsoft.FeatureManagement;

namespace HotReload;

public interface IAuthUriService
{
    Task<string> GetAsync();
}

public class AuthUriService : IAuthUriService
{
    private const string App2AppFeatureName = "app2app";
    private const string AuthUriApp2App = "bankapp://login";
    private const string AuthUriWeb = "https://bankwebsite.com/authorize";

    private readonly ILogger<AuthUriService> _logger;
    private readonly IFeatureManager _featureManager;

    public AuthUriService(ILogger<AuthUriService> logger, IFeatureManager featureManager) =>
        (_logger, _featureManager) = (logger, featureManager);

    public async Task<string> GetAsync() =>
        await _featureManager.IsEnabledAsync(App2AppFeatureName)
            ? AuthUriApp2App
            : AuthUriWeb;
}