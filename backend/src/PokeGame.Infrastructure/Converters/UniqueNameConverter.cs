using PokeGame.Domain;

namespace PokeGame.Infrastructure.Converters;

internal class UniqueNameConverter : JsonConverter<UniqueNameUnit>
{
  public override UniqueNameUnit? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    return UniqueNameUnit.TryCreate(reader.GetString());
  }

  public override void Write(Utf8JsonWriter writer, UniqueNameUnit uniqueName, JsonSerializerOptions options)
  {
    writer.WriteStringValue(uniqueName.Value);
  }
}
