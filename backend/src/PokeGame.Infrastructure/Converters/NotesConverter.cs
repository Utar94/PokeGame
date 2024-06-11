using PokeGame.Domain;

namespace PokeGame.Infrastructure.Converters;

internal class NotesConverter : JsonConverter<NotesUnit>
{
  public override NotesUnit? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    return NotesUnit.TryCreate(reader.GetString());
  }

  public override void Write(Utf8JsonWriter writer, NotesUnit notes, JsonSerializerOptions options)
  {
    writer.WriteStringValue(notes.Value);
  }
}
