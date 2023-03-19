using ChatGPTSharp;
using ChatGPTSharp.Model;

namespace Thabe.Bot.Launch.Plugin.Preset;


/// <summary>
/// 插件
/// </summary>
internal partial class ShowAppMessage : PresetPlugin
{
    public ShowAppMessage() : base(nameof(ShowAppMessage)) { }

    public static async void Method(FriendMessageReceiver receiver)
    {
        if (receiver.MessageChain is not [_, AppMessage app]) return;

        MessagePack pack = new(QQBot.Master, "AppMessage");
        pack.Add(receiver.FormatSenderInfo());
        pack.Add(app);
        pack.Add(app.Content);

        await receiver.ReplyAsync(pack.Build());
    }
}