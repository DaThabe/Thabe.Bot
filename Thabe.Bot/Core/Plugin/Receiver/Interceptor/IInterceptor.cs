namespace Thabe.Bot.Core.Plugin.Receiver.Interceptor;


/// <summary>
/// 接收器拦截器
/// </summary>
public interface IInterceptor
{
    /// <summary>
    /// 是否继续执行
    /// </summary>
    bool IsContinue();
}
