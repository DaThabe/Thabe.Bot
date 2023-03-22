using Mirai.Net.Data.Messages;
using Thabe.Bot.Core.Plugin.Receiver.Dispatch.Context;
using Thabe.Bot.Core.Plugin.Receiver.Interceptor;

namespace Thabe.Bot.Core.Plugin.Receiver.Dispatch;



/// <summary>
/// 插件调度管理类
/// </summary>
public static class DispatchManager
{
    /// <summary>
    /// 排序后的接收器句柄
    /// </summary>
    private static List<ReceiverHandel> _sortedReceiverHandels = new();

    /// <summary>
    /// 已排序的接收器句柄
    /// </summary>
    public static IEnumerable<ReceiverHandel> SortedReceivers => _sortedReceiverHandels;


    /// <summary>
    /// 初始化接收器句柄
    /// </summary>
    static DispatchManager() => ReloadReceiverHandels();


    /// <summary>
    /// 添加到任务列表
    /// </summary>
    /// <param name="receiver"></param>
    public static void AddToTaskList(this MessageReceiverBase receiver)
    {
        if (receiver.GetContext() is ContextHandel context)
        {
            //执行指定的上下
            context.Continue(receiver);
            return;
        }

        //按照先后顺序调用
        foreach (var x in _sortedReceiverHandels)
        {
            x.Receive(receiver);

            var interceptor = receiver.GetInterceptor();

            //如果有拦截器 就直接退出
            if (interceptor?.IsContinue() == false)
            {
                interceptor.Release();
                return;
            }
        }
    }

    /// <summary>
    /// 刷新接收器句柄并重新排序
    /// </summary>
    public static void ReloadReceiverHandels()
    {
        var values = PluginManager.Receivers.ToList();
        if (values == null) return;

        _sortedReceiverHandels = values;
        _sortedReceiverHandels.Sort((x, y) => x.MetaInfo.Level.CompareTo(y.MetaInfo.Level));
    }
}
