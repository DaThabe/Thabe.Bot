using Spectre.Console;
using Thabe.Bot.Core.Logger;

namespace Thabe.Bot.Util;


/// <summary>
/// 控制台帮助类
/// </summary>
public static class ConsoleScaffold
{
    /// <summary>
    /// 转为<see cref="AnsiConsole.Markup"/>支持带颜色文本语法
    /// </summary>
    /// <param name="str"></param>
    /// <param name="colorName"></param>
    /// <returns></returns>
    public static string Color(this string str, string colorName)
    {
        return $"[{colorName}]{str.ColorRemove()}[/]";
    }

    /// <summary>
    /// 转为<see cref="AnsiConsole.Markup"/>支持带颜色文本语法
    /// </summary>
    /// <param name="str"></param>
    /// <param name="level">日志等级</param>
    /// <returns></returns>
    public static string Color(this string str, LogLevel level)
    {
        return level switch
        {
            LogLevel.Unimportance => str.ColorUnimportant(),
            LogLevel.Tips => str.ColorTips(),
            LogLevel.Important => str.ColorHighlight(),
            LogLevel.Warning => str.ColorWarning(),
            LogLevel.Error => str.ColorError(),
            _ => str,
        };
    }

    /// <summary>
    /// 移除颜色标识
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string ColorRemove(this string str) => str.Replace("[", "[[").Replace("]", "]]");

    /// <summary>
    /// 提示文本 绿色
    /// </summary>
    public static string ColorTips(this string str) => str.Color("springgreen2");

    /// <summary>
    /// 警告文本 黄色
    /// </summary>
    public static string ColorWarning(this string str) => str.Color("yellow1");

    /// <summary>
    /// 错误文本 红色
    /// </summary>
    public static string ColorError(this string str) => str.Color("indianred1_1");

    /// <summary>
    /// 高亮文本 紫色
    /// </summary>
    public static string ColorHighlight(this string str) => str.Color("violet");

    /// <summary>
    /// 不重要的文本 灰色
    /// </summary>
    public static string ColorUnimportant(this string str) => str.Color("grey30");
}