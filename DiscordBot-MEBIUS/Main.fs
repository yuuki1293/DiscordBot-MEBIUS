module DiscordBot_MEBIUS.Main

open System
open System.Threading.Tasks
open DSharpPlus
open DSharpPlus.CommandsNext
open DiscordBot_MEBIUS.ReadJson
open DiscordBot_MEBIUS.Commands
open Microsoft.Extensions.Logging

[<EntryPoint>]
let main _ =
    let jsonConfig = ReadConfig
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
    commands.RegisterCommands<SendHelloCommand>()

    client.ConnectAsync()
    |> Async.AwaitTask
    |> Async.RunSynchronously

    Task.Delay -1
    |> Async.AwaitTask
    |> Async.RunSynchronously

    0
