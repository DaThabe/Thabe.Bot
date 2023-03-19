namespace Thabe.Bot.Core.Plugin.Script.CodeLoader;


/// <summary>
/// 脚本源码加载器
/// </summary>
public interface IScriptCodeLoader
{
    /// <summary>
    /// 获取源代码
    /// </summary>
    string GetSourceCode();
}
