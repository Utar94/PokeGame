using FluentValidation;

namespace PokeGame.Web.Settings
{
  internal class PortalSettingsValidator : AbstractValidator<ClientPortalSettings>
  {
    public PortalSettingsValidator()
    {
      RuleFor(x => x.Realm)
        .NotEmpty();
    }
  }
}
