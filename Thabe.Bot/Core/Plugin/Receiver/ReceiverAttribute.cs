namespace Thabe.Bot.Core.Plugin.Receiver;


/// <summary>
/// 插件消息接收器特性
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class ReceiverAttribute : Attribute
{
    /// <summary>
    /// 执行优先级, 数字越大越低 默认都很高
    /// </summary>
    public int Level { get; init; } = 0;


    /// <summary>
    /// 接收器描述
    /// </summary>
    public string? Describe { get; init; }
}
