using Mirai.Net.Data.Messages;
using Mirai.Net.Data.Messages.Concretes;
using Mirai.Net.Data.Messages.Receivers;
using Mirai.Net.Sessions.Http.Managers;
using Mirai.Net.Utils.Scaffolds;
using Thabe.Bot.Core.Database.FileHelper;
using Thabe.Bot.Core.Logger;

namespace Thabe.Bot.Util;


/// <summary>
/// 聊天扩展方法
/// </summary>
public static class MiraiNetUtil
{
    /// <summary>
    /// 引用回复消息
    /// </summary>
    private static async Task QuoteMessageAsync(this MessageReceiverBase receiver, MessageChain chain, int recallTime = 0)
    {
        try
        {
            string msg_id = "", id = "";

            if (receiver is GroupMessageReceiver group)
            {
                id = group.GroupId;
                msg_id = await MiraiScaffold.QuoteMessageAsync(group, chain);
            }
            else if (receiver is FriendMessageReceiver friend)
            {
                id = friend.FriendId;
                msg_id = await MiraiScaffold.QuoteMessageAsync(friend, chain);
            }

            if (recallTime != 0)
            {
                await Task.Delay(recallTime);
                await MessageManager.RecallAsync(msg_id, id);
            }

            receiver.LogBotMessage(chain);
        }
        catch (Exception e)
        {
            e.Message.LogError();
        }
    }


    /// <summary>
    /// 回复消息
    /// </summary>
    /// <param name="receiver"></param>
    /// <param name="chain"></param>
    private static async Task ReplyAsync(this MessageReceiverBase receiver, MessageChain chain, int recallTime = 0)
    {
        try
        {
            string msg_id = "", id = "";

            if (receiver is GroupMessageReceiver group)
            {
                id = group.GroupId;
                msg_id = await group.SendMessageAsync(chain);
            }
            else if (receiver is FriendMessageReceiver friend)
            {
                id = friend.FriendId;
                msg_id = await friend.SendMessageAsync(chain);
            }

            if (recallTime != 0)
            {
                await Task.Delay(recallTime);
                await MessageManager.RecallAsync(msg_id, id);
            }

            receiver.LogBotMessage(chain);
        }
        catch (Exception e)
        {
            e.Message.LogError();
        }
    }

    /// <summary>
    /// 用消息链回复
    /// </summary>
    /// <param name="chain">消息链</param>
    /// <param name="replyMode">回复模式</param>
    /// <param name="recallTime">撤回消息时间</param>
    public static async Task ReplyAsync(this MessageReceiverBase receiver, MessageChain chain, Replys replyMode = Replys.None, int recallTime = 0)
    {
       MessageChainBuilder mcb = new();

        if (receiver is GroupMessageReceiver group && (replyMode & Replys.At) == Replys.At)
        {
            mcb.At(group.Sender);
        }
        if ((replyMode & Replys.Recall) != Replys.Recall)
        {
            recallTime = 0;
        }


        chain = mcb.Build() + chain;

        if ((replyMode & Replys.Quote) == Replys.Quote)
        {
            await receiver.QuoteMessageAsync(chain, recallTime);
        }
        else
        {
            await receiver.ReplyAsync(chain, recallTime);
        }
    }

    /// <summary>
    /// 回复文本消息
    /// </summary>
    /// <param name="message">消息</param>
    /// <param name="replyMode">回复模式</param>
    /// <param name="recallTime">撤回时间</param>
    public static async Task ReplyAsync(this MessageReceiverBase receiver, string? message, Replys replyMode = Replys.None, int recallTime = 0)
    {
        if (message == null) return;

        MessageChainBuilder chain = new();
        chain.Plain(message);

        await receiver.ReplyAsync(chain.Build(), replyMode, recallTime);
    }

    /// <summary>
    /// 回复一个消息
    /// </summary>
    public static async Task ReplyAsync(this MessageReceiverBase receiver, MessageBase message, Replys replyMode = Replys.None, int recallTime = 0)
    {
        if (message == null) return;

        MessageChainBuilder chain = new();
        chain.Append(message);

        await receiver.ReplyAsync(chain.Build(), replyMode, recallTime);
    }


    /// <summary>
    /// 回复一个网络图片消息
    /// </summary>
    /// <param name="receiver"></param>
    /// <param name="url"></param>
    /// <returns></returns>
    public static async Task ReplyWebImageAsync(this MessageReceiverBase receiver, string url)
    {
        var b64 = await url.CacheWebFileToBase64();
        await receiver.ReplyAsync(new ImageMessage() { Base64 = b64 });
    }


    /// <summary>
    /// 获取发送者Id  群聊的自己和私聊的自己一样
    /// </summary>
    public static string? GetSenderId(this MessageReceiverBase receiver)
    {
        return receiver switch
        {
            FriendMessageReceiver f => f.FriendId,
            GroupMessageReceiver g => g.Sender.Id,
            TempMessageReceiver t => t.Sender.Id,
            OtherClientMessageReceiver o => o.Sender.Id.ToString(),
            _ => null
        };
    }

    /// <summary>
    /// 获取发送者Handel  群聊的自己和私聊的自己不一样
    /// </summary>
    public static string? GetSenderHandel(this MessageReceiverBase receiver)
    {
        return receiver switch
        {
            FriendMessageReceiver f => f.FriendId,
            GroupMessageReceiver g => $"{g.GroupId}-{g.Sender.Id}",
            TempMessageReceiver t => t.Sender.Id,
            OtherClientMessageReceiver o => o.Sender.Id.ToString(),
            _ => null
        };
    }
}


[Flags]
public enum Replys
{
    /// <summary>
    /// 默认模式
    /// </summary>
    None = 0,

    /// <summary>
    /// 引用
    /// </summary>
    Quote = 1,

    /// <summary>
    /// @
    /// </summary>
    At = 2,

    /// <summary>
    /// 撤回
    /// </summary>
    Recall = 4
}
