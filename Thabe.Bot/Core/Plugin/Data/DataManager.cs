using Thabe.Bot.Core.Database.Config;
using Thabe.Bot.Core.Database.Config.Concrete;
using Thabe.Bot.Core.Database.DataTables;
using Thabe.Bot.Core.Database.DataTables.Concrete;

namespace Thabe.Bot.Core.Plugin.Data;


/// <summary>
/// 插件数据管理
/// </summary>
public static class DataManager
{
    /// <summary>
    /// 获取数据
    /// </summary>
    public static LocalDataTable<T> ResisterDataTable<T>(this PluginHandel pluginHandel, string name) where T : class
        => DatabaseManager.GetLocalTable<T>(name, pluginHandel.PluginMeta.PackName);


    /// <summary>
    /// 获取数据
    /// </summary>
    public static LocalJsonConfig ResisterConfig(this PluginHandel pluginHandel, string name)
        => ConfigManager.GetLocalConfig(name, pluginHandel.PluginMeta.PackName);
}
