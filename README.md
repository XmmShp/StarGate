# StarGate

A library designed to facilitate the use of the Publish/Subscribe pattern and the Chain of Responsibility pattern within .NET applications.

## What are the differences between this library and using native events?

- Provides a highly encapsulated event bus called `StarGate`.
- Offers a highly encapsulated chain of responsibility called `StarBelt`.
- Supports subscribing to events across different Assemblies.
- Enables asynchronous execution of callback functions.
- Allows for listening to different stages of the same event.
- Includes support for interrupting events.
- Enables capturing return values from multiple events.
