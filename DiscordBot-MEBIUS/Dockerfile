FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["DiscordBot-MEBIUS/DiscordBot-MEBIUS.fsproj", "DiscordBot-MEBIUS/"]
RUN dotnet restore "DiscordBot-MEBIUS/DiscordBot-MEBIUS.fsproj"
COPY . .
WORKDIR "/src/DiscordBot-MEBIUS"
RUN dotnet build "DiscordBot-MEBIUS.fsproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DiscordBot-MEBIUS.fsproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DiscordBot-MEBIUS.dll"]
