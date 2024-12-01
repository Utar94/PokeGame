using PokeGame.Domain;

namespace PokeGame.Infrastructure.Converters;

internal class UrlConverter : JsonConverter<Url>
{
  public override Url? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    string? value = reader.GetString();
    return string.IsNullOrWhiteSpace(value) ? null : new Url(value);
  }

  public override void Write(Utf8JsonWriter writer, Url url, JsonSerializerOptions options)
  {
    writer.WriteStringValue(url.Value);
  }
}
