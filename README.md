# Nexaas.Id.Client

This is Dot Net client for the [Nexaas ID](https://id.nexaas.com) API.

# Configuration
### Create a new application
Go to https://id.nexaas.com/applications and create a new application in your Nexaas ID account.

### Setup your environment

```c#

var nexaasId = NexaasID.Production(
  "your client id", 
  "your client secret", 
  "your application callback uri");

```

### Get Nexaas ID login url

```c#

var url = nexaasId.GetAuthorizeUrl();

```

### Get Nexaas ID authorization token

```c#

BaseResponse<AuthTokenResponse> response = await nexaasId.GetAuthorizationToken(code);

```

### Get Profile info

```c#

BaseResponse<Profile> response = await nexaasId.GetProfile(accessToken);

```

### Get Profile professional info

```c#

BaseResponse<ProfessionalInfo> response = await nexaasId.GetProfessionalInfo(accessToken);

```

### Get Profile contacts

```c#

BaseResponse<Contacts> response = await nexaasId.GetContacts(accessToken);

```

### Get Profile emails

```c#

BaseResponse<Emails> response = await nexaasId.GetEmails(accessToken);

```
