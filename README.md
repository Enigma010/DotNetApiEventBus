# EventBus
Library for interacting with the event bus.  The event bus is used to communicate entity state changes to other microservices in the system.

## Guidelines
The following describes guidelines for the architecture.

* The event bus is a shared component between all microservices and as such long term should be moved into its own nuget package.

## Running the EventBus
The event bus uses RabbitMQ.  For development open up a **wsl** command prompt and change directories to `EventBus`.  Then run the command:

```
./lrun.sh
```

The RabbitMq docker container has an management plugin installed in it to support admin and monitoring activities.  To access the page go to:

```
http://localhost:15672/
```