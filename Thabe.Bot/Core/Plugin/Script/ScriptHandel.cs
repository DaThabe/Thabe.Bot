using System.Reflection;
using System.Runtime.Loader;
using Thabe.Bot.Core.Logger;
using Thabe.Bot.Core.Plugin.Script.CodeLoader;
using Thabe.Bot.Util;

namespace Thabe.Bot.Core.Plugin.Script;


/// <summary>
/// 脚本插件句柄
/// </summary>
public class ScriptHandel
{
    /// <summary>
    /// 获取源码
    /// </summary>
    private readonly IScriptCodeLoader _scriptCodeLoader;

    /// <summary>
    /// Dll 名称
    /// </summary>
    private readonly string? _dllName;

    /// <summary>
    /// 重载锁
    /// </summary>
    private readonly object _relaodLock = new();

    /// <summary>
    /// 插件句柄集合
    /// </summary>
    private readonly List<PluginHandel> _pluginHandels = new();

    /// <summary>
    /// 插件程序集加载器
    /// </summary>
    private AssemblyLoadContext? _pluginLoader;

    /// <summary>
    /// 插件程序集
    /// </summary>
    private Assembly? assembly;


    /// <summary>
    /// 加载成功
    /// </summary>
    public event Action<AssemblyLoadContext, Assembly>? Loaded;

    /// <summary>
    /// 卸载中
    /// </summary>
    public event Action<AssemblyLoadContext, Assembly>? Unloading;


    

    /// <summary>
    /// 包含的插件句柄
    /// </summary>
    public IEnumerable<PluginHandel> PluginHandels => _pluginHandels;


    /// <summary>
    /// 创建一个脚本插件
    /// </summary>
    /// <param name="name"></param>
    /// <param name="getSourceCode"></param>
    public ScriptHandel(string? name, IScriptCodeLoader codeLoader)
    {
        _dllName = name;
        _scriptCodeLoader = codeLoader;


        Loaded += (AssemblyLoadContext loader, Assembly assembly) =>
        {
            //加载所有已经加载的插件
            assembly.GetTypes().Foreach(x =>
            {
                var handel = PluginManager.RegisterPluginHandel(x);

                if (handel == null) return;

                _pluginHandels.Add(handel);
            });
        };

        Unloading += (AssemblyLoadContext loader, Assembly assembly) =>
        {
            //卸载所有已经加载的插件
            assembly.GetTypes().Foreach(x =>
            {
                PluginManager.GetPluginHandel(x)?.Unload();
            });

            //卸载程序集
            loader.Unload();
        };
    }


    /// <summary>
    /// 重载插件 成功会激活<see cref="Loaded"/>事件
    /// </summary>
    public void Reload()
    {
        lock (_relaodLock)
        {
            try
            {
                if (_pluginLoader != null && assembly != null)  Unload();

                $"正在重新编译脚本文件{_dllName}".LogTips();

                var code = _scriptCodeLoader.GetSourceCode();
                var arg = CSahrpUtil.DynamicCompilation(_dllName, code);
                if (arg == default) throw new ArgumentNullException(nameof(arg), "未能成功编译获取程序集");

                (_pluginLoader, assembly) = arg;

                $"编译成功{_dllName}".LogTips();
                Loaded?.Invoke(_pluginLoader, assembly);
            }
            catch (Exception ex)
            {
                ex.Message.LogError();
            }
        }
    }

    /// <summary>
    /// 卸载插件, 并没有动作, 会触发<see cref="Unloading"/>事件
    /// </summary>
    public void Unload()
    {
        if (_pluginLoader != null && assembly != null)
        {
            Unloading?.Invoke(_pluginLoader, assembly);
        }
    }
}
