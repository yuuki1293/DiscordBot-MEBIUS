module DiscordBot_MEBIUS.Commands

open System.Threading.Tasks
open DSharpPlus.CommandsNext
open DSharpPlus.CommandsNext.Attributes

type SendHelloCommand() =
    inherit BaseCommandModule()

    member private this.RespondAsync (ctx: CommandContext) (msg: string) =
        ctx.RespondAsync(msg)
        |> Async.AwaitTask
        |> Async.Ignore

    member private this.Wrap (ctx: CommandContext) (atask: Async<unit>) =
        async {
            try
                do! atask
            with
            | Failure (msg) ->
                eprintfn $"Error: %s{msg}"
                do! this.RespondAsync ctx ("Error: " + msg)
            | err ->
                eprintfn $"Error: %A{err}"
                do! this.RespondAsync ctx "Error: Something goes wrong on our side."
        }
        |> Async.StartAsTask :> Task

    
    [<Command("hoge"); Description("Join the channel")>]
    member public this.hoge(ctx: CommandContext) =
        printf "コマンドを受信"
        async { this.RespondAsync ctx "hogeを受信" |> ignore }
        |> Async.StartAsTask :> Task
