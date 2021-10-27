FROM mcr.microsoft.com/dotnet/runtime:5.0
COPY config.json app/config.json
COPY bin/Release/net5.0/publish/ app/
WORKDIR /app
ENTRYPOINT ["dotnet", "DiscordBot-MEBIUS.dll"]