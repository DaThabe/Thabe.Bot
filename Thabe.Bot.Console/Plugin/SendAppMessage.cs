using Thabe.Bot.QQ.Command.Linear;
using Thabe.Bot.QQ.PluginTemplate.Command;

namespace Thabe.Bot.Launch.Plugin.Preset;


/// <summary>
/// 发送AppMessage
/// </summary>
internal class SendAppMessage : CommandPlugin
{
    public SendAppMessage() : base(nameof(SendAppMessage))
    {
    }

    protected override ICommand CreateCommand()
    {
        return new LinearCommandComposer().AddToken("send").AddToken("appmessage", "appmsg").AddRegex("content",".*").Build();
    }

    protected override async void Excute(MessageReceiverBase receiver, CommandMatchResult result)
    {
        await receiver.ReplyAsync(new AppMessage() { Content = result["content"] });
    }
}


internal class Tips : CommandPlugin
{
    public Tips() : base(nameof(Tips))
    {
    }

    protected override ICommand CreateCommand()
    {
        return new LinearCommandComposer()
            .AddToken("tips")
            .AddOption("title", ".+")
            .AddOption("tip", ".+")
            .AddOption("msg", ".+")
            .AddOption("icon", ".+")
            .Build();
    }

    protected override async void Excute(MessageReceiverBase receiver, CommandMatchResult result)
    {
        string json = $$"""
            {
              "app": "com.tencent.miniapp",
              "desc": "",
              "view": "notification",
              "ver": "0.0.0.1",
              "prompt": "{{"Test"}}",
              "meta": {
                "notification": {
                  "appInfo": {
                    "appName": "{{result["title"]}}",
                    "appType": 4,
                    "appid": 2034149631,
                    "iconUrl": "{{result["icon"]}}",
                  },
                  "data": [
                    {
                      "title": "{{result["tips"]}}",
                      "value": "{{result["msg"]}}"
                    }
                  ],
                  "emphasis_keyword": ""
                }
              }
            }
            """;


        await receiver.ReplyAsync(new AppMessage() { Content = json });
    }
}