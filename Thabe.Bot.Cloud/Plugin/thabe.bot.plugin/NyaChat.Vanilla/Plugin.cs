using Mirai.Net.Data.Messages;
using Thabe.Bot.Cloud.Plugin.thabe.bot.plugin.NyaChat.Vanilla.Service;
using Thabe.Bot.Core.Plugin;
using Thabe.Bot.Core.Plugin.Receiver;
using Thabe.Bot.Core.Plugin.Receiver.Interceptor;

namespace Thabe.Bot.Cloud.Plugin.thabe.bot.plugin.ChatGpt;


[Plugin(Package.Name, "ChatGpt - 3.5-Turbo QQ Client")]
public class Plugin
{
    [Receiver(Level = int.MaxValue)]
    public static void AIChat(MessageReceiverBase receiver)
    {
        receiver.ChatGPTReply();
        receiver.SetInterceptor(() => true);
    }
}