FROM mcr.microsoft.com/dotnet/sdk:8.0

WORKDIR /app

RUN dotnet tool install --global dotnet-ef --version '8.*'

ENV PATH="$PATH:/root/.dotnet/tools"


CMD [ "bash", "-c", "dotnet ef migrations add InitialCreate --context WebAppDatabaseContext --project . --startup-project .; dotnet run --project ." ]

