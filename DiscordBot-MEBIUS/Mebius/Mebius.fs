module DiscordBot_MEBIUS.Mebius.Mebius

open DiscordBot_MEBIUS.DataBase.DBConnect
open DiscordBot_MEBIUS.Computation

type MebiusId = MebiusId of id: int

type Mebius =
    {id:MebiusId}
    member this.LevelUp() =
        id

let getMebiusIDs (discordId: uint64) =
    result {
        let! ids = getDBMebiusIDs discordId

        return
            ids
            |> Seq.map (fun x -> { id = MebiusId x })
            |> Seq.toList
    }

let mebiusLevelUp(mebius :Mebius)=
    getMebiusIDs