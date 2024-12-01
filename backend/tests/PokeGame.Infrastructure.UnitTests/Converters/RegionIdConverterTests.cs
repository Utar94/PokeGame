using PokeGame.Domain.Regions;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace PokeGame.Infrastructure.Converters;

[Trait(Traits.Category, Categories.Unit)]
public class RegionIdConverterTests
{
  private readonly JsonSerializerOptions _serializerOptions = new();

  private readonly RegionId _regionId = RegionId.NewId();

  public RegionIdConverterTests()
  {
    _serializerOptions.Converters.Add(new RegionIdConverter());
    _serializerOptions.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
  }

  [Fact(DisplayName = "Read: it should handle null correctly.")]
  public void Read_it_should_handle_null_correctly()
  {
    RegionId regionId = JsonSerializer.Deserialize<RegionId>("null", _serializerOptions);
    Assert.Equal(string.Empty, regionId.Value);
  }

  [Fact(DisplayName = "Read: it should read the correct instance from JSON.")]
  public void Read_it_should_read_the_correct_value_from_Json()
  {
    string json = string.Concat('"', _regionId, '"');
    RegionId regionId = JsonSerializer.Deserialize<RegionId>(json, _serializerOptions);
    Assert.Equal(_regionId, regionId);
  }

  [Fact(DisplayName = "Write: it should write the correct string value.")]
  public void Write_it_should_write_the_correct_string_value()
  {
    string json = JsonSerializer.Serialize(_regionId, _serializerOptions);
    Assert.Equal(string.Concat('"', _regionId, '"'), json);
  }
}
