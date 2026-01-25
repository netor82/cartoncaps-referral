
Features included:
- docker build
- API versioning
- Swagger API documentation
	- http examples in `referrals-api.http` file
- Nuget package version in Directory.Package.props
- 3 layers (API, BLL, DAL)


To do:
- Describe the endpoints
- Use sql lite
- Create mock data
- xUnit - unit tests
- Integration tests
- Instructions to run it using docker

* Mention Async processing of the registration of referrals using a message queue (e.g., RabbitMQ, Azure Service Bus) to decouple the registration process from immediate database writes, improving responsiveness and scalability.