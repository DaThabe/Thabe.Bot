using Mirai.Net.Data.Messages;
using Thabe.Bot.Core.Logger;
using Thabe.Bot.Core.Plugin;
using Thabe.Bot.Core.Plugin.Receiver.Dispatch;

namespace Thabe.Bot.Core.Preload.Concrete;



file class BotCallPlugin : IPreload
{
    public string Describe => "Bot插件呼叫";

    public void Init(ThabeBot bot, IPreload.Logger logger) 
        => bot.Core.MessageReceived.Subscribe(CallPlugin);

    static void CallPlugin(MessageReceiverBase receiver)
        => receiver.AddToTaskList();
}

file class PluginReceiverLoad : IPreload
{
    public string Describe => "加载插件";

    public void Init(ThabeBot bot, IPreload.Logger logger)
    {
        foreach(var i in PluginManager.Plugins)
        {
            $"已加载插件: {i.PluginMeta.Name}".LogImportant();
        }

        int count = PluginManager.Receivers.Count();
        logger.Write($"共 {count} 个消息接收器");
    }
}

file class BotShowReceiverMessage : IPreload
{
    public string Describe => "Bot接收消息显示";

    public void Init(ThabeBot bot, IPreload.Logger logger) 
        => bot.Core.MessageReceived.Subscribe(x => x.LogBotMessage());
}
