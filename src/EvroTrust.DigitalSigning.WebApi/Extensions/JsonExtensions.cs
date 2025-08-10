using System.Text.Json;

namespace EvroTrust.DigitalSigning.WebApi.Extensions
{
    public static class JsonExtensions
    {
        public static string ToJsonString(this object obj, JsonSerializerOptions? options = null)
        {
            ArgumentNullException.ThrowIfNull(obj);
            options ??= new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            };
            
            return JsonSerializer.Serialize(obj, options);
        }
    }
}