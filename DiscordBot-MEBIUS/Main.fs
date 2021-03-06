module DiscordBot_MEBIUS.Main

open System.Threading.Tasks
open DSharpPlus
open DSharpPlus.CommandsNext
open DiscordBot_MEBIUS.ReadJson
open DiscordBot_MEBIUS.Commands
open Emzi0767.Utilities
open Microsoft.Extensions.Logging
open DiscordBot_MEBIUS.Authentication

let initializeEvent (client:DiscordClient) =
    client.add_MessageCreated(AsyncEventHandler receiveTokenEvent)
    client.add_MessageCreated(AsyncEventHandler Mebius.messageCreatedEvent)

[<EntryPoint>]
let main _ =
    let jsonConfig = appConf
    let conf =
        DiscordConfiguration(
            Token = jsonConfig.Token.ToString().Replace("\"", ""),
            TokenType = TokenType.Bot,
            MinimumLogLevel = LogLevel.Debug,
            Intents = DiscordIntents.All
        )

    let client = new DiscordClient(conf)

    let commandConf =
        CommandsNextConfiguration(EnableMentionPrefix = true, StringPrefixes = [ "!mebius"; "!m" ])

    let commands = client.UseCommandsNext(commandConf)
    commands.RegisterCommands<MainCommand>()
    initializeEvent client

    client.ConnectAsync()
    |> Async.AwaitTask
    |> Async.RunSynchronously

    Task.Delay -1
    |> Async.AwaitTask
    |> Async.RunSynchronously
    
    0
