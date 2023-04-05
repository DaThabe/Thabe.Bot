using Mirai.Net.Data.Messages;
using Mirai.Net.Data.Messages.Concretes;
using System.Text;
using Thabe.Bot.Core.Bot;

namespace Thabe.Bot.Core.Plugin.RatifyTask;


/// <summary>
/// 审批任务构建器
/// </summary>
public class RatifyTaskBuilder
{

    private readonly MessageReceiverBase _receiver;
    private readonly MessageChain _chain = new();

    private Action<MessageReceiverBase>? _agreeAction;
    private Action<MessageReceiverBase>? _refuseAction;



    /// <summary>
    /// 
    /// </summary>
    /// <param name="receiver"></param>
    public RatifyTaskBuilder(MessageReceiverBase receiver)
    {
        _receiver = receiver;
        AddSourceInfo();
    }

    private RatifyTaskBuilder AddSourceInfo()
    {
        StringBuilder sb = new();


        _chain.Add(new PlainMessage { Text = sb.ToString() });
        return this;
    }


    public RatifyTaskBuilder AddMessage(string message)
    {

        return this;
    }
    public RatifyTaskBuilder AddMessage(MessageBase message)
    {
        _chain.Add(message);
        return this;
    }
    public RatifyTaskBuilder AddMessage(MessageChain message)
    {
        _chain.AddRange(message);
        return this;
    }

    /// <summary>
    /// 设置同意动作
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    public RatifyTaskBuilder SetAgreeAction(Action<MessageReceiverBase> action)
    {
        _agreeAction = action;
        return this;
    }

    /// <summary>
    /// 设置拒绝动作
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    public RatifyTaskBuilder SetRefuseAction(Action<MessageReceiverBase> action)
    {
        _refuseAction = action;
        return this;
    }

    public IRatifyTask Build()
    {
        _chain.Add(new PlainMessage { Text = "引用回复 y|n [message]" });

        return new DefaultRatifyTask(_receiver, _chain)
        {
            AgreeAction = _agreeAction,
            RefuseAction = _refuseAction
        };
    }


    class DefaultRatifyTask : IRatifyTask
    {
        private readonly MessageChain _tipchain;
        private readonly MessageReceiverBase _receiver;
        

        public Action<MessageReceiverBase>? AgreeAction { get; init; }
        public Action<MessageReceiverBase>? RefuseAction { get; init; }

        public string Handel { get; }


        public DefaultRatifyTask(MessageReceiverBase receiver, MessageChain tipChain)
        {
            if(receiver.MessageChain is not [SourceMessage source, ..])
            {
                throw new ArgumentException();
            }

            Handel = source.MessageId;
            _receiver = receiver;
            _tipchain = tipChain;

            Handel = _tipchain.SendMasterMessageAsync().GetAwaiter().GetResult();
        }

        public void Agree() => AgreeAction?.Invoke(_receiver);

        public void Refuse() => RefuseAction?.Invoke(_receiver);
    }
}
