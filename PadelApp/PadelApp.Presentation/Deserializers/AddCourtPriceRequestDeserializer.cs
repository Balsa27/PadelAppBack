using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PadelApp.Application.Commands.Court.AddCourtPrice;
using PadelApp.Application.Commands.Court.UpdateCourtPrice;
using PadelApp.Domain.ValueObjects;

namespace PadelApp.Presentation.Deserializers;

public class AddCourtPriceRequestDeserializer : JsonConverter<AddCourtPriceRequest>
{
    public override void WriteJson(JsonWriter writer, AddCourtPriceRequest? value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }

    public override AddCourtPriceRequest? ReadJson(JsonReader reader, Type objectType, AddCourtPriceRequest? existingValue,
        bool hasExistingValue, JsonSerializer serializer)
    {
        JObject jsonObj = JObject.Load(reader);

        var courtId = jsonObj["CourtId"].ToObject<Guid>();
        var priceObj = jsonObj["Price"];

        var amount = priceObj["Amount"].ToObject<decimal>();
        var duration = TimeSpan.Parse(priceObj["Duration"].ToString());
        var timeStart = TimeSpan.Parse(priceObj["TimeStart"].ToString());
        var timeEnd = TimeSpan.Parse(priceObj["TimeEnd"].ToString());
        var days = priceObj["Days"].ToObject<List<DayOfWeek>>();


        return new AddCourtPriceRequest(courtId, amount, duration, timeStart, timeEnd, days);
    }
}