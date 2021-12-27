module DiscordBot_MEBIUS.Authentication

open System
open System.Threading.Tasks
open DSharpPlus
open DSharpPlus.Entities
open DiscordBot_MEBIUS.DataBase.DBConnect
open DiscordBot_MEBIUS.Computation
open DiscordBot_MEBIUS.ReadJson

/// <summary>discordのメッセージにメンションが含まれているか調べる</summary>
/// <param name="e">the input DiscordMessage</param>
/// <returns>true or false</returns>
let isMention (e: DiscordMessage) = e.Content.Contains '@'

/// <summary>configで設定したdiscord idのメンションを含んだメッセージを送る</summary>
let mentionOwnerAsync (msg: string)(client: DiscordClient)(e:EventArgs.MessageCreateEventArgs) =
    e.Channel.SendMessageAsync(
        DiscordMessageBuilder()
            .WithContent(
                $"{client
                       .GetUserAsync(
                           appConf.OwnerId |> uint64
                       )
                       .Result
                       .Mention}{msg}"
            )
            .WithAllowedMention(UserMention(appConf.OwnerId |> uint64))
    )

let messageDeleteAsyncTime (time: int) (sent: Task<DiscordMessage>) =
    task {
        do! Task.Delay time
        do! sent.Result.DeleteAsync()
    }

let receiveTokenEvent (client: DiscordClient) (e: EventArgs.MessageCreateEventArgs) =
    task {
        if e.Author.IsBot then
            0 |> ignore
        else
            if e.Channel.Name = appConf.Channel then
                match e.Message.Content with
                | code when String.forall Char.IsDigit code ->
                    printfn $"{code}は10進数値だよ"

                    match getDBUuidFromToken (int code) with
                    | Ok (Some uuid) ->
                        match addUserData uuid e.Author.Id with
                        | Ok (Some x) -> do! e.Channel.SendMessageAsync x |> messageDeleteAsyncTime 3000
                        | Ok None ->
                            //TODO: 認証ロールをつける
                            ()
                        | Error x-> do! mentionOwnerAsync x client e |> messageDeleteAsyncTime 3000 
                    | Ok None ->
                        do!
                            e.Channel.SendMessageAsync "codeが間違っています"
                            |> messageDeleteAsyncTime 3000
                    | Error _ ->
                        do!
                            mentionOwnerAsync "エラーが発生しました" client e
                            |> messageDeleteAsyncTime 3000

                | _ when isMention e.Message ->
                    printfn $"{e.Author}はメンションをしたよ"
                    do! e.Guild.BanMemberAsync e.Author.Id
                | _ ->
                    do!
                        e.Channel.SendMessageAsync "codeが間違っています"
                        |> messageDeleteAsyncTime 3000

            do! e.Message.DeleteAsync()
    }
    :> Task
