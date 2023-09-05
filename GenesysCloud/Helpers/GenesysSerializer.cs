using System.Runtime.Serialization.Json;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Formatting = Newtonsoft.Json.Formatting;

namespace GenesysCloud.Helpers;

public static class GenesysSerializer
{
    public static string JsonSerializeToString(this object json, Type type)
    {
        var formattedJson = JsonConvert.SerializeObject(json, Formatting.Indented);
        return formattedJson ?? string.Empty;
    }
    
    public static T JsonDeserializeFromString<T>(this string objectData)
    {
        try
        {
            return JsonConvert.DeserializeObject<T>(objectData);
        }
        catch (Exception exception)
        {
            throw new InvalidOperationException("Failed to deserialize JSON.", exception);
        }
    }
    
    public static string XmlSerializeToString(this object objectInstance)
    {
        var ns = new XmlSerializerNamespaces();
        var serializer = new XmlSerializer(objectInstance.GetType());
        var settings = new XmlWriterSettings{ Indent = true, OmitXmlDeclaration = true };

        using var stream = new StringWriter();
        using var writer = XmlWriter.Create(stream, settings);
        serializer.Serialize(writer, objectInstance, ns);
        var xml = stream.ToString();
        return xml;
    }

    private static object? XmlDeserializeFromString(this string objectData, Type type)
    {
        var serializer = new XmlSerializer(type);
        using TextReader reader = new StringReader(objectData);
        var result = serializer.Deserialize(reader);
        return result;
    }
}