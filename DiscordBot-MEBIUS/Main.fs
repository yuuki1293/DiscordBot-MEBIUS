module DiscordBot_MEBIUS.Main

open System.Threading.Tasks
open DSharpPlus
open DSharpPlus
open DSharpPlus.CommandsNext
open DiscordBot_MEBIUS.ReadJson
open DiscordBot_MEBIUS.Commands

[<EntryPoint>]
let main argv =
    let jsonConfig = ReadConfig
    
    let conf =
        DiscordConfiguration(Token = jsonConfig.Token.ToString().Replace("\"",""), TokenType = TokenType.Bot)

    let client = new DiscordClient(conf)

    let commandConf =
        CommandsNextConfiguration(EnableMentionPrefix = true, StringPrefixes = [ "!mebius" ])

    let commands = client.UseCommandsNext(commandConf)
    commands.RegisterCommands<SendHelloCommand>()

    client.ConnectAsync()
    |> Async.AwaitTask
    |> Async.RunSynchronously

    Task.Delay -1
    |> Async.AwaitTask
    |> Async.RunSynchronously

    0
