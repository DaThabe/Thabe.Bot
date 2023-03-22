using Mirai.Net.Data.Messages;
using Thabe.Bot.Util;

namespace Thabe.Bot.Core.Plugin.Receiver.Dispatch.Context;


/// <summary>
/// 接收器上下文句柄
/// </summary>
public class ContextHandel
{
    /// <summary>
    /// 顺序响应泪飙
    /// </summary>
    private readonly List<ContextNode> _contextNodes = new();

    /// <summary>
    /// 强制结束
    /// </summary>
    private bool _forcedFinish = false;


    /// <summary>
    /// 当前执行动作步数
    /// </summary>
    public int CurrentStep { get; private set; } = 0;

    /// <summary>
    /// 最大步数
    /// </summary>
    public int MaxStep => _contextNodes.Count - 1;

    /// <summary>
    /// 创建者句柄
    /// </summary>
    public string CreatorHandel { get; private set; }



    /// <summary>
    /// 创建插件上下文
    /// </summary>
    /// <param name="receiver"></param>
    /// <param name="action"></param>
    public ContextHandel(ContextNode node)
    {
        CreatorHandel = node.Receiver.GetSenderHandel() ??
            throw new ArgumentException("无法获取发送者Id", nameof(node));
        Add(node);
    }



    /// <summary>
    /// 添加下一个响应
    /// </summary>
    /// <param name="receiver"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public bool Add(ContextNode resp)
    {
        var sender_handel = resp.Receiver.GetSenderHandel();
        if (sender_handel != CreatorHandel) return false;

        _contextNodes.Add(resp);

        return true;
    }


    /// <summary>
    /// 移动到下一个节点
    /// </summary>
    public bool MoveNext()
    {
        if (CurrentStep >= _contextNodes.Count) return false;

        CurrentStep++;
        return true;
    }

    /// <summary>
    /// 移动到上一个节点
    /// </summary>
    public bool MovePrev()
    {
        if (CurrentStep <= 0) return false;

        CurrentStep--;
        return true;
    }

    /// <summary>
    /// 移动至指定步数
    /// </summary>
    public bool Move(int step)
    {
        if (step >= 0 && step <= MaxStep)
        {
            CurrentStep = step;
            return true;
        }

        return false;
    }


    /// <summary>
    /// 上下文是否已结束
    /// </summary>
    public bool IsFinish() => CurrentStep >= _contextNodes.Count || _forcedFinish;

    /// <summary>
    /// 执行当前节点并且在执行完之后往后移动一个节点
    /// </summary>
    public bool Continue(MessageReceiverBase receiver)
    {
        if (IsFinish()) return false;

        _contextNodes[CurrentStep++]?.Action?.Invoke(receiver);
        return true;
    }

    public void Finish() => _forcedFinish = true;
}