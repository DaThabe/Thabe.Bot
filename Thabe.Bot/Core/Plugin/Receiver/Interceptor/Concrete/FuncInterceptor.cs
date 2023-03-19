namespace Thabe.Bot.Core.Plugin.Receiver.Interceptor.Concrete;


/// <summary>
/// 用方法实现的接收器拦截器接口
/// </summary>
public class FuncInterceptor : IInterceptor
{
    private Func<bool> _func;

    public FuncInterceptor(Func<bool> func) => _func = func;

    public bool IsContinue() => _func();
}
