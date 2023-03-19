namespace Thabe.Bot.Core.Plugin.Script.CodeLoader.Concrete;


/// <summary>
/// 本地脚本源码加载器
/// </summary>
public class LocalFileCodeLoader : IScriptCodeLoader
{
    /// <summary>
    /// 脚本文件路径
    /// </summary>
    private readonly string _filePath;

    public LocalFileCodeLoader(string filePath) => _filePath = filePath;

    public string GetSourceCode() => File.ReadAllText(_filePath);
}
