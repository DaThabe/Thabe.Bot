using Mirai.Net.Data.Messages.Concretes;
using Mirai.Net.Data.Messages.Receivers;
using Mirai.Net.Data.Messages;
using Spectre.Console;
using System.Text;
using Thabe.Bot.Util;

namespace Thabe.Bot.Core.Logger;

public static class LoggerUtil
{
    /// <summary>
    /// 写入控制台
    /// </summary>
    /// <param name="str"></param>
    /// <param name="newLine"></param>
    public static void TryWriteAsync(string str, bool newLine = false)
    {
        Task.Run(() =>
        {
            var log_info = $"{str} {(newLine ? "\n" : "")}";

            try { AnsiConsole.Markup(log_info); } catch { Console.Write(log_info); }
        });
    }

    /// <summary>
    /// 记录日志
    /// </summary>
    public static void Log(this string str, LogLevel level = LogLevel.Info)
    {
        if (string.IsNullOrWhiteSpace(str)) return;

        StringBuilder sb = new();

        sb.Append($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}]".Color(level));
        sb.Append(str);

        TryWriteAsync(sb.ToString(), true);
    }
    public static void Log(this IEnumerable<string> strs, LogLevel level = LogLevel.Info)
    {
        StringBuilder sb = new();

        sb.Append($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}]".Color(level));
        foreach (var str in strs) sb.Append(str);

        TryWriteAsync(sb.ToString(), true);
    }


    /// <summary>
    /// 警告级别日志
    /// </summary>
    public static void LogWarning(this string str) => str.Log(LogLevel.Warning);
    /// <summary>
    /// 错误级别日志
    /// </summary>
    public static void LogError(this string str) => str.Log(LogLevel.Error);
    /// <summary>
    /// 提示级别日志
    /// </summary>
    /// <param name="str"></param>
    public static void LogTips(this string str) => str.Log(LogLevel.Tips);
    /// <summary>
    /// 不重要的日志
    /// </summary>
    public static void LogUnimportance(this string str) => str.Log(LogLevel.Unimportance);
    /// <summary>
    /// 重要日志
    /// </summary>
    public static void LogImportant(this string str) => str.Log(LogLevel.Important);




    /// <summary>
    /// 记录Bot消息
    /// </summary>
    /// <param name="receiver"></param>
    /// <param name="sendMessage"></param>
    public static void LogBotMessage(this MessageReceiverBase receiver, MessageChain? sendMessage = null)
    {
        var msg_chain = sendMessage ?? receiver.MessageChain;

        List<string> msgs = new()
        {
            ReceiverName(receiver).ColorHighlight(),
            (sendMessage != null ? " -> " : " <- ").ColorTips(),
            MessageChianText(msg_chain)
        };

        msgs.Log();


        static string ReceiverName(MessageReceiverBase receiver) => receiver switch
        {
            GroupMessageReceiver g => $"群组({g.GroupId})",
            FriendMessageReceiver f => $"好友({f.FriendId})",
            TempMessageReceiver t => $"临时{t.GroupId}",
            _ => "未知消息"
        };

        static string MessageChianText(MessageChain msg)
        {
            StringBuilder sb = new();

            foreach (var i in msg.ToArray())
            {
                sb.Append(i switch
                {
                    PlainMessage plain => plain.Text,
                    ImageMessage img => $"[[Image: {img.Url}]]",
                    AtMessage at => $"[[At: {at.Target}]]",
                    _ => $"[[{i.GetType().Name}]]"
                });
            }

            return sb.ToString();
        }
    }
}


/// <summary>
/// 日志等级
/// </summary>
public enum LogLevel
{
    /// <summary>
    /// 不重要
    /// </summary>
    Unimportance,

    /// <summary>
    /// 信息
    /// </summary>
    Info,

    /// <summary>
    /// 提示
    /// </summary>
    Tips,

    /// <summary>
    /// 重要
    /// </summary>
    Important,

    /// <summary>
    /// 警告
    /// </summary>
    Warning,

    /// <summary>
    /// 错误
    /// </summary>
    Error
}