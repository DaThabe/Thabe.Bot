using Mirai.Net.Data.Messages;
using Thabe.Bot.Util;

namespace Thabe.Bot.Core.Plugin.Receiver.Dispatch.Context;


/// <summary>
/// 插件上下文集合
/// </summary>
public static class ContextManager
{
    /// <summary>
    /// 插件上下文列表
    /// </summary>
    private static readonly List<ContextHandel> _CONTEXTS = new();

    /// <summary>
    /// 上下文集合
    /// </summary>
    public static IEnumerable<ContextHandel> Contexts => _CONTEXTS;


    /// <summary>
    /// 获取插件上下文
    /// </summary>
    /// <param name="receiver"></param>
    /// <returns></returns>
    public static ContextHandel? GetContext(this MessageReceiverBase receiver)
    {
        var receiver_id = receiver.GetSenderHandel();
        return _CONTEXTS.FirstOrDefault(x => x.CreatorHandel == receiver_id);
    }

    /// <summary>
    /// 添加下一个待执行动作
    /// </summary>
    /// <param name="receiver"></param>
    public static bool AddContext(this MessageReceiverBase receiver, Action<MessageReceiverBase> action)
    {
        ReleaseContext();

        var sender_handel = receiver.GetSenderHandel();
        if (sender_handel == null) return false;

        //获取上下文句柄
        var context_handel = _CONTEXTS.Find(x => x.CreatorHandel == sender_handel);
        ContextNode node = new(receiver, action);

        //句柄不存在 创建新的直接返回
        if (context_handel == null)
        {
            context_handel = new(node);
            _CONTEXTS.Add(context_handel);
            return true;
        }

        //添加上下文节点
        context_handel.Add(node);
        return true;
    }


    /// <summary>
    /// 释放已完成的上下文
    /// </summary>
    private static void ReleaseContext()
        => _CONTEXTS.RemoveAll(x => x.IsFinish());
}