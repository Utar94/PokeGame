﻿namespace PokeGame.Web.Models.Api.Account
{
  public class SignInModel
  {
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public bool Remember { get; set; }
  }
}
