namespace Thabe.Bot.Core.Database;


/// <summary>
/// 表示一个可持久化数据
/// </summary>
public interface IPersistentData
{
    /// <summary>
    /// 拉取数据
    /// </summary>
    void Pull();


    /// <summary>
    /// 推送数据
    /// </summary>
    void Push();
}
