module DiscordBot_MEBIUS.Commands

open System.Threading.Tasks
open DSharpPlus.CommandsNext
open DSharpPlus.CommandsNext.Attributes
open DiscordBot_MEBIUS.DataBase.DBConnect
open DiscordBot_MEBIUS.Monad

type MainCommand() =
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
            | Failure msg ->
                eprintfn $"Error: %s{msg}"
                do! this.RespondAsync ctx ("Error: " + msg)
            | err ->
                eprintfn $"Error: %A{err}"
                do! this.RespondAsync ctx "Error: Something goes wrong on our side."
        }|>Async.StartAsTask
        :> Task

    [<Command("ping"); Description("ping pong")>]
    member public this.hoge(ctx: CommandContext) =
        async { this.RespondAsync ctx "pong" |> ignore }
        |> this.Wrap ctx
    
    [<Command("db_version");Aliases("-v"); Description("get mysql version")>]
    member public this.dbVersion(ctx: CommandContext)=
        async {
            match GetDBVersion with
            | Right x -> x
            | Left x -> $"エラー\n{x}"
            |> this.RespondAsync ctx |> ignore
        } |> this.Wrap ctx