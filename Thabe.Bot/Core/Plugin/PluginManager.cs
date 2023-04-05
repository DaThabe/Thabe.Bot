using System.Reflection;
using System.Runtime.Loader;
using Thabe.Bot.Core.Logger;
using Thabe.Bot.Core.Plugin.Receiver;
using Thabe.Bot.Util;

namespace Thabe.Bot.Core.Plugin;


/// <summary>
/// 插件句柄管理类
/// </summary>
public static class PluginManager
{
    /// <summary>
    /// 重载锁
    /// </summary>
    private static readonly object _RELOAD_LOCK = new();

    /// <summary>
    /// 插件接收器集合
    /// </summary>
    private static readonly Dictionary<PluginHandel, List<ReceiverHandel>> _RECEIVER_HANDELS = new();

    /// <summary>
    /// 插件句柄
    /// </summary>
    private static readonly Dictionary<Type, PluginHandel> _PLUGIN_HANDELS = new();

    /// <summary>
    /// 接收器句柄集合
    /// </summary>
    public static IEnumerable<ReceiverHandel> Receivers
        => from i in _RECEIVER_HANDELS from receiver in i.Value select receiver;

    /// <summary>
    /// 插件句柄集合
    /// </summary>
    public static IEnumerable<PluginHandel> Plugins
        => from i in _RECEIVER_HANDELS select i.Key;


    /// <summary>
    /// 从指定程序集中获取接收器
    /// </summary>
    /// <param name="loader"></param>
    /// <returns></returns>
    public static int LoadAllReceiver(Assembly assembly)
    {
        lock (_RELOAD_LOCK)
        {
            var types = from type in assembly.GetTypes()
                        where type.GetCustomAttribute<PluginAttribute>() != null
                        select type;

            int count = 0;
            types.TryForeach(type =>
            {
                if (RegisterPluginHandel(type) != null) count++;
            });

            return count;
        }
    }

    /// <summary>
    /// 从指定类型所在的程序集加载所有类型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static int LoadAllReceiver<T>()
    {
        lock (_RELOAD_LOCK)
        {
            var assembly = Assembly.GetAssembly(typeof(T));

            if (assembly == null) return 0;

            return LoadAllReceiver(assembly);
        }
    }

    /// <summary>
    /// 注册插件句柄
    /// </summary>
    /// <typeparam name="T">插件类型</typeparam>
    public static PluginHandel? RegisterPluginHandel<T>() => RegisterPluginHandel(typeof(T));

    /// <summary>
    /// 注册插件句柄
    /// </summary>
    /// <param name="type">插件类型</param>
    public static PluginHandel? RegisterPluginHandel(Type type)
    {
        if (_PLUGIN_HANDELS.TryGetValue(type, out PluginHandel? value)) return value;

        var plugin_att = type.GetCustomAttribute<PluginAttribute>();
        if (plugin_att == null) return null;

        //初始化句柄
        _PLUGIN_HANDELS[type] = new PluginHandel
        {
            PluginType = type,
            PluginMeta = plugin_att
        };
        var handel = _PLUGIN_HANDELS[type];

        //获取接收器
        List<ReceiverHandel> receiver_handels = new();

        //添加句柄
        type.GetTypeInfo().DeclaredMethods.TryForeach(method_info =>
        {
            receiver_handels.Add(new ReceiverHandel(type, method_info));
        });



        //接收器关联插件句柄
        if (!_RECEIVER_HANDELS.TryGetValue(handel, out List<ReceiverHandel>? plugin))
        {
            _RECEIVER_HANDELS[handel] = new();
        }

        //插件关联接收器
        _RECEIVER_HANDELS[handel].AddRange(receiver_handels);

        //返回句柄
        return handel;
    }


    /// <summary>
    /// 获取插件句柄
    /// </summary>
    public static PluginHandel? GetPluginHandel<T>()
        => GetPluginHandel(typeof(T));

    /// <summary>
    /// 获取插件句柄
    /// </summary>
    public static PluginHandel? GetPluginHandel(Type type)
    {
        _PLUGIN_HANDELS.TryGetValue(type, out PluginHandel? value);
        return value;
    }


    /// <summary>
    /// 获取指定插件中所有接收器
    /// </summary>
    public static IEnumerable<ReceiverHandel> GetReceiverHandels(this PluginHandel pluginHandel)
    {
        if (!_RECEIVER_HANDELS.TryGetValue(pluginHandel, out List<ReceiverHandel>? receivers))
        {
            return Array.Empty<ReceiverHandel>();
        }

        return receivers;
    }

    /// <summary>
    /// 卸载插件
    /// </summary>
    /// <param name="pluginHandel"></param>
    /// <returns></returns>
    public static bool Unload(this PluginHandel pluginHandel)
    {
        if (!_RECEIVER_HANDELS.TryGetValue(pluginHandel, out var receivers)) return false;

        //释放插件
        if (!_RECEIVER_HANDELS.Remove(pluginHandel, out List<ReceiverHandel>? lst)) return false;

        //释放句柄
        var kv = _PLUGIN_HANDELS.FirstOrDefault(x => x.Value == pluginHandel);
        _PLUGIN_HANDELS.Remove(kv.Key);

        $"{pluginHandel.PluginMeta.Name}已卸载".LogTips();

        return true;
    }
}