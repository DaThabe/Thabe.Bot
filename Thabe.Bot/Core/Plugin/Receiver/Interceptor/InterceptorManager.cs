using Mirai.Net.Data.Messages;
using Thabe.Bot.Util;

namespace Thabe.Bot.Core.Plugin.Receiver.Interceptor;



/// <summary>
/// 接收器拦截器
/// </summary>
public static class InterceptorManager
{
    /// <summary>
    /// 拦截器集合
    /// </summary>
    private static readonly List<InterceptorHandel> _INTERCEPTORS = new();

    /// <summary>
    /// 拦截器集合
    /// </summary>
    public static IEnumerable<InterceptorHandel> Interceptors => _INTERCEPTORS;


    /// <summary>
    /// 设置拦截器 拦截成功接下来的Reciver将不再执行
    /// </summary>
    public static bool SetInterceptor(this MessageReceiverBase receiver, Func<bool> isContinue)
    {
        var handel = receiver.GetSenderHandel();
        if (handel is not { Length: > 0 } id) return false;

        if (_INTERCEPTORS.FindAll(x => x.CreatorHandel == handel).Count != 0) return false;

        _INTERCEPTORS.Add(new() { CreatorHandel = handel, IsContinue = isContinue });
        return true;
    }

    /// <summary>
    /// 获取拦截器
    /// </summary>
    public static InterceptorHandel? GetInterceptor(this MessageReceiverBase receiver)
    {
        if (receiver.GetSenderHandel() is { Length: > 0 } id)
        {
            return _INTERCEPTORS.FirstOrDefault(x => x.CreatorHandel == id);
        }

        return null;
    }

    /// <summary>
    /// 释放拦截器
    /// </summary>
    public static bool Release(this InterceptorHandel interceptor)
    {
        return _INTERCEPTORS.Remove(interceptor);
    }
}
