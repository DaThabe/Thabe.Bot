namespace Thabe.Bot.Core.Plugin.Receiver.Interceptor;


/// <summary>
/// 拦截器句柄
/// </summary>
public class InterceptorHandel
{
    /// <summary>
    /// 创建者句柄
    /// </summary>
    public required string CreatorHandel { get; init; }

    /// <summary>
    /// 是否继续执行
    /// </summary>
    public required Func<bool> IsContinue { get; init; }
}
