using PokeGame.Domain.Abilities;

namespace PokeGame.Infrastructure.Converters;

internal class AbilityIdConverter : JsonConverter<AbilityId?>
{
  public override AbilityId? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    return AbilityId.TryCreate(reader.GetString());
  }

  public override void Write(Utf8JsonWriter writer, AbilityId? abilityId, JsonSerializerOptions options)
  {
    writer.WriteStringValue(abilityId?.Value);
  }
}
