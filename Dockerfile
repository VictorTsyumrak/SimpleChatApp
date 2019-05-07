FROM mcr.microsoft.com/dotnet/core/sdk AS build
WORKDIR /app

COPY . .
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet

WORKDIR /app
COPY --from=build /app/SimpleChatApp/out .
ENTRYPOINT ["dotnet", "SimpleChatApp.dll"]