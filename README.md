# EventBus
Library for interacting with the event bus.  The event bus is used to communicate entity state changes to other microservices in the system.

## Guidelines
The following describes guidelines for the architecture.

* The event bus is a shared component between all microservices and as such long term should be moved into its own nuget package.
