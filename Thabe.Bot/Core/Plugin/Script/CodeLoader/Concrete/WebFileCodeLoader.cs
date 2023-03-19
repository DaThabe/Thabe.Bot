using Flurl.Http;

namespace Thabe.Bot.Core.Plugin.Script.CodeLoader.Concrete;


/// <summary>
/// 网络脚本源码加载器
/// </summary>
public class WebFileCodeLoader : IScriptCodeLoader
{
    /// <summary>
    /// 脚本文件路径
    /// </summary>
    private readonly string _fileUrl;

    public WebFileCodeLoader(string fileUrl) => _fileUrl = fileUrl;

    public string GetSourceCode() => _fileUrl.GetStringAsync().GetAwaiter().GetResult();
}