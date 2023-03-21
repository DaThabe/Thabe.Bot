using Mirai.Net.Data.Messages;
using Thabe.Bot.Cloud.Plugin.thabe.bot.plugin.NyaChat.Vanilla.Model;
using Thabe.Bot.Core.Bot;
using Thabe.Bot.Core.Logger;
using Thabe.Bot.Core.Preload;
using Thabe.Bot.Util;
using Thabe.Kit.EasyChatGPT;

namespace Thabe.Bot.Cloud.Plugin.thabe.bot.plugin.NyaChat.Vanilla.Service;


/// <summary>
/// 聊天上下文
/// </summary>
public static class Chatmanager
{
    /// <summary>
    /// 释放上下文锁
    /// </summary>
    private static readonly object _releaseLock = new();

    /// <summary>
    /// ChatGPT API
    /// </summary>
    private static readonly ChatGPTClient? _CHATGPT_CLIENT;

    /// <summary>
    /// 上下文集合
    /// </summary>
    private static readonly List<ChatContext> _CONTEXTS = new();


    static Chatmanager()
    {
        var api_key = ThabeBot.Config.GetOrNull<string?>("chat_gpt_apikey");
        if (api_key == null)
        {
            "ChatGPT API_KEY 缺失 无法使用Chat GPT!".LogError();
            return;
        }

        _CHATGPT_CLIENT = new(api_key);
    }


    /// <summary>
    /// 发送主人问候消息
    /// </summary>
    public static async void SendMasterMessage()
    {
        if (_CHATGPT_CLIENT == null) return;

        MessageBuilder mb = new();
        mb.AddSystemMessage(ChatContext.GetVanillaInfo());
        mb.AddUserMessage("如果你明白我的意思, 请回复你今天的状态!");

        var message = await _CHATGPT_CLIENT.SendMessagesAsync(mb);

        message.LogImportant();
        await ThabeBotUtil.SendMasterMessageAsync(message);
    }


    /// <summary>
    /// 获取回复
    /// </summary>
    public static async void ChatGPTReply(this MessageReceiverBase receiver)
    {
        if (_CHATGPT_CLIENT == null) return;

        var handel = receiver.GetSenderHandel();
        if (handel == null) return;

        var context = _CONTEXTS.Find(x => x.SenderHandel == handel);

        //没有上下文就创建
        if (context == null)
        {
            context = new(_CHATGPT_CLIENT, receiver);
            _CONTEXTS.Add(context);
        }

        //回复内容
        await context.ReplyAsync(receiver);

        //执行释放上下文任务
        await Task.Run(ReleaseContenxt);
    }

    /// <summary>
    /// 释放已超时的上下文
    /// </summary>
    private static void ReleaseContenxt()
    {
        if (!Monitor.TryEnter(_releaseLock, TimeSpan.Zero))
        {
            return;
        }

        try
        {
            //释放
            var timeout_context = _CONTEXTS.FindAll(x => x.IsTimeout);

            foreach(var i in timeout_context)
            {
                _CONTEXTS.Remove(i);
                i.Dispose();
            }
        }
        finally
        {
            Monitor.Exit(_releaseLock);
        }
    }
}


file class ChatGPTClinetPreload : IPreload
{
    public string Describe => "Chat GPT 客户端预创建";

    public void Init(ThabeBot bot, IPreload.Logger sb) => Chatmanager.SendMasterMessage();
}