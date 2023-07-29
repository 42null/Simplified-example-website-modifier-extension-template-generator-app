using System;
using System.Text.Json;
using System.Text.Json.Serialization;

public class JsonControllerGeneric
{
    GenericJsonObject genericJsonObject;

    public JsonControllerGeneric(String jsonStartString)
    {
        // Deserialize the JSON string to JsonElement
        JsonElement root = JsonDocument.Parse(jsonStartString).RootElement;

        // Convert the JsonElement to a GenericJsonObject
        genericJsonObject = new GenericJsonObject(root);
    }

    public string GetString(string propertyName)
    {
        return genericJsonObject.GetString(propertyName);
    }

    public GenericJsonObject GetGenericObject()
    {
        return genericJsonObject;
    }
    public GenericJsonObject GetGenericObject(string propertyName)
    {
        return genericJsonObject.GetGenericObject(propertyName);
    }
}

// Custom JsonObject class to simplify property access
public class GenericJsonObject
{
    private readonly JsonElement _element;

    public GenericJsonObject(JsonElement element)
    {
        _element = element;
    }

    public string GetString(string propertyName)
    {
        return _element.TryGetProperty(propertyName, out var property) ? property.GetString() : null;
    }
    
    public GenericJsonObject GetGenericObject(string propertyName)
    {
        return _element.TryGetProperty(propertyName, out var property) ? new GenericJsonObject(property) : null;
    }
    
    
    

    public int GetInt32(string propertyName)
    {
        return _element.TryGetProperty(propertyName, out var property) ? property.GetInt32() : 0;
    }

    public bool GetBoolean(string propertyName)
    {
        return _element.TryGetProperty(propertyName, out var property) && property.GetBoolean();
    }
}

// using System.Text.Json;
//
// namespace ExtensionGenerator;
//
// public class JsonControllerGeneric
// {
//     // private string jsonStartingString;
//     private readonly JsonElement _element;
//
//     public JsonControllerGeneric(string jsonAsString)
//     {
//         // Deserialize the JSON string to JsonDocument
//         using JsonDocument document = JsonDocument.Parse(jsonAsString);
//
//         // Get the root element
//         _element = document.RootElement;
//     }
//
//     public string GetString(string propertyName)
//     {
//         return _element.TryGetProperty(propertyName, out var property) ? property.GetString() : null;
//     }
//
//     public int GetInt32(string propertyName)
//     {
//         return _element.TryGetProperty(propertyName, out var property) ? property.GetInt32() : 0;
//     }
//
//     public bool GetBoolean(string propertyName)
//     {
//         return _element.TryGetProperty(propertyName, out var property) && property.GetBoolean();
//     }
// }
