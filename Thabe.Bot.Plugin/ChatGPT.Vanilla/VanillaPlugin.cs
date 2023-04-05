using Mirai.Net.Data.Messages;
using Thabe.Bot.Core.Plugin;
using Thabe.Bot.Core.Plugin.Receiver.Interceptor;
using Thabe.Bot.Plugin.ChatGPT.Vanilla.Service;

namespace Thabe.Bot.Plugin.ChatGPT.Vanilla;


[Plugin(Package.Name, "ChatGpt - 3.5-Turbo QQ Client")]
public class VanillaPlugin
{
    //[Receiver(Level = int.MaxValue)]
    public static void AIChat(MessageReceiverBase receiver)
    {
        receiver.ChatGPTReply();
        receiver.SetInterceptor(() => false);
    }
}