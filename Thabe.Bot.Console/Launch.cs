using Thabe.Bot;
using Thabe.Bot.Core.Plugin;
using Thabe.Bot.Core.Preload;

PreloadManager.AddPreloadContent("测试", (bot, logger) =>
{
    PluginManager.RegisterPluginHandel<Thabe.Bot.Cloud.Plugin.thabe.bot.plugin.SayPlugin.Plugin>();
    PluginManager.RegisterPluginHandel<Thabe.Bot.Cloud.Plugin.thabe.bot.plugin.ChatGpt.Plugin>();

    logger.WriteLine("手动注册插件!");
});

await ThabeBot.ConnectAsync();

ThabeBot.Wait();