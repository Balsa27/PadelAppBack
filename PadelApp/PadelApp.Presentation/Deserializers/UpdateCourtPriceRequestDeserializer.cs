using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PadelApp.Application.Commands.Court.UpdateCourtPrice;
using PadelApp.Domain.ValueObjects;

namespace PadelApp.Presentation.Deserializers;

public class UpdateCourtPriceRequestDeserializer : JsonConverter<UpdateCourtPriceRequest>
{
    public override void WriteJson(JsonWriter writer, UpdateCourtPriceRequest? value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }

    public override UpdateCourtPriceRequest? ReadJson(JsonReader reader, Type objectType, UpdateCourtPriceRequest? existingValue,
        bool hasExistingValue, JsonSerializer serializer)
    {
        JObject jsonObj = JObject.Load(reader);

        var courtId = jsonObj["CourtId"].ToObject<Guid>();
        var priceObj = jsonObj["Price"];

        var id = priceObj["Id"].ToObject<Guid>();
        var amount = priceObj["Amount"].ToObject<decimal>();
        var duration = TimeSpan.Parse(priceObj["Duration"].ToString());
        var timeStart = TimeSpan.Parse(priceObj["TimeStart"].ToString());
        var timeEnd = TimeSpan.Parse(priceObj["TimeEnd"].ToString());
        var days = priceObj["Days"].ToObject<List<DayOfWeek>>();

        var price = new Price(id, amount, duration, timeStart, timeEnd, days);

        return new UpdateCourtPriceRequest(courtId, price);
    }
}