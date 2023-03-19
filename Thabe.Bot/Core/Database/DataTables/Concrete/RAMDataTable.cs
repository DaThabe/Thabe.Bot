using Newtonsoft.Json;
using Thabe.Bot.Util;

namespace Thabe.Bot.Core.Database.DataTables.Concrete;


/// <summary>
/// 依赖内存的数据库, 特点是数据仅仅在程序运行时存在.
/// </summary>
/// <typeparam name="T"></typeparam>
public class RAMDataTable<T> : IDataTable<T> where T : class
{
    /// <summary>
    /// 根据List作为数据保存容器
    /// </summary>
    [JsonProperty("values")]
    private readonly List<T> _values = new();

    /// <summary>
    /// 数据集合
    /// </summary>
    public IEnumerable<T> Values => _values;


    /// <summary>
    /// 重置数据
    /// </summary>
    /// <param name="values"></param>
    public void Reset(IEnumerable<T> values)
    {
        lock(_values)
        {
            _values.Clear();
            _values.AddRange(values);
        }
    }


    public void Insert(T value)
        => _values.Add(value);

    public int Delete(Func<T, bool> predicate)
        => _values.RemoveAll(x => predicate(x));

    public IEnumerable<T> Select(Func<T, bool> predicate)
        => _values.FindAll(x => predicate(x));

    
    public int Update(Func<T, bool> predicate, Action<T> newValue)
    {
        var lst = Select(predicate).ToArray();
        lst.Foreach(newValue);

        return lst.Length;
    }

    public int Replace(Func<T, bool> predicate, Func<T, T> getNewValue)
    {
        var lst = Select(predicate).ToArray();
        for (int i = 0; i < lst.Length; i++)
        {
            lst[i] = getNewValue(lst[i]);
        }

        return lst.Length;
    }
}
