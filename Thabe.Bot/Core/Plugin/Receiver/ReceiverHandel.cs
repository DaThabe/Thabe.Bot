using Mirai.Net.Data.Messages;
using System.Reflection;

namespace Thabe.Bot.Core.Plugin.Receiver;


/// <summary>
/// 插件接收器句柄
/// </summary>
public class ReceiverHandel
{
    /// <summary>
    /// 所属插件
    /// </summary>
    public Type PluginType { get; init; }

    /// <summary>
    /// 插件元信息
    /// </summary>
    public PluginAttribute PluginMetaInfo { get; init; }

    /// <summary>
    /// 接收器特性
    /// </summary>
    public ReceiverAttribute MetaInfo { get; init; }

    /// <summary>
    /// 接收方法信息
    /// </summary>
    public MethodInfo MethodInfo { get; init; }

    /// <summary>
    /// 消息接收器类型
    /// </summary>
    public Type MessageReceiverType { get; init; }


    /// <summary>
    /// 初始化插件接收器
    /// </summary>
    /// <param name="receiver">方法信息</param>
    /// <exception cref="ArgumentException"></exception>
    public ReceiverHandel(Type plugin, MethodInfo receiver)
    {
        PluginMetaInfo = plugin.GetCustomAttribute<PluginAttribute>()
            ?? throw new ArgumentException("该类型不是插件类型!", nameof(plugin));

        MetaInfo = receiver.GetCustomAttribute<ReceiverAttribute>()
            ?? throw new ArgumentException("该方法缺少接收器特性!", nameof(receiver));

        var @params = receiver.GetParameters();
        if (@params.Length != 1 || !typeof(MessageReceiverBase).IsAssignableFrom(@params[0].ParameterType))
        {
            throw new ArgumentException("该方法缺少MessageReceiverBase或子类类型参数!", nameof(receiver));
        }

        MethodInfo = receiver;
        PluginType = plugin;
        MessageReceiverType = @params[0].ParameterType;
    }

    /// <summary>
    /// 接收消息并处理
    /// </summary>
    /// <param name="receiver"></param>
    public void Receive(MessageReceiverBase receiver)
    {
        if (!MessageReceiverType.IsAssignableFrom(receiver.GetType()))
        {
            return;
        }

        MethodInfo.Invoke(null, new object[] { receiver });
    }
}
