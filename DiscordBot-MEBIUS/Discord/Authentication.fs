module DiscordBot_MEBIUS.Authentication

open System
open System.Threading.Tasks
open DSharpPlus
open System.Linq
open DiscordBot_MEBIUS.DataBase.DBConnect
open DiscordBot_MEBIUS.Monad

let IsMention(e:EventArgs.MessageCreateEventArgs)=
    e.Message.Content.Contains '@'

let ReceiveTokenEvent(client:DiscordClient)(e:EventArgs.MessageCreateEventArgs) =
    task {
        if e.Channel.Name = ReadJson.ReadConfig.Channel
        then
            match e.Message.Content with
            | x when String.forall Char.IsDigit x ->
                printfn $"{x}は10進数値だよ"
                match GetDBUuidFromToken(int x) with
                | Right(Some(x)) ->
                    Some x //TODO: mcid等を取得してDBに書き込む
                | Right(None)-> e.Channel.SendMessageAsync "codeが間違っています" |> ignore
                | Left(x) ->
                    e.Channel.SendMessageAsync "@静的#5318 エラーが発生しました" |> ignore
                    
            | _ when IsMention e ->
                printfn $"{e.Author}はメンションをしたよ"
                e.Guild.BanMemberAsync e.Author.Id |> ignore
            | x -> printfn $"{x}は数値じゃないよ"
        e.Message.DeleteAsync() |> ignore
    }
    :>Task
