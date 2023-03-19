using Flurl.Http;
using Thabe.Bot.Core.Logger;

namespace Thabe.Bot.Core.Database.Config.Concrete;


/// <summary>
/// 网络Json配置
/// </summary>
public class WebJsonConfig : IConfig, IPersistentData
{
    private JsonConfig _config;

    public WebJsonConfig(string url)
    {
        _config = new(GetJson, SaveJson);

        string GetJson()
        {
            try { return url.GetStringAsync().GetAwaiter().GetResult(); }
            catch (Exception ex)
            {
                ex.Message.LogError();
                return "";
            }
        }

        static void SaveJson(string json)
        {
            $"{nameof(WebJsonConfig)}暂不支持保存".LogError();
        }
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
