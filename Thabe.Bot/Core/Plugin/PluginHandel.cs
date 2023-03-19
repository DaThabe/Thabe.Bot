namespace Thabe.Bot.Core.Plugin;


/// <summary>
/// 一个插件句柄
/// </summary>
public class PluginHandel
{
    /// <summary>
    /// 插件类 类型
    /// </summary>
    public required Type PluginType { get; init; }

    /// <summary>
    /// 插件元数据
    /// </summary>
    public required PluginAttribute PluginMeta { get; init; }
}
