namespace Thabe.Bot.Core.Database.DataTables;


/// <summary>
/// 数据表接口
/// </summary>
public interface IDataTable<T> where T : class
{
    /// <summary>
    /// 数据集合
    /// </summary>
    IEnumerable<T> Values { get; }


    /// <summary>
    /// 筛选数据
    /// </summary>
    /// <param name="predicate">筛选条件</param>
    IEnumerable<T> Select(Func<T, bool> predicate);

    /// <summary>
    /// 删除数据
    /// </summary>
    /// <param name="predicate">删除条件</param>
    /// <returns>删除成功的数据数量</returns>
    int Delete(Func<T, bool> predicate);

    /// <summary>
    /// 更新数据
    /// </summary>
    /// <param name="predicate">更新条件</param>
    /// <param name="updateAction">更新参数的方法</param>
    /// <returns>更新成功的数据数量</returns>
    int Update(Func<T, bool> predicate, Action<T> updateAction);

    /// <summary>
    /// 替换数据
    /// </summary>
    /// <param name="predicate">替换条件</param>
    /// <param name="getNewValue">获取新的需要替换的值</param>
    /// <returns></returns>
    int Replace(Func<T, bool> predicate, Func<T, T> getNewValue);

    /// <summary>
    /// 插入数据
    /// </summary>
    /// <param name="value">数据实例</param>
    void Insert(T value);
}