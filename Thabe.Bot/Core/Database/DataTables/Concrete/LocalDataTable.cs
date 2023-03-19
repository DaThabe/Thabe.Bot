using Newtonsoft.Json;
using Thabe.Bot.Core.Database.PathUtil;
using Thabe.Bot.Core.Logger;

namespace Thabe.Bot.Core.Database.DataTables.Concrete;


/// <summary>
/// 本地数据表
/// </summary>
public class LocalDataTable<T> : RAMDataTable<T>, IPersistentData where T : class
{
    /// <summary>
    /// 路径合成器
    /// </summary>
    private readonly LocalFileHelper _fileHelper;


    /// <summary>
    /// 本地文件监视器, 如果有修改自动更新内存中的数据
    /// </summary>
    private readonly FileSystemWatcher _tableFileWatcher;

    /// <summary>
    /// 本地数据更新事件
    /// </summary>
    public event EventHandler? LocalFileDataUpdated;


    /// <summary>
    /// 创建本地数据表
    /// </summary>
    /// <param name="name">数据表路径</param>
    public LocalDataTable(string name, string? path = null)
    {
        name = name.Trim().NameException();
        if (path != null) path = path.Trim().NameException();

        _fileHelper = new(name, path);

        //拉取本地数据
        Pull();

        //绑定本地数据文件
        _tableFileWatcher = new(_fileHelper.FileFolder)
        {
            Filter = _fileHelper.FileName,
            NotifyFilter = NotifyFilters.LastWrite
        };

        _tableFileWatcher.Changed += (sender, e) => Pull();
        _tableFileWatcher.EnableRaisingEvents = true;
    }


    public void Pull()
    {
        try
        {
            if (!File.Exists(_fileHelper.FullPath))
            {
                Push();
                return;
            }

            var json = _fileHelper.LoadText();
            var new_value = JsonConvert.DeserializeObject<List<T>>(json);

            if (new_value == null)
            {
                throw new ArgumentNullException("读取数据失败, 可能是类型不匹配!");
            }

            //重置数据
            Reset(new_value);

            //触发本地文件已经更新事件
            LocalFileDataUpdated?.Invoke(this, EventArgs.Empty);
        }
        catch (Exception ex)
        {
            ex.Message.LogError();
        }
    }

    public void Push()
    {
        try
        {
            if (_tableFileWatcher != null)
            {
                _tableFileWatcher.EnableRaisingEvents = false;
            }

            var json = JsonConvert.SerializeObject(Values);
            _fileHelper.SaveText(json);
        }
        catch (Exception ex)
        {
            ex.Message.LogError();
        }

        if (_tableFileWatcher != null)
        {
            _tableFileWatcher.EnableRaisingEvents = true;
        }
    }
}
