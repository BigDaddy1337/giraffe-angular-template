#!/bin/sh
dotnet restore src
dotnet build src

dotnet restore tests
dotnet build tests
dotnet test tests
