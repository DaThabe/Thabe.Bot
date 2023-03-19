namespace Thabe.Bot.Core.Database.Config;



/// <summary>
/// 配置数据 通常内容没有固定格式
/// </summary>
public interface IConfig
{
    /// <summary>
    /// 获取一个键值, 如果不存在则使用默认值, 日志会提示是否使用了默认值
    /// </summary>
    public T GetOrDefault<T>(string key, T defaultValue) where T : notnull;


    /// <summary>
    /// 获取一个键值, 如果不存在则返回null
    /// </summary>
    public T? GetOrNull<T>(string key);


    /// <summary>
    /// 设置一个配置
    /// </summary>
    public void Set<T>(string key, T data) where T : notnull;
}
