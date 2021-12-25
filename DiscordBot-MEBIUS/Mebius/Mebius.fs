module DiscordBot_MEBIUS.Mebius.Mebius

open DSharpPlus.Entities
open DiscordBot_MEBIUS.DataBase.DBConnect
open DiscordBot_MEBIUS.Computation

type MebiusId = MebiusId of id: int

type Mebius =
    { id: MebiusId }
    member this.LevelUp() = this.id

let getMebiusIDs (discordUser: DiscordUser) =
    either {
        let! ids = getDBMebiusIDs discordUser

        return
            ids
            |> Seq.map (fun x -> { id = MebiusId x })
            |> Seq.toList
    }
