using Mirai.Net.Data.Messages;
using Mirai.Net.Data.Messages.Concretes;
using System.Text.RegularExpressions;
using Thabe.Bot.Core.Database.FileHelper;
using Thabe.Bot.Core.Plugin;
using Thabe.Bot.Core.Plugin.RatifyTask;
using Thabe.Bot.Core.Plugin.Receiver;
using Thabe.Bot.Core.Plugin.Trigger;
using Thabe.Bot.Util;

namespace Thabe.Bot.Plugin.Pixiv;



[Plugin(Package.Name, "Pixiv 的一些便捷工具")]
internal class PixivPlugin
{
    [Receiver(Describe = "下载指定的Pid作品")]
    public static void PIDDownload(MessageReceiverBase receiver)
    {
        receiver.TriggerSuccessAction<PidTrigger>(async x =>
        {
            await receiver.ReplyAsync("等一下...");

            ImageMessage img = new()
            {
                Base64 = await $"https://pixiv.re/{x.Pid}.jpg".CacheWebFileToBase64()
            };

            var task = receiver.BuildRatifyTask()
                .AddMessage("申请获取pixiv图片")
                .AddMessage(img)
                .SetAgreeAction(async x =>
                {
                    await x.ReplyAsync(img, Replys.Quote);
                })
                .SetRefuseAction(async x =>
                {
                    await x.ReplyAsync("拒绝了审批");
                })
                .Build();

            task.Enqueue();

            await receiver.ReplyAsync("已提交至审核列表...");
        });
    }
}


file record PidTrigger : ITrigger<PidTrigger>
{
    public required string Pid { get; init; }


    public PidTrigger? Match(MessageReceiverBase receiver)
    {
        if(receiver.MessageChain is not [_, PlainMessage plain])
        {
            return null;
        }

        var result = Regex.Match(plain.Text, @"^\s*/pid\s*(?<id>\d+)\s*$");
        if(!result.Success)
        {
            return null;
        }

        return new() { Pid = result.Groups["id"].Value };
    }
}