using Aron.GrassMiner.Extensions;
using Aron.GrassMiner.Models;
using System.Text.Json.Serialization;
using Aron.NetCore.Util.Extensions;
using static Aron.NetCore.Util.Extensions.ServiceExtensions;
using Aron.GrassMiner.ViewModels;
using Aron.NetCore.Util.ViewModels;

namespace Aron.GrassMiner.Services
{
    [JsonSourceGenerationOptions
        (
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Converters = new[] { typeof(JsonStringEnumConverter), typeof(DateTimeConverter) }
        )]
    [JsonSerializable(typeof(MinerRecord))]
    [JsonSerializable(typeof(ResponseResult<LoginResp>))]
    [JsonSerializable(typeof(RequestResult<LoginReq>))]
    [JsonSerializable(typeof(ResponseResult<string>))]
    public partial class MyJsonContext : JsonSerializerContext
    {
        
    }
}
