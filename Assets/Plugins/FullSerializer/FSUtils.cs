using FullSerializer;
using Newtonsoft.Json;

public static class FSUtils
{
    private static readonly fsSerializer _serializer = new fsSerializer();

    public static string SerializeNS<T>(T content)
    {
        return JsonConvert.SerializeObject(content,Formatting.None,new JsonSerializerSettings() 
        {
            NullValueHandling = NullValueHandling.Ignore,
        });
    }

    public static T DeserializeNS<T>(string json)
    {
        return JsonConvert.DeserializeObject<T>(json,new JsonSerializerSettings() 
        {
            NullValueHandling = NullValueHandling.Ignore
        });
    }

    public static string Serialize<T>(T content)
    {
        fsData data;
        
        _serializer.TrySerialize<T>(content, out data);

        return fsJsonPrinter.PrettyJson(data);
    }

    public static T Deserialize<T>(string serializedState)
    {
        fsData data = fsJsonParser.Parse(serializedState);

        T deserialized = default;
        _serializer.TryDeserialize(data, ref deserialized);

        return deserialized;
    }
}