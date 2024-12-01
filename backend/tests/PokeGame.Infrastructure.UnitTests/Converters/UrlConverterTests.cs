using PokeGame.Domain;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace PokeGame.Infrastructure.Converters;

[Trait(Traits.Category, Categories.Unit)]
public class UrlConverterTests
{
  private readonly JsonSerializerOptions _serializerOptions = new();

  private readonly Url _url = new("https://bulbapedia.bulbagarden.net/wiki/Kanto");

  public UrlConverterTests()
  {
    _serializerOptions.Converters.Add(new UrlConverter());
    _serializerOptions.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
  }

  [Fact(DisplayName = "Read: it should handle null correctly.")]
  public void Read_it_should_handle_null_correctly()
  {
    Url? url = JsonSerializer.Deserialize<Url>("null", _serializerOptions);
    Assert.Null(url);
  }

  [Fact(DisplayName = "Read: it should read the correct instance from JSON.")]
  public void Read_it_should_read_the_correct_value_from_Json()
  {
    string json = string.Concat('"', _url, '"');
    Url? url = JsonSerializer.Deserialize<Url>(json, _serializerOptions);
    Assert.NotNull(url);
    Assert.Equal(_url, url);
  }

  [Fact(DisplayName = "Write: it should write the correct string value.")]
  public void Write_it_should_write_the_correct_string_value()
  {
    string json = JsonSerializer.Serialize(_url, _serializerOptions);
    Assert.Equal(string.Concat('"', _url, '"'), json);
  }
}
