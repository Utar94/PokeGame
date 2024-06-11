using PokeGame.Domain;

namespace PokeGame.Infrastructure.Converters;

internal class ReferenceConverter : JsonConverter<ReferenceUnit>
{
  public override ReferenceUnit? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    return ReferenceUnit.TryCreate(reader.GetString());
  }

  public override void Write(Utf8JsonWriter writer, ReferenceUnit reference, JsonSerializerOptions options)
  {
    writer.WriteStringValue(reference.Value);
  }
}
