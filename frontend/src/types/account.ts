import type { Locale } from "./i18n";

export type AccountPhone = {
  countryCode?: string;
  number: string;
};

export type CompleteProfilePayload = SaveProfilePayload & {
  token: string;
  password?: string;
  multiFactorAuthenticationMode: MultiFactorAuthenticationMode;
};

export type ContactType = "Email" | "Phone";

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

export type OneTimePasswordPayload = {
  id: string;
  code: string;
};

export type OneTimePasswordValidation = {
  oneTimePasswordId: string;
  sentMessage: SentMessage;
};

export type PersonNameType = "first" | "last" | "middle";

export type SaveProfilePayload = {
  firstName: string;
  middleName?: string;
  lastName: string;
  birthdate?: Date;
  gender?: string;
  locale: string;
  timeZone: string;
};

export type SentMessage = {
  confirmationNumber: string;
  contactType: ContactType;
  maskedContact: string;
};

export type SignInPayload = {
  locale: string;
  credentials?: Credentials;
  authenticationToken?: string;
  oneTimePassword?: OneTimePasswordPayload;
  profile?: CompleteProfilePayload;
};

export type SignInResponse = {
  authenticationLinkSentTo?: SentMessage;
  isPasswordRequired: boolean;
  oneTimePasswordValidation?: OneTimePasswordValidation;
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
