dotnet restore src/GiraffeAngularTemplate
dotnet build src/GiraffeAngularTemplate

dotnet restore tests/GiraffeAngularTemplate.Tests
dotnet build tests/GiraffeAngularTemplate.Tests
dotnet test tests/GiraffeAngularTemplate.Tests
