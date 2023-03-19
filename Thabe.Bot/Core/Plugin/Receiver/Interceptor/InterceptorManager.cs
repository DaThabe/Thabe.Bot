using System.Collections.Concurrent;
using Mirai.Net.Data.Messages;
using Thabe.Bot.Core.Plugin.Receiver.Interceptor.Concrete;
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
    private static readonly ConcurrentDictionary<string, IInterceptor> _INTERCEPTORS = new();


    /// <summary>
    /// 设置拦截器 拦截成功接下来的Reciver将不再执行
    /// </summary>
    public static void SetInterceptor(this MessageReceiverBase receiver, Func<bool> func)
        => receiver.SetInterceptor(new FuncInterceptor(func));

    /// <summary>
    /// 设置拦截器 拦截成功接下来的Reciver将不再执行
    /// </summary>
    public static bool SetInterceptor(this MessageReceiverBase receiver, IInterceptor interceptor)
    {
        if(receiver.GetSenderHandel() is { Length : > 0} id)
        {
            _INTERCEPTORS.AddOrUpdate(id, interceptor, (k, v) => v);
            return true;
        }
        return false;
    }


    /// <summary>
    /// 获取拦截器
    /// </summary>
    public static IInterceptor? GetInterceptor(this MessageReceiverBase receiver)
    {
        if (receiver.GetSenderHandel() is { Length: > 0 } id)
        {
            _INTERCEPTORS.TryGetValue(id, out IInterceptor? value);
            return value;
        }

        return null;
    }


    /// <summary>
    /// 释放拦截器
    /// </summary>
    public static bool Release(this IInterceptor interceptor)
    {
        return _INTERCEPTORS.Values.Remove(interceptor);
    }
}
