using System.Collections.Concurrent;
using Thabe.Bot.Core.Database.DataTables.Concrete;

namespace Thabe.Bot.Core.Database.DataTables;


/// <summary>
/// 数据库管理类
/// </summary>
public static class DatabaseManager
{
    public static RAMDataTable<T> GetRAMTable<T>(string name, string? group = null) where T : class
        => DataManager<RAMDataTable<T>>.GetTable(name, group, () => new RAMDataTable<T>());

    /// <summary>
    /// 获取本地数据表
    /// </summary>
    /// <exception cref="KeyNotFoundException">数据表不存在</exception>
    public static LocalDataTable<T> GetLocalTable<T>(string name, string? group = null) where T : class
        => DataManager<LocalDataTable<T>>.GetTable(name, group, () => new LocalDataTable<T>(name, group));


    /// <summary>
    /// 如果名称不符合规定 会直接触发异常   只要不是空白和null就不会触发异常
    /// </summary>
    /// <param name="name"></param>
    /// <exception cref="ArgumentException"></exception>
    internal static string NameException(this string name)
    {
        if (!string.IsNullOrWhiteSpace(name))
        {
            return name;
        }

        throw new ArgumentException(name);
    }

    /// <summary>
    /// 获取数据表的id
    /// </summary>
    internal static string GetTableId(string name, string? group = null)
    {
        name.Trim().NameException();
        group?.Trim().NameException();

        return $"{group ?? ""}-{name}";
    }



    /// <summary>
    /// 数据实例类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    private static class DataManager<T> where T : class
    {
        /// <summary>
        /// 根据字典来保存数据表操作实例
        /// </summary>
        private static readonly ConcurrentDictionary<string, T> _INSTANCE
            = new(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// 获取数据表
        /// </summary>
        /// <exception cref="KeyNotFoundException">数据表不存在</exception>
        public static T GetTable(string name, string? group, Func<T> getInstance)
        {
            var id = GetTableId(name, group);
            if (_INSTANCE.TryGetValue(id, out T? value) && value != null)
            {
                return value;
            }

            lock (_INSTANCE.GetOrAdd(id, getInstance()))
            {
                return _INSTANCE[id];
            }
        }
    }
}
