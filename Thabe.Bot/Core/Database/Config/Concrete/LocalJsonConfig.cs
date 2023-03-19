using Thabe.Bot.Core.Database.PathUtil;

namespace Thabe.Bot.Core.Database.Config.Concrete;


/// <summary>
/// 本地Json配置
/// </summary>
public class LocalJsonConfig : IConfig, IPersistentData
{
    private readonly LocalFileHelper _fileHelper;

    private readonly JsonConfig _config;

    public LocalJsonConfig(string name, string? path = null)
    {
        _fileHelper = new(name, path);
        _config = new(_fileHelper.LoadText, _fileHelper.SaveText);
    }

    public T GetOrDefault<T>(string key, T defaultValue) where T : notnull
        => _config.GetOrDefault(key, defaultValue);

    public T? GetOrNull<T>(string key)
        => _config.GetOrNull<T>(key);

    public void Set<T>(string key, T data) where T : notnull
        => _config.Set(key, data);


    public void Pull() => _config.Pull();

    public void Push() => _config.Push();
}
