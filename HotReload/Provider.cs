namespace HotReload;

public class Provider
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public List<string> AllowedClients { get; set; } = new();
}