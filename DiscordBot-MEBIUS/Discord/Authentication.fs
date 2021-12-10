module DiscordBot_MEBIUS.Authentication

open System
open System.IO
open System.Net
open System.Net.Http
open System.Threading.Tasks
open DSharpPlus
open System.Linq
open DiscordBot_MEBIUS.DataBase.DBConnect
open DiscordBot_MEBIUS.DataBase.DBType
open DiscordBot_MEBIUS.Monad
open Hopac

let IsMention(e:EventArgs.MessageCreateEventArgs)=
    e.Message.Content.Contains '@'

let MessageDeleteAsyncTime(time:int)(sent:Task<Entities.DiscordMessage>)=
    task{
        do! Task.Delay time 
        do! sent.Result.DeleteAsync()
    }
let ReceiveTokenEvent(client:DiscordClient)(e:EventArgs.MessageCreateEventArgs) =
    task {
        if e.Author.IsBot then 0 |> ignore else
        if e.Channel.Name = ReadJson.ReadConfig.Channel
        then
            match e.Message.Content with
            | code when String.forall Char.IsDigit code ->
                printfn $"{code}は10進数値だよ"
                match GetDBUuidFromToken(int code) with
                | Right(Some(uuid)) ->
                    use client = new HttpClient()
                    let! response = client.GetAsync $"https://api.mojang.com/user/profiles/{uuid}/names"
                    match response.StatusCode with
                    | HttpStatusCode.NoContent ->
                        do! e.Message.RespondAsync "minecraftアカウントが存在しません。"
                            |> MessageDeleteAsyncTime 3000
                    | HttpStatusCode.NotFound ->
                        do! e.Channel.SendMessageAsync "@静的#5318 Mojang APIのサーバーが落ちている可能性が高いです。しばらく待ってからやり直してください。\nおのれもやん"
                            |>MessageDeleteAsyncTime 3000
                    | HttpStatusCode.OK ->
                        use reader = new StreamReader(response.Content.ReadAsStream())
                        let mcid = reader.ReadLine().Replace("[{\"name\":","").Trim([|'}';']';'"'|])
                        AddUserData({Mcid=mcid;Uuid=uuid;User={Discord_id=e.Author.Id;Mebius_count=0}})
                        //TODO: DBに書き込む
                    | _ ->
                        do! e.Channel.SendMessageAsync "@静的#5318 予期しないエラーが発生しました"
                            |>MessageDeleteAsyncTime 3000
                | Right(None)->
                    do! e.Channel.SendMessageAsync "codeが間違っています"
                        |>MessageDeleteAsyncTime 3000
                | Left(x) ->
                    do! e.Channel.SendMessageAsync "@静的#5318 エラーが発生しました"
                        |>MessageDeleteAsyncTime 3000
                    
            | _ when IsMention e ->
                printfn $"{e.Author}はメンションをしたよ"
                do! e.Guild.BanMemberAsync e.Author.Id
            | _ -> do! e.Channel.SendMessageAsync "codeが間違っています"
                        |>MessageDeleteAsyncTime 3000
        do! e.Message.DeleteAsync()
    }
    :>Task
