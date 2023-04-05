using Mirai.Net.Data.Messages;
using Mirai.Net.Data.Messages.Concretes;
using Mirai.Net.Sessions.Http.Managers;
using System.Threading.Tasks;
using Thabe.Bot.Core.Bot;
using Thabe.Bot.Core.Preload;
using Thabe.Bot.Util;

namespace Thabe.Bot.Core.Plugin.RatifyTask;


/// <summary>
/// 批准任务
/// </summary>
public static class RatifyTaskManager
{
    private static readonly List<IRatifyTask> _TASKS = new();

    /// <summary>
    /// 构建批准任务
    /// </summary>
    public static RatifyTaskBuilder BuildRatifyTask(this MessageReceiverBase receiver) => new(receiver);


    /// <summary>
    /// 添加到审批任务
    /// </summary>
    public static void Enqueue(this IRatifyTask ratifyTask)
    {
        if(_TASKS.Find(x => x.Handel == ratifyTask.Handel) != null)
        {
            return;
        }

        _TASKS.Add(ratifyTask);
    }


    class BindRatiryMessage : IPreload
    {
        public string Describe => "绑定批准任务审批消息";

        public void Init(ThabeBot bot, IPreload.Logger logger)
        {
            bot.Core.MessageReceived.Subscribe(x =>
            {
                if (x.GetSenderHandel() != ThabeBot.Config.BOT_MASTER)
                {
                    return;
                }

                if (x.MessageChain is not [_, QuoteMessage quote, PlainMessage plain])
                {
                    return;
                }

                var text = plain.Text.Trim();
                bool? is_ratify = text == "y" ? true : text == "n" ? false : null;

                if (is_ratify == null) return;

                var task = _TASKS.Find(x => x.Handel == quote.MessageId);
                if (task == null) return;

                if (is_ratify == true) task.Agree();
                else if (is_ratify == false) task.Refuse();
            });
        }
    }
}