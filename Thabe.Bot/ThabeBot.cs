using Mirai.Net.Sessions;
using Spectre.Console;
using Thabe.Bot.Core.Bot;
using Thabe.Bot.Core.Logger;
using Thabe.Bot.Core.Preload;
using Thabe.Bot.Util;

namespace Thabe.Bot;


/// <summary>
/// ThabeBot
/// </summary>
public partial class ThabeBot
{
    #region --私有--

    /// <summary>
    /// 打印BOT LOGO
    /// </summary>
    static ThabeBot() => AnsiConsole.Write(new FigletText(Config.BOT_LOGO));


    /// <summary>
    /// 连接锁
    /// </summary>
    private readonly static object _lock = new();

    /// <summary>
    /// 初始化Bot
    /// </summary>
    /// <param name="info">连接信息</param>
    /// <exception cref="Exception">无法创建Bot</exception>
    private ThabeBot(ConnectInfo info)
    {
        if (Instance != null)
        {
            throw new Exception($"无法重复创建连接信息, 因为已创建{Instance.Core.QQ}");
        }

        Instance = this;

        Core = new()
        {
            QQ = info.QQ,
            Address = info.Addres,
            VerifyKey = info.VerifyKey
        };

        $"ThabeBot[[{Core.QQ.ColorHighlight()}]] 向 [[{"Mirai-Http-API".ColorHighlight()}]] 申请握手!".LogTips();

        Core.LaunchAsync().GetAwaiter().GetResult();

        "达成协议!".LogImportant();

        Config.Set("bot_session", info);
        Config.Save();
    }

    /// <summary>
    /// 连接 Mirai HTTP API
    /// </summary>
    /// <param name="getConcentInfo">返回一个连接信息</param>
    /// <param name="isRetry">连接异常处理, int:这是第几次连接, Exception:连接异常内容,  返回是否继续连接></param>
    /// <returns>是否连接成功</returns>
    private static Task ConnectAsync(Func<ConnectInfo> getConcentInfo, Func<int, Exception, bool> isRetry)
    {
        return Task.Run(() =>
        {
            lock (_lock)
            {
                for (int retry_count = 1; ; retry_count++)
                {
                    try
                    {
                        Instance = new(getConcentInfo());
                        break;
                    }
                    catch (Exception ex)
                    {
                        if (!isRetry(retry_count, ex)) return;
                    }
                }

                //发送连接信息给主人QQ和打印在控制台
                ThabeBotUtil.SendConcentInfo();

                //预加载内容
                Instance.CallPreloadContent();
            }
        });
    }

    #endregion

    /// <summary>
    /// ThabeBot实例
    /// </summary>
    public static ThabeBot? Instance { get; private set; }


    /// <summary>
    /// ThabeBot核心Mirai.Net Bot
    /// </summary>
    public MiraiBot Core { get; }


    /// <summary>
    /// 在控制台输入信息并连接
    /// </summary>
    public static Task ConnectAsync()
    {
        return ConnectAsync(GetConnectInfo, ConnectException);

        static ConnectInfo GetConnectInfo()
        {
            var info = Config.GetOrNull<ConnectInfo>("bot_session");
            if (info != null) return info;

            return new()
            {
                QQ = AnsiConsole.Ask<string>($"请输入{"QQ".ColorTips()}>").Trim(),
                Addres = AnsiConsole.Ask<string>($"请输入{"MiraiHttpApi地址".ColorTips()}>").Trim(),
                VerifyKey = AnsiConsole.Ask<string>($"请输入{"验证码".ColorTips()}>").Trim()
            };
        }

        static bool ConnectException(int retryCount, Exception ex)
        {
            ex.Message.LogError();

            return AnsiConsole.Ask<string>($"是否需要{"重新登录".ColorTips()}[[y]]>").ToLower() != "y";
        }
    }

    /// <summary>
    /// 输入连接信息并连接
    /// </summary>
    /// <param name="info">连接信息</param>
    public static Task ConnectAsync(ConnectInfo info)
    {
        return ConnectAsync(() => info, ConnectException);


        static bool ConnectException(int retryCount, Exception ex)
        {
            ex.Message.LogError();

            return false;
        }
    }

    /// <summary>
    /// 阻塞当前线程 不要结束
    /// </summary>
    public static void Wait()
    {
        while (true) Console.ReadLine();
    }
}
