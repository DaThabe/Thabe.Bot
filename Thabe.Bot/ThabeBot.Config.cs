using Thabe.Bot.Core.Database.Config.Concrete;

namespace Thabe.Bot;


/// <summary>
/// 一些静态配置数据
/// </summary>
/// 
public partial class ThabeBot
{
    /// <summary>
    /// Bot配置
    /// </summary>
    public static class Config
    {
        #region --不会改变的数据--

        /// <summary>
        /// 机器人英文名称
        /// </summary>
        public const string BOT_LOGO = "Thabe Bot";

        /// <summary>
        /// 配置文件名称
        /// </summary>
        private const string CONFIG_FILE_NAME = "ThabeBotConfig";



        /// <summary>
        /// 程序运行目录
        /// </summary>
        public static string APP_FOLDER => AppDomain.CurrentDomain.BaseDirectory;

        /// <summary>
        /// 本地数据库文件夹名称
        /// </summary>
        public const string LOCAL_DATABASE_FOLDER_NAME = "data";

        /// <summary>
        /// 获取本地数据文件夹
        /// </summary>
        public static string LOCAL_DATABASE_PATH => Path.Combine(APP_FOLDER, LOCAL_DATABASE_FOLDER_NAME);

        #endregion

        /// <summary>
        /// 本地配置
        /// </summary>
        private static readonly LocalJsonConfig _LOCAL_CONFIG = new(CONFIG_FILE_NAME);


        /// <summary>
        /// bot主人QQ
        /// </summary>
        public static string BOT_MASTER => GetOrDefault(nameof(BOT_MASTER).ToLower(), "2217568525");

        /// <summary>
        /// Bot版本
        /// </summary>
        public static string BOT_VERSION => GetOrDefault(nameof(BOT_VERSION).ToLower(), "0.0.1");

        /// <summary>
        /// 上次启动时间
        /// </summary>
        public static DateTime LAST_LAUNCH_TIME
        {
            get => GetOrDefault(nameof(LAST_LAUNCH_TIME).ToLower(), DateTime.Now);

            //TODO: 配置数据无法保存到本地
            set => Set(nameof(LAST_LAUNCH_TIME), value);
        }


        /// <summary>
        /// 获取一个键值, 如果不存在则使用默认值, 日志会提示是否使用了默认值
        /// </summary>
        public static T GetOrDefault<T>(string key, T defaultValue) where T : notnull
            => _LOCAL_CONFIG.GetOrDefault(key, defaultValue);

        /// <summary>
        /// 获取一个键值, 如果不存在则返回null
        /// </summary>
        public static T? GetOrNull<T>(string key)
           => _LOCAL_CONFIG.GetOrNull<T>(key);


        /// <summary>
        /// 设置一个配置
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        public static void Set<T>(string key, T data) where T : notnull
            => _LOCAL_CONFIG.Set(key, data);


        /// <summary>
        /// 保存当前配置
        /// </summary>
        public static void Save() => _LOCAL_CONFIG.Push();
    }
}