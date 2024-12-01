using PokeGame.Domain;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace PokeGame.Infrastructure.Converters;

[Trait(Traits.Category, Categories.Unit)]
public class DescriptionConverterTests
{
  private readonly JsonSerializerOptions _serializerOptions = new();

  private readonly Description _description = new("The Kanto region is a region of the Pokémon world.");

  public DescriptionConverterTests()
  {
    _serializerOptions.Converters.Add(new DescriptionConverter());
    _serializerOptions.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
  }

  [Fact(DisplayName = "Read: it should handle null correctly.")]
  public void Read_it_should_handle_null_correctly()
  {
    Description? description = JsonSerializer.Deserialize<Description>("null", _serializerOptions);
    Assert.Null(description);
  }

  [Fact(DisplayName = "Read: it should read the correct instance from JSON.")]
  public void Read_it_should_read_the_correct_value_from_Json()
  {
    string json = string.Concat('"', _description, '"');
    Description? description = JsonSerializer.Deserialize<Description>(json, _serializerOptions);
    Assert.NotNull(description);
    Assert.Equal(_description, description);
  }

  [Fact(DisplayName = "Write: it should write the correct string value.")]
  public void Write_it_should_write_the_correct_string_value()
  {
    string json = JsonSerializer.Serialize(_description, _serializerOptions);
    Assert.Equal(string.Concat('"', _description, '"'), json);
  }
}
