using PokeGame.Domain;

namespace PokeGame.Infrastructure.Converters;

internal class DescriptionConverter : JsonConverter<Description>
{
  public override Description? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    string? value = reader.GetString();
    return string.IsNullOrWhiteSpace(value) ? null : new Description(value);
  }

  public override void Write(Utf8JsonWriter writer, Description description, JsonSerializerOptions options)
  {
    writer.WriteStringValue(description.Value);
  }
}
