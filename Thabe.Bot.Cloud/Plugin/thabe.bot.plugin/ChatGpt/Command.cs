using PowerArgs;
using PowerArgs.Samples;

namespace Thabe.Bot.Cloud.Plugin.thabe.bot.plugin.ChatGpt;


/*

用户指令:

/nyachat new [name]
    --重新开一个聊天情景

/nyachat del [name]
    --删除指定的聊天情景

/nyachat change [name]
    --切换到指定聊天情景

/nyachat list
    --查看所有聊天情景

/nyachat defaut
    --切换到公共聊天情景
 

管理员指令:

/nyachat new [name] [@Target]
    --重新开一个聊天情景

/nyachat del [name] [@Target]
    --删除指定的聊天情景

/nyachat change [name] [@Target]
    --切换到指定聊天情景

/nyachat list [@Target]
    --查看所有聊天情景

/nyachat defaut [@Target]
    --切换到公共聊天情景


/nyachat -global new [name]

 */



//[ArgExceptionBehavior(ArgExceptionPolicy.StandardExceptionHandling)]
internal class Command
{
    [HelpHook, ArgShortcut("-?"), ArgDescription("显示这条指令的帮助")]
    public bool Help { get; set; }


    [ArgActionMethod, ArgDescription("重新开一个聊天情景")]
    public void New(
        [ArgRequired][ArgDescription("情景名称")][ArgPosition(1)] string name)
    {
        Console.WriteLine(name);
    }
}