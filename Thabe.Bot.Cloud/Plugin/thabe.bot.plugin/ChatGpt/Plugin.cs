using Mirai.Net.Data.Messages;
using Thabe.Bot.Core.Plugin;
using Thabe.Bot.Core.Plugin.Receiver;

namespace Thabe.Bot.Cloud.Plugin.thabe.bot.plugin.ChatGpt;


[Plugin(Package.Name, "ChatGpt - 3.5-Turbo QQ Client")]
public class Plugin
{
    [Receiver]
    public static void AIChat(MessageReceiverBase receiver) => receiver.ChatGPTReply();
}