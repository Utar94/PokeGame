using FluentValidation;
using PokeGame.Application;

namespace PokeGame.Web.Settings
{
  internal class ApiSettingsValidator : AbstractValidator<ApiSettings>
  {
    public ApiSettingsValidator()
    {
      When(x => x.Contact != null, () =>
      {
        RuleFor(x => x.Contact!)
        .SetValidator(new ContactInfoValidator());
      });

      When(x => x.License != null, () =>
      {
        RuleFor(x => x.License!)
        .SetValidator(new LicenseInfoValidator());
      });

      When(x => x.Title != null, () =>
      {
        RuleFor(x => x.Title)
          .NotEmpty();
      });
    }
  }

  internal class ContactInfoValidator : AbstractValidator<ContactInfo>
  {
    public ContactInfoValidator()
    {
      When(x => x.Email != null, () =>
      {
        RuleFor(x => x.Email)
          .NotEmpty()
          .EmailAddress();
      });

      When(x => x.Name != null, () =>
      {
        RuleFor(x => x.Name)
          .NotEmpty();
      });

      When(x => x.Url != null, () =>
      {
        RuleFor(x => x.Url)
          .NotEmpty()
          .Must(ValidationRules.BeAValidUrl);
      });
    }
  }

  internal class LicenseInfoValidator : AbstractValidator<LicenseInfo>
  {
    public LicenseInfoValidator()
    {
      When(x => x.Name != null, () =>
      {
        RuleFor(x => x.Name)
          .NotEmpty();
      });

      When(x => x.Url != null, () =>
      {
        RuleFor(x => x.Url)
          .NotEmpty()
          .Must(ValidationRules.BeAValidUrl);
      });
    }
  }
}
