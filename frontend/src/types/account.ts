import type { Locale } from "./i18n";

export type AccountPhone = {
  countryCode?: string;
  number: string;
};

export type Credentials = {
  emailAddress: string;
  password?: string;
};

export type CurrentUser = {
  displayName: string;
  emailAddress?: string;
  pictureUrl?: string;
};

export type MultiFactorAuthenticationMode = "None" | "Email" | "Phone";

export type PersonNameType = "first" | "last" | "middle";

export type SignInPayload = {
  locale: string;
  credentials?: Credentials;
  authenticationToken?: string;
  // TODO(fpion): OneTimePassword
  // TODO(fpion): Profile
};

export type SignInResponse = {
  // TODO(fpion): AuthenticationLinkSentTo
  isPasswordRequired: boolean;
  // TODO(fpion): OneTimePasswordValidation
  profileCompletionToken?: string;
  currentUser?: CurrentUser;
};

export type UserProfile = {
  createdOn: string;
  completedOn: string;
  updatedOn: string;
  passwordChangedOn?: string;
  authenticatedOn?: string;
  multiFactorAuthenticationMode: MultiFactorAuthenticationMode;
  emailAddress: string;
  phone?: AccountPhone;
  firstName: string;
  middleName?: string;
  lastName: string;
  fullName: string;
  birthdate?: string;
  gender?: string;
  locale: Locale;
  timeZone: string;
};
