using Mirai.Net.Data.Messages;
using Thabe.Bot.Core.Logger;
using Thabe.Bot.Core.Plugin;
using Thabe.Bot.Core.Plugin.Context;
using Thabe.Bot.Core.Plugin.Receiver.Interceptor;
using Thabe.Bot.Util;

namespace Thabe.Bot.Core.Preload.Concrete;



file class BotCallPlugin : IPreload
{
    public string Describe => "Bot插件呼叫";

    public void Init(ThabeBot bot, IPreload.Logger logger) 
        => bot.Core.MessageReceived.Subscribe(CallPlugin);

    static void CallPlugin(MessageReceiverBase receiver)
    {
        if (receiver.GetContext() is ContextHandel context)
        {
            //执行指定的上下
            context.Continue(receiver);
            return;
        }
        
        //按照接收器预先设置的优先级排序
        var receivers = PluginManager.ReceiverHandels.ToList();
        receivers.Sort((x, y) => y.MetaInfo.Level.CompareTo(x.MetaInfo.Level));

        //按照先后顺序调用
        receivers.Foreach(x =>
        {
            var interceptor = receiver.GetInterceptor();

            //如果有拦截器 就直接退出
            if (interceptor?.IsContinue() == false) return;
            interceptor?.Release();

            x.Receive(receiver);
        });
    }
}

file class PluginReceiverLoad : IPreload
{
    public string Describe => "加载插件";

    public void Init(ThabeBot bot, IPreload.Logger logger)
    {
        PluginManager.ReloadAllReceiver();

        foreach(var i in PluginManager.PluginHandels)
        {
            $"已加载插件: {i.PluginMeta.Name}".LogImportant();
        }

        int count = PluginManager.ReceiverHandels.Count();
        logger.Write($"共 {count} 个消息接收器");
    }
}

file class BotShowReceiverMessage : IPreload
{
    public string Describe => "Bot接收消息显示";

    public void Init(ThabeBot bot, IPreload.Logger logger) 
        => bot.Core.MessageReceived.Subscribe(x => x.LogBotMessage());
}
