using Thabe.Bot;
using Thabe.Bot.Core.Plugin;


PluginManager.LoadAllReceiver<Thabe.Bot.Plugin.Package>();

await ThabeBot.ConnectAsync();
ThabeBot.Wait();