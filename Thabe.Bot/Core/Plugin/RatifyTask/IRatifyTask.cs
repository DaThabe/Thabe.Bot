using Mirai.Net.Data.Messages;

namespace Thabe.Bot.Core.Plugin.RatifyTask;


/// <summary>
/// 批准任务接口
/// </summary>
public interface IRatifyTask
{
    /// <summary>
    /// 任务句柄
    /// </summary>
    string Handel { get; }

    /// <summary>
    /// 同意
    /// </summary>
    void Agree();

    /// <summary>
    /// 拒绝
    /// </summary>
    void Refuse();
}
