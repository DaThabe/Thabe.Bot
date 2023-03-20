# Thabe.Bot preview 0.0.1
[![FOSSA Status](https://app.fossa.com/api/projects/git%2Bgithub.com%2FDaThabe%2FThabe.Bot.svg?type=shield)](https://app.fossa.com/projects/git%2Bgithub.com%2FDaThabe%2FThabe.Bot?ref=badge_shield)


Thabe.Bot 是基于 [Mirai.Net](https://github.com/SinoAHpx/Mirai.Net) 的QQ机器人

* `Thabe.Bot` 是 `Thabe.Bot` 的核心
* `Thabe.Bot.Cloud` 是 `Thabe.Bot`的云数据项目
* `Thabe.Bot.Console` 是 `Thabe.Bot`的控制台

## 快速上手

### 启动
直接启动 在控制台补充连接信息
``` C#
using Thabe.Bot;

await ThabeBot.ConnectAsync();
ThabeBot.Wait();
```
或者在代码中输入连接信息
``` C#
using Thabe.Bot;
using Thabe.Bot.Core.Bot;

await ThabeBot.ConnectAsync(new ConnectInfo()
{
    QQ = "2217568525",
    Addres = "localhost:xxxx",
    VerifyKey = "ThabeBot"
});

ThabeBot.Wait();
```

### 预加载
实现该接口的类会在 `ThabeConsole.ConnectAsync()` 之前调用
```C#
public interface IThabePrestrain
{
    string Describe { get; }

    void Init(ThabeBot bot, Logger logger);
}
```
或通过 `PreloadManager` 类添加一个预加载动作  但是该方法要在 `ThabeBot.ConnectAsync()` 之前使用
``` C#
PreloadManager.AddPreloadContent("预加载描述", (bot, logger) =>
{
    logger.WriteLine($"{bot.Core.QQ} 已连接成功!");
});
```

### 插件
下面代码是一个复读插件的展示
``` C#
//Plugin特性表示这是一个插件类
[Plugin("thabe.bot.plugin.repeat", "复读")]
public class RepeatPlugin
{
    //Receiver特性表示这是一个消息接受器 每个QQ消息都会传递到这里
    [Receiver(Describe = "接收指令后等待下一句回复，并发送")]
    public static async void Main(MessageReceiverBase receiver)
    {
        //判断消息内容是不是 /复读
        if (receiver.MessageChain is not [_, PlainMessage plain]) return;
        if (plain.Text.Trim() != "/复读") return;

        //回复消息 '我会复读下一句话'
        await receiver.ReplyAsync("我会复读下一句话", Replys.Quote | Replys.At | Replys.Recall, 1000);

        //设置下一个消息要进行的动作
        receiver.AddContext(async x =>
        {
            //获取消息并移除第一个SourceMessage
            var msg_chain = new MessageChain(x.MessageChain.ToArray()[1..]);

            //回复内容
            await receiver.ReplyAsync(msg_chain);
        });
    }
}
```

### 数据持久化
Thabe.Bot内部提供了一些数据持久化的帮助类, 它们的用法是这样的
``` C#
[Plugin("thabe.bot.plugin.log", "消息日志")]
public class LogPlugin
{
    private static readonly LocalDataTable<string> LOG_CONTENT
        = PluginManager.GetPluginHandel<Plugin>()
        ?.ResisterDataTable<string>(nameof(LOG_CONTENT))
        ?? throw new ArgumentNullException(nameof(LOG_CONTENT), "数据表初始化失败");

    [Receiver(Describe = "记录下每一条消息并保存在本地")]
    public static async void Main(MessageReceiverBase receiver)
    {
        //把消息插入数据表
        LOG_CONTENT.Insert(msg_chain.GetPlainMessage());
        
        //推送数据 (保存
        LOG_CONTENT.Push();
    }
}
```

## 关于插件
* 一个插件的范围是一个类且标注了 `PluginAttribute` 特性
* 一个插件可以拥有多个消息接收器
* 插件在注册之会保存插件的句柄和其中包含的所有接收器句柄

### PluginAttribute 特性标注一个 插件
* 需要写一个包名 和 插件名称
* 标注这个特性后 Bot在连接成功后 会加载注册
``` C#
[Plugin("package.plugin.name", "测试插件")]
class TestPlugin {  }
```

### ReceiverAttribute 特性标注一个 消息接收器
* 一个静态的方法且只有一个参数
* `MessageReceiverBase` 是 `Mirai.Net` 的消息接收器, 是所有消息接收器的基类, 所以可以处理所有消息内容
* 这里写入`MessageReceiverBase`的子类就会限定为子类的消息类型
* 比如 `FriendMessageReceiver` 是好友消息接收器 写入这个之后仅处理好友消息
* 比如 `GroupMessageReceiver` 是群组消息接收器 写入这个之后仅处理群消息
``` C#
[Receiver]
static void Method(MessageReceiverBase receiver);
```

### 插件和消息接收器的句柄?
* 通常标注了 `PluginAttribute` 特性的类都会自动注册和保存句柄
* 如果 `<T>` 已经标注 `PluginAttribute` 可通过下面两种方法获取到
``` C#
using Thabe.Bot.Core.Plugin;

var handel = PluginManager.GetPluginHandel<T>();
handel = PluginManager.GetPluginHandel(Type type);
```
* 有了 `PluginHandel` 之后可用下面的方法获取到 `消息接收器` 句柄
``` C#
var receiver_handels = handel.GetReceiverHandels();
```
### 有了句柄可以干什么?
* 目前主要是用来获取 `元信息` 和 `扩展方法`

###  插件句柄的公开属性和方法
``` C#
public class PluginHandel
{
    //插件类 类型
    public Type PluginType { get; init; }

    //插件元数据
    public PluginAttribute PluginMeta { get; init; }
}
```

###  接收器句柄的公开属性和方法
``` C#
public class ReceiverHandel
{
    //所属插件
    public Type PluginType { get; init; }

    ///插件元信息
    public PluginAttribute PluginMetaInfo { get; init; }

    ///接收器特性
    public ReceiverAttribute MetaInfo { get; init; }

    ///接收方法信息
    public MethodInfo MethodInfo { get; init; }

    ///消息接收器类型
    public Type MessageReceiverType { get; init; }

    //接收消息并处理
    public void Receive(MessageReceiverBase receiver);
}
```

## 脚本插件
* 可通过 `ScriptUtil` 加载脚本插件
``` C#
public static class ScriptUtil
{
    /// <summary>
    /// 加载一个脚本插件
    /// </summary>
    /// <param name="codeLoader">代码加载器</param>
    /// <param name="name">插件名称</param>
    public static ScriptHandel Load(IScriptCodeLoader codeLoader, string? name = null);

    /// <summary>
    /// 从网络文件加载
    /// </summary>
    /// <param name="url">网址</param>
    public static ScriptHandel LoadFromWebFile(string url, string? name = null);

    /// <summary>
    /// 从本地文件加载
    /// </summary>
    /// <param name="filePath">文件路径</param>
    /// <param name="name">脚本名称</param>
    public static ScriptHandel LoadFromLocalFile(string filePath, string? name = null);
    
    /// <summary>
    /// 从源代码加载
    /// </summary>
    /// <param name="code">源码</param>
    /// <param name="name">脚本名称</param>
    public static ScriptHandel LoadFromSourceCode(string code, string? name = null);
}
```
### ScriptHandel
``` C#
public class ScriptHandel
{
    //加载成功事件
    public event Action<AssemblyLoadContext, Assembly>? Loaded;

    //卸载中事件
    public event Action<AssemblyLoadContext, Assembly>? Unloading;

    ///包含的所有插件句柄
    public IEnumerable<PluginHandel> PluginHandels;


    //重载插件
    public void Reload();

    //卸载插件
    public void Unload();
}
```

## License
[![FOSSA Status](https://app.fossa.com/api/projects/git%2Bgithub.com%2FDaThabe%2FThabe.Bot.svg?type=large)](https://app.fossa.com/projects/git%2Bgithub.com%2FDaThabe%2FThabe.Bot?ref=badge_large)