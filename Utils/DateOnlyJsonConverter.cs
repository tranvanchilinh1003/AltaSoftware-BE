//using System;
//using System.Text.Json;
//using System.Text.Json.Serialization;

//public class DateOnlyJsonConverter : JsonConverter<DateOnly>
//{
//    private const string DateFormat = "yyyy-MM-dd";

//    public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
//    {
//        return DateOnly.Parse(reader.GetString()!);
//    }

//    public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
//    {
//        writer.WriteStringValue(value.ToString(DateFormat));
//    }
//}

// using System.Text.Json.Serialization;
// using System.Text.Json;

// public class DateOnlyJsonConverter : JsonConverter<DateTime>
// {
//     private const string DateFormat = "yyyy-MM-dd";

//     public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
//     {
//         if (reader.TokenType == JsonTokenType.String)
//         {
//             var dateString = reader.GetString();
//             if (string.IsNullOrWhiteSpace(dateString))
//                 return null; // Nếu chuỗi rỗng "", trả về null

//             if (DateOnly.TryParse(dateString, out var date))
//                 return date;

//             throw new JsonException($"Ngày không hợp lệ: {dateString}");
//         }

//         return null;
//     }

//     public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
//     {
//         if (value.HasValue)
//             writer.WriteStringValue(value.Value.ToString(DateFormat));
//         else
//             writer.WriteNullValue(); // Ghi `null` nếu giá trị là `null`
//     }
// }

