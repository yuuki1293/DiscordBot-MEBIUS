module DiscordBot_MEBIUS.MojangConnect

open System.Net
open System.Net.Http
open FSharp.Data
open System.Linq
open DiscordBot_MEBIUS.Computation

type UserProfilesUuidNamesType = JsonProvider<"{\"name\":\"mcid\",\"changedToAt\":\"0\"}">

let readMcidList list =
    UserProfilesUuidNamesType.ParseList list

let getMcidFromUuid (uuid: string) =
    task {
        use httpClient = new HttpClient()
        let! response = httpClient.GetAsync $"https://api.mojang.com/user/profiles/{uuid}/names"

        match response.StatusCode with
        | HttpStatusCode.OK ->
            let! content = response.Content.ReadAsStringAsync()
            let mcidList = readMcidList content
            return Right(mcidList.Last().Name)
        | HttpStatusCode.NoContent -> return Left(HttpStatusCode.NoContent, "存在しないアカウント")
        | HttpStatusCode.NotFound -> return Left(HttpStatusCode.NotFound, "api.mojang.comに接続できません")
        | x when int x >= 400 && int x < 500 -> return Left(x, "Mojang APIとの通信中に何らかのエラーが発生しました")
        | x when int x >= 500 -> return Left(x, "Mojang APIのサーバーが正常に作動していません")
        | x -> return Left(x, "予期しないリクエスト")
    }
