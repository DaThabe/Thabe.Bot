using Mirai.Net.Data.Messages.Concretes;
using Mirai.Net.Data.Messages.Receivers;
using System.Diagnostics;
using Thabe.Bot.Core.Plugin;
using Thabe.Bot.Core.Plugin.Receiver;
using Thabe.Bot.Core.Plugin.Receiver.Interceptor;
using Thabe.Bot.Util;

namespace Thabe.Bot.Plugin.Terminal;


[Plugin(Package.Name, "终端插件")]
public class TerminalPlugin
{
    [Receiver(Describe = "插件主方法")]
    public static async void Main(FriendMessageReceiver receiver)
    {
        if(receiver.MessageChain is not [_, PlainMessage p])
        {
            return;
        }
        if (!p.Text.Trim().StartsWith("$")) return;

        receiver.SetInterceptor(() => false);

        var cmd = p.Text.Trim().StartWithRemove("$");
        await receiver.ReplyAsync(await ExcuteCommandAsync(cmd));
    }

    public static async Task<string> ExcuteCommandAsync(string cmd)
    {
        return await Task.Run(() =>
        {
            using Process process = new()
            {
                StartInfo = new()
                {
                    FileName = "cmd.exe",
                    Arguments = "/c " + cmd,
                    UseShellExecute = false,
                    RedirectStandardOutput = true
                }
            };

            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            return output;
        });
    }
}
