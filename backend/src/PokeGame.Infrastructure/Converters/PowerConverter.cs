﻿using PokeGame.Domain.Moves;

namespace PokeGame.Infrastructure.Converters;

internal class PowerConverter : JsonConverter<Power>
{
  public override Power? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    return reader.TryGetInt32(out int value) ? new Power(value) : null;
  }

  public override void Write(Utf8JsonWriter writer, Power power, JsonSerializerOptions options)
  {
    writer.WriteNumberValue(power.Value);
  }
}
