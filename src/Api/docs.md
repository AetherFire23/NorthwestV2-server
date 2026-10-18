1. Watch out for mediator registering as singleton and takign in a Context.
as a parameter : it will be a lifestyle mismatch.

It will work in tests because tests work in a single scope. 

2. Watch out for the connection string. Put it at top-level.
The installer expects a specific connection string key :
databasenamelowercaseConnectionString 