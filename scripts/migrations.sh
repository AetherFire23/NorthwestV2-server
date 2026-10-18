#!/bin/bash

# Create an EF Core migration with a random GUID name

PROJECT_PATH="../src/Core"

dotnet ef migrations add "initial $(uuidgen)" --project "$PROJECT_PATH"
