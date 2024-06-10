walidacja danych zawsze wywoływana w kontrolerze i tam zwracam BadRequest/NotFound! przed wywołaniem jakichkolwiek transakcji (dodających na przykład) do bazy danych


- dotnet tool install --global dotnet-ef

nuget:
- Microsoft.EntityFrameworkCore.Design (8.0.6 u mnie)
- Microsoft.EntityFrameworkCore.SqlServer (8.0.6)

commands:
- dotnet ef migrations add Init
- dotnet ef database update

1. utwórz modele - folder Models
2. utwórz ApplicationContext - folder Data
