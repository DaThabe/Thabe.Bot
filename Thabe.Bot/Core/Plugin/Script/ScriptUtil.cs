using Thabe.Bot.Core.Plugin.Script.CodeLoader;
using Thabe.Bot.Core.Plugin.Script.CodeLoader.Concrete;

namespace Thabe.Bot.Core.Plugin.Script;


/// <summary>
/// 脚本插件帮助类
/// </summary>
public static class ScriptUtil
{
    /// <summary>
    /// 加载一个脚本插件
    /// </summary>
    /// <param name="codeLoader">代码加载器</param>
    /// <param name="name">插件名称</param>
    public static ScriptHandel Load(IScriptCodeLoader codeLoader, string? name = null)
    {
        ScriptHandel scriptPlugin = new(name, codeLoader);

        scriptPlugin.Reload();

        return scriptPlugin;
    }

    /// <summary>
    /// 从网络文件加载
    /// </summary>
    /// <param name="url">网址</param>
    public static ScriptHandel LoadFromWebFile(string url, string? name = null) => Load(new WebFileCodeLoader(url), name);

    /// <summary>
    /// 从本地文件加载
    /// </summary>
    /// <param name="filePath">文件路径</param>
    /// <param name="name">脚本名称</param>
    public static ScriptHandel LoadFromLocalFile(string filePath, string? name = null) => Load(new LocalFileCodeLoader(filePath), name);
    
    /// <summary>
    /// 从源代码加载
    /// </summary>
    /// <param name="code">源码</param>
    /// <param name="name">脚本名称</param>
    public static ScriptHandel LoadFromSourceCode(string code, string? name = null) => Load(new CustomCodeLoader(() => code), name);
}