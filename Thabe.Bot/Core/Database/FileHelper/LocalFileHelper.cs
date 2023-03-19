using Thabe.Bot.Core.Logger;

namespace Thabe.Bot.Core.Database.PathUtil;


/// <summary>
/// 本地路径合成器管理类
/// </summary>
public class LocalFileHelper
{
    /// <summary>
    /// 文件完整路径
    /// </summary>
    public string FullPath { get; init; }

    /// <summary>
    /// 文件路径
    /// </summary>
    public string? FilePath { get; init; }

    /// <summary>
    /// 文件名称
    /// </summary>
    public string FileName { get; init; }

    /// <summary>
    /// 数据文件类型
    /// </summary>
    public string FileType { get; init; }



    /// <summary>
    /// 文件名称, 包含文件扩展名
    /// </summary>
    public string GetFileName => $"{FileName}{FileType}";

    /// <summary>
    /// 数据文件夹
    /// </summary>
    public string FileFolder => Path.GetDirectoryName(FullPath)
        ?? throw new ArgumentNullException(nameof(FileFolder), "路径有问题!");

    /// <summary>
    /// 获取数据根路径
    /// </summary>
    public static string RootPath => ThabeBot.Config.LOCAL_DATABASE_PATH;


    /// <summary>
    /// 初始化本地路径合成器    数据根目录/{path}/{name.type}
    /// </summary>
    /// <param name="name">数据名称</param>
    /// <param name="path">数据路径</param>
    /// <param name="fileType">数据文件类型</param>
    public LocalFileHelper(string name, string? path = null, string fileType = ".json")
    {
        if(string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentNullException(nameof(name));
        }

        path = path?.Replace("/", "\\") ?? "";

        (FileName, FilePath, FileType) = (name.ToLower(), path.ToLower(), fileType.ToLower());
        FullPath = Path.Combine(RootPath, FilePath, GetFileName);
    }


    /// <summary>
    /// 设置文件储存路径. 没有就创建
    /// </summary>
    /// <returns>路径是否存在</returns>
    public bool SetFileFolder()
    {
        if(!Directory.Exists(FileFolder))
        {
            Directory.CreateDirectory(FileFolder);
        }

        return Directory.Exists(FileFolder);
    }


    /// <summary>
    /// 保存文本文件
    /// </summary>
    /// <param name="content">文件内容</param>
    public async void SaveText(string content)
    {
        if (!SetFileFolder())
        {
            throw new DirectoryNotFoundException("路径创建失败");
        }

        try
        {
            var backup = $"{FullPath}.backup";

            await File.WriteAllTextAsync(backup, content);
            File.Delete(FullPath);
            File.Move(backup, FullPath);
        }
        catch
        {
            $"数据文件[ {FullPath} ] 保存失败!".LogError();
        }
    }

    /// <summary>
    /// 加载文本文件
    /// </summary>
    public string LoadText()
    {
        try
        {
            return File.ReadAllText(FullPath);
        }
        catch(Exception ex)
        {
            ex.Message.LogError();
            return "";
        }
    }
}
