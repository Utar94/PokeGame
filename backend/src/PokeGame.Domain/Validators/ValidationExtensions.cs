using FluentValidation;
using Logitar.Identity.Contracts.Settings;

namespace PokeGame.Domain.Validators;

public static class ValidationExtensions
{
  private const int GenderMaximumLength = byte.MaxValue;
  private const int PersonNameMaximumLength = byte.MaxValue;
  private const int TimeZoneMaximumLength = 32;

  public static IRuleBuilderOptions<T, DateTime> Birthdate<T>(this IRuleBuilder<T, DateTime> ruleBuilder)
  {
    return ruleBuilder.Past();
  }

  public static IRuleBuilderOptions<T, string> Description<T>(this IRuleBuilder<T, string> ruleBuilder)
  {
    return ruleBuilder.NotEmpty();
  }

  public static IRuleBuilderOptions<T, string> DisplayName<T>(this IRuleBuilder<T, string> ruleBuilder)
  {
    return ruleBuilder.NotEmpty().MaximumLength(Domain.DisplayName.MaximumLength);
  }

  public static IRuleBuilderOptions<T, DateTime> Future<T>(this IRuleBuilder<T, DateTime> ruleBuilder, DateTime? now = null)
  {
    return ruleBuilder.SetValidator(new FutureValidator<T>(now));
  }

  public static IRuleBuilderOptions<T, string> Gender<T>(this IRuleBuilder<T, string> ruleBuilder)
  {
    return ruleBuilder.NotEmpty().MaximumLength(GenderMaximumLength);
  }

  public static IRuleBuilderOptions<T, string> Locale<T>(this IRuleBuilder<T, string> ruleBuilder)
  {
    return ruleBuilder.NotEmpty().MaximumLength(Domain.Locale.MaximumLength).SetValidator(new LocaleValidator<T>());
  }

  public static IRuleBuilderOptions<T, string> Name<T>(this IRuleBuilder<T, string> ruleBuilder)
  {
    return ruleBuilder.NotEmpty().MaximumLength(Domain.Name.MaximumLength);
  }

  public static IRuleBuilderOptions<T, string> Notes<T>(this IRuleBuilder<T, string> ruleBuilder)
  {
    return ruleBuilder.NotEmpty();
  }

  public static IRuleBuilderOptions<T, string> Password<T>(this IRuleBuilder<T, string> ruleBuilder, IPasswordSettings settings)
  {
    IRuleBuilderOptions<T, string> options = ruleBuilder.NotEmpty();
    if (settings.RequiredLength > 1)
    {
      options = options.MinimumLength(settings.RequiredLength)
        .WithErrorCode("PasswordTooShort")
        .WithMessage($"'{{PropertyName}}' must be at least {settings.RequiredLength} characters.");
    }
    if (settings.RequiredUniqueChars > 1)
    {
      options = options.Must(x => x.GroupBy(c => c).Count() >= settings.RequiredUniqueChars)
        .WithErrorCode("PasswordRequiresUniqueChars")
        .WithMessage($"'{{PropertyName}}' must use at least {settings.RequiredUniqueChars} different characters.");
    }
    if (settings.RequireNonAlphanumeric)
    {
      options = options.Must(x => x.Any(c => !char.IsLetterOrDigit(c)))
        .WithErrorCode("PasswordRequiresNonAlphanumeric")
        .WithMessage($"'{{PropertyName}}' must have at least one non alphanumeric character.");
    }
    if (settings.RequireLowercase)
    {
      options = options.Must(x => x.Any(char.IsLower))
        .WithErrorCode("PasswordRequiresLower")
        .WithMessage($"'{{PropertyName}}' must have at least one lowercase ('a'-'z').");
    }
    if (settings.RequireUppercase)
    {
      options = options.Must(x => x.Any(char.IsUpper))
        .WithErrorCode("PasswordRequiresUpper")
        .WithMessage($"'{{PropertyName}}' must have at least one uppercase ('A'-'Z').");
    }
    if (settings.RequireDigit)
    {
      options = options.Must(x => x.Any(char.IsDigit))
        .WithErrorCode("PasswordRequiresDigit")
        .WithMessage($"'{{PropertyName}}' must have at least one digit ('0'-'9').");
    }
    return options;
  }

  public static IRuleBuilderOptions<T, DateTime> Past<T>(this IRuleBuilder<T, DateTime> ruleBuilder, DateTime? now = null)
  {
    return ruleBuilder.SetValidator(new PastValidator<T>(now));
  }

  public static IRuleBuilderOptions<T, string> PersonName<T>(this IRuleBuilder<T, string> ruleBuilder)
  {
    return ruleBuilder.NotEmpty().MaximumLength(PersonNameMaximumLength);
  }

  public static IRuleBuilderOptions<T, string> TimeZone<T>(this IRuleBuilder<T, string> ruleBuilder)
  {
    return ruleBuilder.NotEmpty().MaximumLength(TimeZoneMaximumLength).SetValidator(new TimeZoneValidator<T>());
  }

  public static IRuleBuilderOptions<T, string> UniqueName<T>(this IRuleBuilder<T, string> ruleBuilder)
  {
    // TODO(fpion): "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_"
    return ruleBuilder.NotEmpty().MaximumLength(Domain.UniqueName.MaximumLength);
  }

  public static IRuleBuilderOptions<T, string> Url<T>(this IRuleBuilder<T, string> ruleBuilder)
  {
    return ruleBuilder.NotEmpty().MaximumLength(Domain.Url.MaximumLength).SetValidator(new UrlValidator<T>());
  }

  public static IRuleBuilderOptions<T, string> VolatileCondition<T>(this IRuleBuilder<T, string> ruleBuilder)
  {
    return ruleBuilder.NotEmpty().MaximumLength(Moves.VolatileCondition.MaximumLength);
  }
}
