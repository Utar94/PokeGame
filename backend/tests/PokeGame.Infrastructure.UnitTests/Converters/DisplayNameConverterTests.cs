using PokeGame.Domain;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace PokeGame.Infrastructure.Converters;

[Trait(Traits.Category, Categories.Unit)]
public class DisplayNameConverterTests
{
  private readonly JsonSerializerOptions _serializerOptions = new();

  private readonly DisplayName _displayName = new("Kanto.");

  public DisplayNameConverterTests()
  {
    _serializerOptions.Converters.Add(new DisplayNameConverter());
    _serializerOptions.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
  }

  [Fact(DisplayName = "Read: it should handle null correctly.")]
  public void Read_it_should_handle_null_correctly()
  {
    DisplayName? displayName = JsonSerializer.Deserialize<DisplayName>("null", _serializerOptions);
    Assert.Null(displayName);
  }

  [Fact(DisplayName = "Read: it should read the correct instance from JSON.")]
  public void Read_it_should_read_the_correct_value_from_Json()
  {
    string json = string.Concat('"', _displayName, '"');
    DisplayName? displayName = JsonSerializer.Deserialize<DisplayName>(json, _serializerOptions);
    Assert.NotNull(displayName);
    Assert.Equal(_displayName, displayName);
  }

  [Fact(DisplayName = "Write: it should write the correct string value.")]
  public void Write_it_should_write_the_correct_string_value()
  {
    string json = JsonSerializer.Serialize(_displayName, _serializerOptions);
    Assert.Equal(string.Concat('"', _displayName, '"'), json);
  }
}
