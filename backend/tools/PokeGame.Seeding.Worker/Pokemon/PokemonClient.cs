using Logitar.Net.Http;
using Logitar.Portal.Client;
using Logitar.Portal.Contracts.Errors;
using Logitar.Portal.Contracts.Search;
using PokeGame.Contracts.Abilities;
using PokeGame.Contracts.Moves;

namespace PokeGame.Seeding.Worker.Pokemon;

internal class PokemonClient : IPokemonClient
{
  private readonly JsonApiClient _client;
  private readonly JsonSerializerOptions _serializerOptions = new();

  private const string AbilitiesPath = "/abilities";
  private static readonly Uri AbilitiesUri = new(AbilitiesPath, UriKind.Relative);
  private const string MovesPath = "/moves";
  private static readonly Uri MovesUri = new(MovesPath, UriKind.Relative);

  public PokemonClient(HttpClient client, PokemonSettings settings)
  {
    _client = new JsonApiClient(client, settings.ToHttpApiSettings());

    _serializerOptions.Converters.Add(new JsonStringEnumConverter());
    _serializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
  }

  public async Task<Ability> CreateAbilityAsync(CreateAbilityPayload payload, CancellationToken cancellationToken)
  {
    return await SendAsync<Ability>(HttpMethod.Post, AbilitiesUri, payload, cancellationToken)
      ?? throw CreateInvalidApiResponseException(nameof(CreateAbilityAsync), HttpMethod.Post, AbilitiesUri, payload);
  }

  public async Task<Move> CreateMoveAsync(CreateMovePayload payload, CancellationToken cancellationToken)
  {
    return await SendAsync<Move>(HttpMethod.Post, MovesUri, payload, cancellationToken)
      ?? throw CreateInvalidApiResponseException(nameof(CreateMoveAsync), HttpMethod.Post, MovesUri, payload);
  }

  public async Task<Ability?> ReplaceAbilityAsync(Guid id, ReplaceAbilityPayload payload, long? version, CancellationToken cancellationToken)
  {
    Uri uri = new UrlBuilder().SetPath($"{AbilitiesPath}/{id}").SetVersion(version).BuildUri(UriKind.Relative);
    return await SendAsync<Ability>(HttpMethod.Put, uri, payload, cancellationToken);
  }
  public async Task<Move?> ReplaceMoveAsync(Guid id, ReplaceMovePayload payload, long? version, CancellationToken cancellationToken)
  {
    Uri uri = new UrlBuilder().SetPath($"{MovesPath}/{id}").SetVersion(version).BuildUri(UriKind.Relative);
    return await SendAsync<Move>(HttpMethod.Put, uri, payload, cancellationToken);
  }

  public async Task<SearchResults<Ability>> SearchAbilitiesAsync(SearchAbilitiesPayload payload, CancellationToken cancellationToken)
  {
    Uri uri = new UrlBuilder().SetPath(AbilitiesPath).SetQuery(payload).BuildUri(UriKind.Relative);
    return await SendAsync<SearchResults<Ability>>(HttpMethod.Get, uri, content: null, cancellationToken)
      ?? throw CreateInvalidApiResponseException(nameof(SearchAbilitiesAsync), HttpMethod.Get, uri, content: null);
  }
  public async Task<SearchResults<Move>> SearchMovesAsync(SearchMovesPayload payload, CancellationToken cancellationToken)
  {
    IUrlBuilder builder = new UrlBuilder().SetPath(MovesPath).SetQuery(payload);
    if (payload.Type.HasValue)
    {
      builder.SetQuery("type", payload.Type.Value.ToString());
    }
    if (payload.Category.HasValue)
    {
      builder.SetQuery("category", payload.Category.Value.ToString());
    }
    Uri uri = builder.BuildUri(UriKind.Relative);
    return await SendAsync<SearchResults<Move>>(HttpMethod.Get, uri, content: null, cancellationToken)
      ?? throw CreateInvalidApiResponseException(nameof(SearchMovesAsync), HttpMethod.Get, uri, content: null);
  }

  private async Task<T?> SendAsync<T>(HttpMethod method, Uri uri, object? content, CancellationToken cancellationToken)
  {
    HttpRequestParameters parameters = new(method, uri);
    if (content != null)
    {
      parameters.Content = JsonContent.Create(content, content.GetType(), mediaType: null, _serializerOptions);
    }

    JsonApiResult result;
    try
    {
      result = await _client.SendAsync(parameters, cancellationToken);
    }
    catch (HttpFailureException<JsonApiResult> exception)
    {
      result = exception.Result;
      if (result.Status.Code == (int)HttpStatusCode.NotFound)
      {
        Error? error = result.Deserialize<Error>(_serializerOptions);
        if (IsNullOrEmpty(error))
        {
          return default;
        }
      }

      throw;
    }

    return result.Deserialize<T>(_serializerOptions);
  }
  private static bool IsNullOrEmpty(Error? error) => error == null
    || (string.IsNullOrWhiteSpace(error.Code) && string.IsNullOrWhiteSpace(error.Message) && error.Data.Count == 0);

  private InvalidApiResponseException CreateInvalidApiResponseException(string methodName, HttpMethod httpMethod, Uri uri, object? content)
  {
    string? serializedContent = content == null ? null : JsonSerializer.Serialize(content, content.GetType(), _serializerOptions);
    return new InvalidApiResponseException(GetType(), methodName, httpMethod, uri, serializedContent, context: null);
  }
}
