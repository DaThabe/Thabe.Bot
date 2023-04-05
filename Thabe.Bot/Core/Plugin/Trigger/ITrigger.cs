using Mirai.Net.Data.Messages;

namespace Thabe.Bot.Core.Plugin.Trigger;


/// <summary>
/// 触发器接口
/// </summary>
public interface ITrigger<T>
{
    /// <summary>
    /// 匹配结果
    /// </summary>
    T? Match(MessageReceiverBase recevier);
}