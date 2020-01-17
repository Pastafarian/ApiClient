using Newtonsoft.Json;
namespace Avalara.ApiClient.Serialization
{
    /// <summary>
    /// Serialization methods used by the client.
    /// </summary>
    /// <remarks>
    /// The default used is <see cref="DefaultSerializer"/>. Override as required, e.g. if custom <see cref="JsonConverter"/> are used.
    /// </remarks>
    public interface ISerializer
    {

        string SerializeObject(object value);

        T DeserializeObject<T>(string value);
    }
}