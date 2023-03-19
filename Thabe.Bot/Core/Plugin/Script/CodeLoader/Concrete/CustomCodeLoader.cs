namespace Thabe.Bot.Core.Plugin.Script.CodeLoader.Concrete;


/// <summary>
/// 自定义脚本源码加载器
/// </summary>
public class CustomCodeLoader : IScriptCodeLoader
{
    /// <summary>
    /// 获取源码的方法
    /// </summary>
    private readonly Func<string> _getSourceCode;

    public CustomCodeLoader(Func<string> getSourceCode) => _getSourceCode = getSourceCode;

    public string GetSourceCode() => _getSourceCode();
}
