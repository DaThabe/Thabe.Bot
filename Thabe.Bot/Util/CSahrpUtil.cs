using System.Runtime.Loader;
using System.Reflection;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using Thabe.Bot.Core.Plugin.Script;
using System.Security.Cryptography;
using System.Text;

namespace Thabe.Bot.Util;


/// <summary>
/// C#帮助类
/// </summary>
public static class CSahrpUtil
{
    #region --反射类扩展--

    /// <summary>
    /// 获取Thabe.Bot引用程序集中的所有类型
    /// </summary>
    public static IEnumerable<Type> GetAllTypes()
    {
        return from assembly in AssemblyLoadContext.Default.Assemblies
               from type in assembly.GetTypes()
               select type;
    }

    /// <summary>
    /// 从程序集加载上下文中获取所有类型
    /// </summary>
    public static IEnumerable<Type> GetAllTypes(this AssemblyLoadContext loader)
    {
        return from assembly in loader.Assemblies
               from type in assembly.GetTypes()
               select type;
    }

    /// <summary>
    /// 从类型中获取指定的类型
    /// </summary>
    public static IEnumerable<Type> OfType<T>(this IEnumerable<Type> types)
    {
        foreach (var i in types)
        {
            if (!typeof(T).IsAssignableFrom(i)) continue;

            yield return i;
        }
    }


    /// <summary>
    /// 动态编译C#代码
    /// </summary>
    /// <param name="dllName">插件类名称</param>
    /// <param name="code">代码</param>
    /// <exception cref="Exception"></exception>
    public static (AssemblyLoadContext Context, Assembly Assembly) DynamicCompilation(string? dllName, string code)
    {
        var syntaxTree = CSharpSyntaxTree.ParseText(code);
        AssemblyLoadContext loader = new(dllName);

        ///动态库类型编译
        var dll = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);

        // 这算是偷懒了吗？我把.NET Core 运行时用到的那些引用都加入到引用了。
        // 加入引用是必要的，不然连 object 类型都是没有的，肯定编译不通过。
        var references = from i in AppDomain.CurrentDomain.GetAssemblies()
                         where !string.IsNullOrEmpty(i.Location)
                         select MetadataReference.CreateFromFile(i.Location);

        var mirai_ref = MetadataReference.CreateFromFile(typeof(ScriptManager).Assembly.Location);

        // 指定编译选项。
        var assemblyName = $"{dllName}.g";
        var compilation = CSharpCompilation.Create(assemblyName, new[] { syntaxTree }, options: dll)
            .AddReferences(references).AddReferences(mirai_ref);

        // 编译到内存流中。
        using MemoryStream ms = new();
        var result = compilation.Emit(ms);

        //编译失败
        if (!result.Success)
        {
            throw new Exception(string.Join(Environment.NewLine, result.Diagnostics));
        }

        ms.Seek(0, SeekOrigin.Begin);

        return (loader, loader.LoadFromStream(ms));
    }

    #endregion


    #region --迭代器类扩展--


    /// <summary>
    /// 遍历一个实现迭代器的数据组
    /// </summary>
    /// <param name="action">遍历期间每个数据的处理方法</param>
    public static void Foreach<T>(this IEnumerable<T> values, Action<T> action)
    {
        foreach (var item in values)
        {
            action(item);
        }
    }

    /// <summary>
    /// 异步遍历
    /// </summary>
    /// <param name="action">遍历期间每个数据的处理方法</param>
    public static Task ForeachAsync<T>(this IEnumerable<T> values, Action<T> action)
    {
        return Task.Run(() =>
        {
            foreach (var item in values) action(item);
        });
    }


    /// <summary>
    /// 尝试遍历一个实现迭代器的数据组 会拦截每一个Action出现的异常
    /// </summary>
    /// <param name="action">遍历期间每个数据的处理方法</param>
    /// <param name="exception">拦截的异常处理方法</param>
    public static void TryForeach<T>(this IEnumerable<T> values, Action<T> action, Action<Exception>? exception = null)
    {
        foreach (var item in values)
        {
            try
            {
                action(item);
            }
            catch (Exception ex)
            {
                exception?.Invoke(ex);
            }
        }
    }


    /// <summary>
    /// 尝试同步遍历一个实现迭代器的数据组 会拦截每一个Action出现的异常
    /// </summary>
    /// <param name="action">遍历期间每个数据的处理方法</param>
    /// <param name="exception">拦截的异常处理方法</param>
    public static Task TryForeachAsync<T>(this IEnumerable<T> values, Action<T> action, Action<Exception>? exception = null)
    {
        return Task.Run(() =>
        {
            foreach (var item in values)
            {
                try
                {
                    action(item);
                }
                catch (Exception ex)
                {
                    exception?.Invoke(ex);
                }
            }
        });
    }

    #endregion


    #region --字符串--


    /// <summary>
    /// 判断输入的字符串是否在前面, 如果在前面就返回移除之后的字符串, 如果不存在或长度大于自己返回原来的
    /// </summary>
    /// <param name="removeStr">需要比较和删除的字符串</param>
    public static string StartWithRemove(this string str, string removeStr)
    {
        if (str.StartsWith(removeStr))
        {
            return str.Remove(0, removeStr.Length);
        }

        return str;
    }


    /// <summary>
    /// 吧字符串转为MD5
    /// </summary>
    /// <param name="format">"x2"结果为32位,"x3"结果为48位,"x4"结果为64位</param>
    public static string ToMD5(this string str, string format = "x2")
    {
        byte[] data = MD5.HashData(Encoding.UTF8.GetBytes(str));

        StringBuilder sb = new();

        for (int i = 0; i < data.Length; i++)
        {
            sb.Append(data[i].ToString(format));
        }

        return sb.ToString();
    }

    #endregion
}