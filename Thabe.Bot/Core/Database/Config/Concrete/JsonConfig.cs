using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Thabe.Bot.Core.Logger;
using System.Collections.Concurrent;

namespace Thabe.Bot.Core.Database.Config.Concrete;


/// <summary>
/// Json配置
/// </summary>
public class JsonConfig : IConfig, IPersistentData
{

    private readonly Func<string> _getJsonString;
    private readonly Action<string> _saveJsonString;

    private ConcurrentDictionary<string, JToken> _value = new();


    /// <summary>
    /// 初始化Json配置
    /// </summary>
    /// <param name="getJsonString">如何获取Json字符串</param>
    /// <param name="saveJsonString">如何保存Json字符串</param>
    public JsonConfig(Func<string> getJsonString, Action<string> saveJsonString)
    {
        _getJsonString = getJsonString;
        _saveJsonString = saveJsonString;

        Pull();
    }



    public T GetOrDefault<T>(string key, T defaultValue) where T : notnull
    {
        var j_token = _value.GetValueOrDefault(key);

        if (j_token == null)
        {
            var log_info = $"配置数据 [{key}] 不存在, 已经使用默认值[{defaultValue}]";
            log_info.LogWarning();


            Set(key, defaultValue);
            Push();

            return defaultValue;
        }

        try
        {
            return j_token.ToObject<T>() ?? throw new ArgumentNullException();
        }
        catch
        {
            var log_info = $"配置数据 [{key}] 转换失败, 已经使用默认值, 但没有保存在本地, 请查询是否用同key配置或配置数据有误!";
            log_info.LogWarning();

            return defaultValue;
        }
    }

    public T? GetOrNull<T>(string key)
    {
        var j_token = _value.GetValueOrDefault(key);

        if (j_token != null && j_token.ToObject<T>() is T t) return t;

        return default;
    }

    public void Set<T>(string key, T data) where T : notnull
    {
        //TODO: 需要测试这个写法是否可以执行
        _value.AddOrUpdate(key, x => JToken.FromObject(data), (x, y) => JToken.FromObject(data));
    }


    public void Pull()
    {
        try
        {
            var instance = JsonConvert.DeserializeObject<ConcurrentDictionary<string, JToken>>(_getJsonString());

            if (instance != null) _value = instance;
        }
        catch
        {
            "配置文件加载失败, 将使用默认配置!".LogWarning();
        }
    }

    public void Push()
    {
        try
        {
            var json = JsonConvert.SerializeObject(_value);
            _saveJsonString(json);
        }
        catch
        {
            "配置文件保存失败".LogError();
        }
    }
}
