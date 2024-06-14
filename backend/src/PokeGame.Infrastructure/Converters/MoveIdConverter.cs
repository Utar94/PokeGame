using PokeGame.Domain.Moves;

namespace PokeGame.Infrastructure.Converters;

internal class MoveIdConverter : JsonConverter<MoveId?>
{
  public override MoveId? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    return MoveId.TryCreate(reader.GetString());
  }

  public override void Write(Utf8JsonWriter writer, MoveId? moveId, JsonSerializerOptions options)
  {
    writer.WriteStringValue(moveId?.Value);
  }
}
