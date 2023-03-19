namespace Thabe.Bot.Core.Plugin;


/// <summary>
/// 插件特性
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class PluginAttribute : Attribute
{
    public string PackName { get; init; }

    /// <summary>
    /// 插件名称
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// 初始化插件
    /// </summary>
    /// <param name="name"></param>
    public PluginAttribute(string packName, string name)
    {
        (PackName, Name) = (packName, name);
    }
}