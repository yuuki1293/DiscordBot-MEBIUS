module DiscordBot_MEBIUS.ReadJson

open System.IO
open FSharp.Data

type ConfigType = JsonProvider<"config.json">

let appConf =
    File.ReadAllText("config.json")
    |> ConfigType.Parse
