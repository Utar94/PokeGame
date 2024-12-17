using PokeGame.Domain.Moves;

namespace PokeGame.Infrastructure.Converters;

internal class VolatileConditionConverter : JsonConverter<VolatileCondition>
{
  public override VolatileCondition? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    return VolatileCondition.TryCreate(reader.GetString());
  }

  public override void Write(Utf8JsonWriter writer, VolatileCondition volatileCondition, JsonSerializerOptions options)
  {
    writer.WriteStringValue(volatileCondition.Value);
  }
}
