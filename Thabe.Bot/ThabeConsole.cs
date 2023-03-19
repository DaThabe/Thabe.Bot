using Mirai.Net.Sessions.Http.Managers;
using Spectre.Console;
using Thabe.Bot.Core.Logger;

namespace Thabe.Bot;


/// <summary>
/// ThabeBot唯一实例
/// </summary>
public static class ThabeConsole
{
    /// <summary>
    /// 初始化中事件
    /// </summary>
    public static event Action<IThabePreload.Logger>? Prestraing;


    static ThabeConsole()
    {
        AnsiConsole.Write(new FigletText("ThabeBot"));

        //Console.WriteLine(ThabeBot.Config.BOT_LOGO_TEXT);
    }

    /// <summary>
    /// 连接Bot
    /// </summary>
    public static void Run()
    {
        //连接Bot
        ThabeBot.Instance.LaunchAsync().GetAwaiter().GetResult();

        //发送连接信息
        SendConcentInfo().GetAwaiter().GetResult();

        //预加载内容
        ExcutePrestrain();

        //TODO: 可以加一个指令分析
        while (true) Console.ReadLine();
    }

    /// <summary>
    /// 发送连接信息
    /// </summary>
    /// <returns></returns>
    private static async Task SendConcentInfo()
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
        await MessageManager.SendFriendMessageAsync(ThabeBot.Config.BOT_MASTER, concent_info);
    }

    /// <summary>
    /// 读取预加载内容
    /// </summary>
    private static void ExcutePrestrain()
    {
        IThabePreload.Logger logger = new();
        Prestraing?.Invoke(logger);
        logger.ToString().LogUnimportance();

        CSahrpUtil.GetAllTypes().OfType<IThabePreload>().TryForeachAsync(x =>
        {
            if (x.IsInterface || x.IsAbstract) return;
            if (Activator.CreateInstance(x) is not IThabePreload prestrain) return;

            try
            {
                IThabePreload.Logger logger = new();

                prestrain.Init(ThabeBot.Instance, logger);

                $"预处理事件: {prestrain.Describe} {logger}".LogUnimportance();
            }
            catch (Exception ex)
            {
                ex.ToString().LogError();
            }
        });

        "所有预处理事件已完成!".LogTips();
    }
}