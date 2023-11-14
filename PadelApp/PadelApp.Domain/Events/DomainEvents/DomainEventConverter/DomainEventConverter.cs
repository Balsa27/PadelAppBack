using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PadelApp.Domain.Primitives;

namespace PadelApp.Domain.DomainEvents.DomainEventConverter;

public class DomainEventConverter : JsonConverter<IDomainEvent>
{
    public override void WriteJson(JsonWriter writer, IDomainEvent? value, JsonSerializer serializer)
    => serializer.Serialize(writer, value);
    
    public override bool CanWrite => true;

    public override IDomainEvent? ReadJson(JsonReader reader, Type objectType, IDomainEvent? existingValue, bool hasExistingValue,
        JsonSerializer serializer)
    {
        var jsonObject = JObject.Load(reader);
        var eventType = jsonObject["Type"]!.Value<string>(); 

        IDomainEvent? domainEvent = null;

        switch (eventType)
        {
            case "PlayerRegisteredDomainEvent":
                domainEvent = jsonObject.ToObject<PlayerRegisteredDomainEvent>(serializer);
                break;
            // Add more cases here for other domain events
            default:
                throw new InvalidOperationException($"Unknown event type: {eventType}");
        }

        return domainEvent;
    }
    
}