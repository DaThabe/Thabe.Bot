namespace Thabe.Bot.Launch.Plugin.Preset;


internal class GetImageMessageUrl : CommandPlugin
{
    public GetImageMessageUrl() : base(nameof(GetImageMessageUrl)) { }

    protected override ICommand CreateCommand()
    {
        return new LinearCommandComposer()
        .AddToken("geturl")
        .AddExpress(x =>
        {
            if (x.Msg is not ImageMessage img) return false;
            x.Value["url"] = img.Url;
            return true;
        })
        .Build();
    }

    protected override async void Excute(MessageReceiverBase receiver, CommandMatchResult result)
    {
        await receiver.ReplyAsync(result["url"] ?? "null");
    }
}
