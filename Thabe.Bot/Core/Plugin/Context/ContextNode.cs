using Mirai.Net.Data.Messages;

namespace Thabe.Bot.Core.Plugin.Context;


/// <summary>
/// 插件上下文相应节点
/// </summary>
public class ContextNode
{
    /// <summary>
    /// 接收器数据
    /// </summary>
    public MessageReceiverBase Receiver { get; set; }

    /// <summary>
    /// 动作方法
    /// </summary>
    public Action<MessageReceiverBase> Action { get; set; }


    /// <summary>
    /// 初始化
    /// </summary>
    public ContextNode(MessageReceiverBase receiver, Action<MessageReceiverBase> action)
    {
        (Receiver, Action) = (receiver, action);
    }
}