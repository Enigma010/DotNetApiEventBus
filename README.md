# EventBus
Library for interacting with the event bus.  The event bus is used to communicate entity state changes to other microservices in the system.

## Guidelines
The following describes guidelines for the architecture.

* The event bus is a shared component between all microservices and as such long term should be moved into its own nuget package.

## End to End Testing
The testing of the event bus is done through end to end testing, that is to say the tests use four main components to test the frame work:

* The tests
* A REST API that listens to **EventOne** events and can be queried about those events
* A REST API that listens to **EventTwo** events and can be queried about those events
* The event bus infrastructure (**RabbitMQ**)

The unit tests start the REST APIs and the event infrastructure when they start.  Once those are started a standard test process will do the following:

* The test will query the REST API about a specific event by ID and the expectation is that the REST API will return a not found result.
* The test will emit an event through the event bus.
* The test will query the REST API about the specific event by ID and the expectation is that the REST API will find the event being requested.

There are some issues with the current implementation that need to be manually handled.

1. Docker needs to be running prior to any tests running.
1. The solution needs to be compiled in release mode prior to attempting to run any of the unit tests
1. If the REST APIs are already running then the unit tests will not be able to start new versions of them when they start and you might need to manually stop the processes.  You are looking for processes running the following commands:

```
dotnet run --project ..DotNetApiEventBus.Tests.EndToEnd.Api --configuration Release --no-build
```

```
dotnet run --project ..DotNetApiEventBus.Tests.EndToEnd.Api2 --configuration Release --no-build
```             