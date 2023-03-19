using Thabe.Bot.Core.Logger;
using Thabe.Bot.Util;

namespace Thabe.Bot.Core.Preload;


/// <summary>
/// 预加载管理类
/// </summary>
public static class PreloadManager
{
    /// <summary>
    /// 预处理动作列表
    /// </summary>
    private readonly static List<IPreload> _ACTIONS = new();


    /// <summary>
    /// 预先读取所有预加载类型
    /// </summary>
    static PreloadManager()
    {
        CSahrpUtil.GetAllTypes().OfType<IPreload>().Foreach(x =>
        {
            if (x.IsInterface || x.IsAbstract) return;
            if (typeof(ActionPreload).IsAssignableFrom(x)) return;
            if (Activator.CreateInstance(x) is not IPreload prestrain) return;

            _ACTIONS.Add(prestrain);
        });
    }

    /// <summary>
    /// 呼叫预加载内容
    /// </summary>
    internal static void CallPreloadContent(this ThabeBot instance)
    {
        //并发加载
        Parallel.ForEach(_ACTIONS, prestrain =>
        {
            try
            {
                IPreload.Logger logger = new();

                prestrain.Init(instance, logger);

                $"预处理事件: {prestrain.Describe} {logger}".LogUnimportance();
            }
            catch (Exception ex)
            {
                ex.ToString().LogError();
            }
        });
    }


    /// <summary>
    /// 添加一个预处理动作
    /// </summary>
    public static void AddPreloadContent(string describe, Action<ThabeBot, IPreload.Logger> action)
    {
        _ACTIONS.Add(new ActionPreload()
        {
            Action = action,
            Describe = describe
        });
    }
}


/// <summary>
/// 用方法实现预加载接口
/// </summary>
file class ActionPreload : IPreload
{
    public required string Describe { get; init; }

    public required Action<ThabeBot, IPreload.Logger> Action { get; init; }

    public void Init(ThabeBot bot, IPreload.Logger logger)
        => Action(bot, logger);
}