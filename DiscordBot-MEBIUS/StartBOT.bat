docker-compose up -d
dotnet publish -c Release
docker build -t mebius-bot-image -f Dockerfile .
docker run -it --rm mebius-bot-image