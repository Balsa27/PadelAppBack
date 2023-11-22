    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using PadelApp.Application.Commands.Court.AddCourt;
    using PadelApp.Domain.ValueObjects;

    namespace PadelApp.Presentation.Deserializers;

    public class AddCourtRequestDeserializer : JsonConverter<AddCourtRequest>
    {
        public override void WriteJson(JsonWriter writer, AddCourtRequest? value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override AddCourtRequest? ReadJson(JsonReader reader, Type objectType, AddCourtRequest existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            JObject jsonObject = JObject.Load(reader);

            // Extract properties from jsonObject and convert them to the correct type
            var name = jsonObject["Name"].ToObject<string>();
            var description = jsonObject["Description"].ToObject<string>();
            var address = jsonObject["Address"].ToObject<Address>(); // Assuming Address has a simple conversion
            var workStartTime = TimeSpan.Parse(jsonObject["WorkStartTime"].ToString());
            var workEndTime = TimeSpan.Parse(jsonObject["WorkEndTime"].ToString());
            var imageUrl = jsonObject["ImageUrl"]?.ToObject<string>();
            var courtImages = jsonObject["CourtImages"]?.ToObject<List<string>>();

            // Special handling for Prices
            var pricesArray = jsonObject["Prices"].ToObject<JArray>();
            List<Price> prices = new List<Price>();
            foreach (var priceToken in pricesArray)
            {
                // Extract and convert the Price object properties
                var amount = priceToken["Amount"].ToObject<decimal>();
                var duration = TimeSpan.Parse(priceToken["Duration"].ToString());

                // Handling for nullable TimeStart and TimeEnd
                TimeSpan timeStart = priceToken["TimeStart"] != null ? TimeSpan.Parse(priceToken["TimeStart"].ToString()) : TimeSpan.Zero;
                TimeSpan timeEnd = priceToken["TimeEnd"] != null ? TimeSpan.Parse(priceToken["TimeEnd"].ToString()) : TimeSpan.Zero;

                var days = priceToken["Days"]?.ToObject<List<DayOfWeek>>();

                Price price = new Price(amount, duration, timeStart, timeEnd, days);
                prices.Add(price);
            }

            // Construct and return the AddCourtRequest object
            return new AddCourtRequest(name, description, address, workStartTime, workEndTime, prices, imageUrl, courtImages);
            }
    }