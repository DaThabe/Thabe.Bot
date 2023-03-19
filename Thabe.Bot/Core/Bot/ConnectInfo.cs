using Newtonsoft.Json;

namespace Thabe.Bot.Core.Bot;


/// <summary>
/// Bot连接信息
/// </summary>
public class ConnectInfo
{
    [JsonProperty("qq")]
    public required string QQ { get; init; }


    [JsonProperty("addres")]
    public required string Addres { get; init; }


    [JsonProperty("verify_key")]
    public required string VerifyKey { get; init; }
}