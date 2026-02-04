# Carton Caps – Referrals Service

## Features:
- Docker build.
- API versioning using path `/api/v1`.
- Swagger API documentation (see _How to run the project_ section below) .
	- Examples in [referrals-api.http](./src/CartonCaps.Referrals.Api/referrals-api.http) file.
- Nuget package versions in [Directory.Package.props](./src/Directory.Packages.props).
- 3 layers (API, BLL, DAL).
- Uses Sqlite.

# Assumptions
(and a little bit of my thought process) 🤓
1. All the requests are authenticated using an JWT token passed in the request or an authentication header.
   - For the scope of this exercise, the `UserId` will be in the request header `X-User-Id` instead.
1. There are other services like `User sevice` and `Receipt serice`, that are out of the scope of this exercise.
1. Referral code is saved in the `User service`, and it's created when the user registers.
   - Note: I'm assuming the same _shareable deferred deep links_ can be shared with multiple people
      as the text message or the email can be forwarded to multiple destinations.
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
1. Referrals are completed when the referred user registers their first grocery receipt, to avoid abuse.
1. Datetime objects are transmitted in UTC.
1. The API is behind a gateway, prohibiting direct public access to it.
   - Some endpoints are intended for internal communication between services only and should not be
     exposed publicly.

# Endpoints

## 1. Get current user referrals
Get referrals by user id.

## 2. Get user referrals by user id
Intended as example of communication between services.  This endpoint should not be exposed publicly.

## 3. Create user referral
Intended as example of communication between services.  This endpoint should not be exposed publicly.
To be used by `User service` when a new user registers with a valid referral code.

## 4. Complete user referral
Intended for communication between services.  This endpoint should not be exposed publicly.
To be used by `Receipt service` when a referred user registers their first grocery receipt.

# How to run the project

## Visual Studio or other IDE
- .Net 10.0 SDK is required.
- Open the solution file `CartonCaps.Referrals.sln` in Visual Studio or other IDE.
- Run the project `CartonCaps.Referrals.Api` using _https in browser_ launch profile.

## Docker
- Docker is required.
- Build the docker image from the root of the repository:
  ```bash
  docker build -t cartoncaps-referrals .
  docker run -it --rm -p 5097:8080 --name cartoncaps-referrals cartoncaps-referrals
  ```
- Now you can use [referrals-api.http](./src/CartonCaps.Referrals.Api/referrals-api.http) file to test the API.

> The data will be deleted because the container is removed after stopping it (--rm option).


# Add a migration
```bash
dotnet ef migrations add --project CartonCaps.Referrals.Data --startup-project CartonCaps.Referrals.Api --context CartonCaps.Referrals.Data.ReferralDbContext <<Name>>
```
