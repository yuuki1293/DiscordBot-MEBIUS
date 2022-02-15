module DiscordBot_MEBIUS.Commands

open System.Threading.Tasks
open DSharpPlus.CommandsNext
open DSharpPlus.CommandsNext.Attributes
open DiscordBot_MEBIUS.DataBase.DBConnect
open DiscordBot_MEBIUS.Mebius

type MainCommand() =
    inherit BaseCommandModule()

    member private this.RespondAsync (ctx: CommandContext) (msg: string) =
        ctx.RespondAsync(msg)
        |> Async.AwaitTask
        |> Async.Ignore

    member private this.Wrap (ctx: CommandContext) (aTask: Async<unit>) =
        task {
            try
                do! aTask
            with
            | Failure msg ->
                eprintfn $"Error: %s{msg}"
                do! this.RespondAsync ctx ("Error: " + msg)
            | err ->
                eprintfn $"Error: %A{err}"
                do! this.RespondAsync ctx "Error: Something goes wrong on our side."
        }
        :> Task
    
    [<Command("db_version");Aliases("-v"); Description("get mysql version")>]
    member public this.dbVersion(ctx: CommandContext)=
        async {
            do!
                match getDbVersion with
                | Ok x -> x
                | Error x -> $"エラー\n{x}"
                |> this.RespondAsync ctx
        } |> this.Wrap ctx