using Mirai.Net.Data.Messages;
using Mirai.Net.Data.Messages.Concretes;
using PowerArgs;

namespace Thabe.Bot.Util;


/// <summary>
/// 指令帮助类
/// </summary>
public static class CommandManager
{

    private static string To(this AtMessage atMsg)
    {
        return atMsg.ToString();
    }


    //public static T? ExcuteCommand<T>(this MessageReceiverBase receiver) where T : class
    //{
    //    var cache = receiver.ExcuteCommand<Cache>();
    //}


    class Cache
    {
        [ArgRequired, ArgDescription("是否开启缓存"), ArgPosition(1)]
        public bool Open { get; }
    }
}
