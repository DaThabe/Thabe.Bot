using Mirai.Net.Data.Messages;
using Mirai.Net.Data.Messages.Concretes;
using Thabe.Bot.Core.Database.Config;
using Thabe.Bot.Core.Database.Config.Concrete;
using Thabe.Bot.Core.Database.DataTables.Concrete;
using Thabe.Bot.Core.Plugin;
using Thabe.Bot.Core.Plugin.Data;
using Thabe.Bot.Core.Plugin.Receiver;
using Thabe.Bot.Core.Plugin.Receiver.Dispatch.Context;
using Thabe.Bot.Util;

namespace Thabe.Bot.Cloud.Plugin.thabe.bot.plugin.SayPlugin;


[Plugin(Package.Name, "Say")]
public class Plugin
{
    private static readonly LocalDataTable<string> SAY_CONTENT = PluginManager.GetPluginHandel<Plugin>()
       ?.ResisterDataTable<string>(nameof(SAY_CONTENT)) ?? throw new ArgumentNullException(nameof(SAY_CONTENT), "数据表初始化失败");


    [Receiver(Describe = "接收指令后等待下一句回复，并发送")]
    public static async void Main(MessageReceiverBase receiver)
    {
        if (receiver.MessageChain is not [_, PlainMessage plain])
        {
            return;
        }

        if (plain.Text.Trim() != "/say") return;


        await receiver.ReplyAsync("请发送一句话", Replys.Quote | Replys.At | Replys.Recall, 1000);

        receiver.AddContext(async x =>
        {
            var msg_chain = new MessageChain(x.MessageChain.ToArray()[1..]);

            SAY_CONTENT.Insert(msg_chain.GetPlainMessage());
            SAY_CONTENT.Push();

            await receiver.ReplyAsync(msg_chain);
        });
    }


    private class Message
    {
        public required string Content { get; set; }
    }
}