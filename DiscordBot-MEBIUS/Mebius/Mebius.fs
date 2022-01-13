module DiscordBot_MEBIUS.Mebius.Mebius

open DiscordBot_MEBIUS.DataBase.DBConnect
open DiscordBot_MEBIUS.Computation

type MebiusId = MebiusId of id: int

type Mebius =
    {id:int}
