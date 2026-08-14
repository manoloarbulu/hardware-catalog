@workspace /agents/backend/dotnet-architect.md /skills/backend/cqrs-mediatr.md

I need to create a new CRUD feature in the backend for a new entity called `[ENTITY_NAME]`.

Please generate the following files:

1. The Domain Entity in `Domain/Entities`.
2. The MediatR Commands (Create, Update, Delete) and their Handlers in `Application/Features/[ENTITY_NAME]/Commands`.
3. The MediatR Queries (GetAll, GetById) and their Handlers in `Application/Features/[ENTITY_NAME]/Queries`.
4. The FluentValidation rules for the Create and Update commands.
5. The API Controller in `WebApi/Controllers` exposing these endpoints.

Ensure the code adheres exactly to the Clean Architecture rules defined in our context.
