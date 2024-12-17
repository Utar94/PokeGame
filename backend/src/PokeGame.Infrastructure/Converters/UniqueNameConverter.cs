using PokeGame.Domain;

namespace PokeGame.Infrastructure.Converters;

internal class UniqueNameConverter : JsonConverter<UniqueName>
{
  public override UniqueName? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    return UniqueName.TryCreate(reader.GetString());
  }

  public override void Write(Utf8JsonWriter writer, UniqueName uniqueName, JsonSerializerOptions options)
  {
    writer.WriteStringValue(uniqueName.Value);
  }
}
