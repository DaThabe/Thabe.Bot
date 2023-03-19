using Newtonsoft.Json;
using Thabe.Bot.Core.Database.Config.Concrete;

namespace Thabe.Bot.Core.Database.Config;


/// <summary>
/// COnfig管理类
/// </summary>
public static class ConfigManager
{
    private static readonly WebConfig WEB_CONFIG = ThabeBot.Config.GetOrDefault<WebConfig>(nameof(WEB_CONFIG), new()
    {
        GiteeCloudUrl = "https://gitee.com/DathaBe/thabe-library/raw/master/Thabe.Bot.Cloud/"
    });


    /// <summary>
    /// 从Gitee仓库读取Json配置
    /// </summary>
    /// <param name="dataPath">以仓库为根目录 只需要 root/data.json</param>
    /// <returns></returns>
    public static WebJsonConfig GetGiteeCloudConfig(string dataPath)
        => new($"{WEB_CONFIG.GiteeCloudUrl}{dataPath}");

    /// <summary>
    /// 获取本地配置文件
    /// </summary>
    public static LocalJsonConfig GetLocalConfig(string name, string? path = null)
        => new(name, path);


    class WebConfig
    {
        [JsonProperty("gitee_cloud_url")]
        public required string GiteeCloudUrl { get; init; }
    }
}
