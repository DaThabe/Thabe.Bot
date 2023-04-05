using Mirai.Net.Data.Messages;

namespace Thabe.Bot.Core.Plugin.Trigger;


/// <summary>
/// 触发器管理类
/// </summary>
public static class TriggerManager
{
    private static readonly Dictionary<Type, object> _TRIGGERS = new();

    /// <summary>
    /// 获取匹配结果
    /// </summary>
    /// <typeparam name="T">该类型必须实现<see cref="ITrigger{T}"/>接口</typeparam>
    /// <param name="receiver"></param>
    /// <returns></returns>
    public static T? GetTriggerResult<T>(this MessageReceiverBase receiver)
    {
        var type = typeof(T);

        if (!_TRIGGERS.ContainsKey(type))
        {
            _TRIGGERS[type] = Activator.CreateInstance<T>()
                ?? throw new ArgumentNullException(nameof(type), "触发器创建失败!");
        }

        if (_TRIGGERS[type] is not ITrigger<T> trigger) return default;
        return trigger.Match(receiver);
    }

    /// <summary>
    /// 触发成功指定的动作
    /// </summary>
    /// <param name="action"></param>
    public static void TriggerSuccessAction<T>(this MessageReceiverBase receiver, Action<T> action)
    {
        var result = GetTriggerResult<T>(receiver);

        if (result == null) return;

        action(result);
    }
}
