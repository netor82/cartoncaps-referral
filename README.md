# Carton Caps – Referrals Service

## Features:
- Docker build.
- API versioning using path `/api/v1`.
- Swagger API documentation.
	- Examples in [referrals-api.http](./src/CartonCaps.Referrals.Api/referrals-api.http) file.
- Nuget package versions in [Directory.Package.props](./src/Directory.Packages.props).
- 3 layers (API, BLL, DAL).

# Technical Assumptions
(and a little bit of my thought process) 🤓
1. All the requests are authenticated using an JWT token passed in the request or an authentication header.
   - For the scope of this exercise, the `UserId` will be in the request header `X-User-Id` instead.
      - The `IUserContext` implementation will read the `UserId` from the respective header / token claim
        in not-development environments.
1. Referral code is saved in the `User service`, and it's created when the user registers.
   - Note: I'm assuming the same _shareable deferred deep links_ can be shared with multiple peopla
      as the text message or the email can be forwared to multiple destinations.
1. `User service` will be in charge of validating the referral code during user registration and to
    resolve the `UserId` given the referral code.
1. `User service` will call this `Referrals service` to create the referral after a new user registers
    with a valid referral code.
   - This process can be async to decouple the user registration from the referral creation.
1. The `Receipt service` will call this `Referrals service` to complete the referral when a referred
    user registers their first grocery receipt.
   - This process can be async to decouple the receipt registration from the referral creation.
1. Carton Caps app is able to create and read _shareable deferred deep links_, using the third-party service.
   - If the user was referred, the app acts accordingly following the flow in the UI designs.
1. Referrals are completed when the referred user registers their first grocery receipt, to avoid abuse

# Endpoints

## Get current user referrals
Get referrals by user id.

## Get user referrals by user id
Intended as example of communication between services.  This endpoint should not be exposed publicly.

## Create user referral
Intended as example of communication between services.  This endpoint should not be exposed publicly.
To be used by `User service` when a new user registers with a valid referral code.

## Complete user referral
Intended as example of communication between services.  This endpoint should not be exposed publicly.
To be used by `Receipt service` when a referred user registers their first grocery receipt.

# How to run the project

## Visual Studio or other IDE
- .Net 10.0 SDK is required.
- Open the solution file `CartonCaps.Referrals.sln` in Visual Studio or other IDE.
- Run the project `CartonCaps.Referrals.Api` using _https in browser_ launch profile.

## Docker
- Docker is required.
- Build the docker image:
  ```bash
  docker build -t cartoncaps-referrals .
  docker run -d -p 5000:80 --name cartoncaps-referrals cartoncaps-referrals
  ```

# Add a migration
```bash
dotnet ef migrations add --project CartonCaps.Referrals.Data --startup-project CartonCaps.Referrals.Api --context CartonCaps.Referrals.Data.ReferralDbContext <<Name>>
```
 
To do:
- Describe the endpoints
- Use sql lite
- Create mock data
- xUnit - unit tests
- Integration tests
- Instructions to run it using docker

* Mention Async processing of the registration of referrals using a message queue (e.g., RabbitMQ, Azure Service Bus) to decouple the registration process from immediate database writes, improving responsiveness and scalability.