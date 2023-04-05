using Mirai.Net.Data.Messages;
using Mirai.Net.Sessions.Http.Managers;
using Thabe.Bot.Core.Logger;

namespace Thabe.Bot.Core.Bot;



/// <summary>
/// ThabeBot
/// </summary>
public static class ThabeBotUtil
{
    /// <summary>
    /// 发送消息给主人QQ
    /// </summary>
    /// <returns></returns>
    public static async Task<string> SendMasterMessageAsync(string message)
    {
        return await MessageManager.SendFriendMessageAsync(ThabeBot.Config.BOT_MASTER, message);
    }

    /// <summary>
    /// 发送消息给主人QQ
    /// </summary>
    /// <param name="chain"></param>
    /// <returns></returns>
    public static async Task<string> SendMasterMessageAsync(this MessageBase messages)
    {
        return await MessageManager.SendFriendMessageAsync(ThabeBot.Config.BOT_MASTER, messages);
    }

    /// <summary>
    /// 发送消息给主人QQ
    /// </summary>
    /// <param name="chain"></param>
    /// <returns></returns>
    public static async Task<string> SendMasterMessageAsync(this MessageChain chain)
    {
        return await MessageManager.SendFriendMessageAsync(ThabeBot.Config.BOT_MASTER, chain);
    }


    /// <summary>
    /// 发送连接信息
    /// </summary>
    internal static void SendConcentInfo()
    {
        var concent_info = $"""
            ThabeBot 连接成功!
            连接时间: {DateTime.Now:yyyy-MM-dd HH:mm:ss}
            上次连接: {ThabeBot.Config.LAST_LAUNCH_TIME:yyyy-MM-dd HH:mm:ss}

            核心版本: {ThabeBot.Config.BOT_VERSION}
            客户端用户: {Environment.MachineName}/{Environment.UserName}
            客户端系统: {Environment.OSVersion.Version}
            """;

        ThabeBot.Config.LAST_LAUNCH_TIME = DateTime.Now;
        ThabeBot.Config.Save();

        $"{concent_info}\n\n".LogImportant();
        SendMasterMessageAsync(concent_info).GetAwaiter().GetResult();
    }
}
