using System.Text;

namespace Thabe.Bot.Core.Preload;


/// <summary>
/// ThabeConsole 预加载调用接口
/// </summary>
public interface IPreload
{
    /// <summary>
    /// 预加载作用描述
    /// </summary>
    string Describe { get; }

    /// <summary>
    /// 预加载处理方法
    /// </summary>
    void Init(ThabeBot bot, Logger logger);


    /// <summary>
    /// 预加载日志记录器
    /// </summary>
    public class Logger
    {
        private StringBuilder sb = new();

        public void Write(string message) => sb.Append(message);
        public void WriteLine(string message) => sb.AppendLine(message);

        public override string ToString() => sb.ToString();
    }
}