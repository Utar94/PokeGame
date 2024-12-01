using PokeGame.Domain;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace PokeGame.Infrastructure.Converters;

[Trait(Traits.Category, Categories.Unit)]
public class UniqueNameConverterTests
{
  private readonly JsonSerializerOptions _serializerOptions = new();

  private readonly UniqueName _uniqueName = new("kanto");

  public UniqueNameConverterTests()
  {
    _serializerOptions.Converters.Add(new UniqueNameConverter());
    _serializerOptions.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
  }

  [Fact(DisplayName = "Read: it should handle null correctly.")]
  public void Read_it_should_handle_null_correctly()
  {
    UniqueName? uniqueName = JsonSerializer.Deserialize<UniqueName>("null", _serializerOptions);
    Assert.Null(uniqueName);
  }

  [Fact(DisplayName = "Read: it should read the correct instance from JSON.")]
  public void Read_it_should_read_the_correct_value_from_Json()
  {
    string json = string.Concat('"', _uniqueName, '"');
    UniqueName? uniqueName = JsonSerializer.Deserialize<UniqueName>(json, _serializerOptions);
    Assert.NotNull(uniqueName);
    Assert.Equal(_uniqueName, uniqueName);
  }

  [Fact(DisplayName = "Write: it should write the correct string value.")]
  public void Write_it_should_write_the_correct_string_value()
  {
    string json = JsonSerializer.Serialize(_uniqueName, _serializerOptions);
    Assert.Equal(string.Concat('"', _uniqueName, '"'), json);
  }
}
