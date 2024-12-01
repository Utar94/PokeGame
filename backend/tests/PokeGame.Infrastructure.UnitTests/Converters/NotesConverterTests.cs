using PokeGame.Domain;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace PokeGame.Infrastructure.Converters;

[Trait(Traits.Category, Categories.Unit)]
public class NotesConverterTests
{
  private readonly JsonSerializerOptions _serializerOptions = new();

  private readonly Notes _notes = new("Kanto is located east of Johto, which together form a joint landmass that is south of Sinnoh.");

  public NotesConverterTests()
  {
    _serializerOptions.Converters.Add(new NotesConverter());
    _serializerOptions.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
  }

  [Fact(DisplayName = "Read: it should handle null correctly.")]
  public void Read_it_should_handle_null_correctly()
  {
    Notes? notes = JsonSerializer.Deserialize<Notes>("null", _serializerOptions);
    Assert.Null(notes);
  }

  [Fact(DisplayName = "Read: it should read the correct instance from JSON.")]
  public void Read_it_should_read_the_correct_value_from_Json()
  {
    string json = string.Concat('"', _notes, '"');
    Notes? notes = JsonSerializer.Deserialize<Notes>(json, _serializerOptions);
    Assert.NotNull(notes);
    Assert.Equal(_notes, notes);
  }

  [Fact(DisplayName = "Write: it should write the correct string value.")]
  public void Write_it_should_write_the_correct_string_value()
  {
    string json = JsonSerializer.Serialize(_notes, _serializerOptions);
    Assert.Equal(string.Concat('"', _notes, '"'), json);
  }
}
