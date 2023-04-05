using Flurl.Http;
using Thabe.Bot.Core.Logger;
using Thabe.Bot.Util;

namespace Thabe.Bot.Core.Database.FileHelper;


/// <summary>
/// 文件管理类
/// </summary>
public static class FileManager
{
    /// <summary>
    /// 下载网络文件
    /// </summary>
    /// <param name="url">网址</param>
    /// <param name="isCoverSave">是否覆盖获取</param>
    /// <returns></returns>
    public static async Task<string> CacheWebFile(this string url, bool isCoverSave = false)
    {
        var path = ThabeBot.Config.LOCAL_CACHE_PATH;
        var name = url.ToMD5();
        var file = Path.Combine(path, name);


        if(File.Exists(file) && isCoverSave == false)
        {
            return file;
        }

        try
        {
            return await url.DownloadFileAsync(path, name);
        }
        catch(Exception ex)
        {
            ex.Message.LogError();
            return "";
        }
    }

    /// <summary>
    /// 缓存网络文件并转为base64
    /// </summary>
    /// <param name="url"></param>
    /// <param name="isCoverSave"></param>
    /// <returns></returns>
    public static async Task<string> CacheWebFileToBase64(this string url, bool isCoverSave = false)
    {
        var file_path = await url.CacheWebFile(isCoverSave);
        using FileStream filestream = new(file_path, FileMode.Open);

        byte[] bt = new byte[filestream.Length];
        filestream.Read(bt, 0, bt.Length);

        return Convert.ToBase64String(bt);
    }
}
