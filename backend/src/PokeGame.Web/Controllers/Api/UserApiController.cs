using AutoMapper;
using Logitar.Portal.Client;
using Logitar.Portal.Core.Emails.Messages;
using Logitar.Portal.Core.Emails.Messages.Models;
using Logitar.Portal.Core.Emails.Messages.Payloads;
using Logitar.Portal.Core.Tokens.Models;
using Logitar.Portal.Core.Tokens.Payloads;
using Logitar.Portal.Core.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokeGame.Application.Models;
using PokeGame.Web.Models.Api.User;
using PokeGame.Web.Models.Users;
using PokeGame.Web.Settings;
using System.Text;
using PortalModels = Logitar.Portal.Core;
using UserModels = Logitar.Portal.Core.Users.Models;

namespace PokeGame.Web.Controllers.Api
{
  [ApiController]
  [Authorize(Policy = Constants.Policies.Administrator)]
  [Route("api/users")]
  public class UserApiController : ControllerBase
  {
    private readonly HashSet<RecipientPayload> _bcc;
    private readonly IMapper _mapper;
    private readonly IMessageService _messageService;
    private readonly ClientPortalSettings _portalSettings;
    private readonly ITokenService _tokenService;
    private readonly IUserService _userService;

    public UserApiController(
      IConfiguration configuration,
      IMapper mapper,
      IMessageService messageService,
      ClientPortalSettings portalSettings,
      ITokenService tokenService,
      IUserService userService
    )
    {
      _bcc = configuration.GetSection("Bcc").Get<IEnumerable<string>>()
        ?.Select(email => new RecipientPayload
        {
          Address = email,
          Type = RecipientType.Bcc
        })
        .ToHashSet() ?? new();

      _mapper = mapper;
      _messageService = messageService;
      _portalSettings = portalSettings;
      _tokenService = tokenService;
      _userService = userService;
    }

    [HttpGet]
    public async Task<ActionResult<ListModel<UserSummary>>> GetAsync(bool? isConfirmed, bool? isDisabled, string? search,
      UserSort? sort, bool desc,
      int? index, int? count,
      CancellationToken cancellationToken)
    {
      PortalModels.ListModel<UserModels.UserSummary> users = await _userService.GetAsync(isConfirmed, isDisabled, _portalSettings.Realm, search,
        sort, desc,
        index, count,
        cancellationToken);

      return Ok(_mapper.Map<ListModel<UserSummary>>(users));
    }

    [HttpPost("invite")]
    public async Task<ActionResult> InviteAsync([FromBody] InviteUserPayload payload, CancellationToken cancellationToken)
    {
      string email = payload.Email.Trim().ToLower();
      PortalModels.ListModel<UserModels.UserSummary> users = await _userService.GetAsync(realm: _portalSettings.Realm, search: email, cancellationToken: cancellationToken);
      if (users.Items.Any(x => x.Email?.ToLower() == email))
      {
        return Conflict(new { field = nameof(payload.Email) });
      }

      var createTokenPayload = new CreateTokenPayload
      {
        Email = email,
        Lifetime = Constants.CreateUser.Lifetime,
        Purpose = Constants.CreateUser.Purpose,
        Realm = _portalSettings.Realm
      };
      TokenModel token = await _tokenService.CreateAsync(createTokenPayload, cancellationToken);

      var sendMessagePayload = new SendMessagePayload
      {
        IgnoreUserLocale = true,
        Locale = payload.Locale,
        Realm = _portalSettings.Realm,
        Recipients = GetRecipients(email),
        Template = Constants.CreateUser.Template,
        Variables = GetVariables(payload.Locale, token.Token)
      };
      SentMessagesModel messages = await _messageService.SendAsync(sendMessagePayload, cancellationToken);
      if (messages.Success.Count() != 1 || messages.Error.Any() || messages.Unsent.Any())
      {
        var message = new StringBuilder();

        message.AppendLine("The message sending operation failed.");
        message.AppendLine($"Success ({messages.Success.Count()}): {string.Join(", ", messages.Success)}");
        message.AppendLine($"Error ({messages.Error.Count()}): {string.Join(", ", messages.Error)}");
        message.AppendLine($"Unsent ({messages.Unsent.Count()}): {string.Join(", ", messages.Unsent)}");

        throw new InvalidOperationException(message.ToString());
      }

      return NoContent();
    }

    private IEnumerable<RecipientPayload> GetRecipients(string email)
    {
      var recipients = new List<RecipientPayload>(capacity: 1 + _bcc.Count)
      {
        new() { Address = email }
      };

      recipients.AddRange(_bcc);

      return recipients;
    }

    private static IEnumerable<VariablePayload> GetVariables(string? locale, string token) => new VariablePayload[]
    {
      new()
      {
        Key = "Culture",
        Value = locale
      },
      new()
      {
        Key = "Token",
        Value = token
      }
    };
  }
}
