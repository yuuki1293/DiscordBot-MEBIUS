module DiscordBot_MEBIUS.ReadJson

open System.IO
open System.Text.Json
open FSharp.Data

type ConfigType = JsonProvider<"./config.json">

let ReadConfig =
    File.ReadAllText("config.json")
    |> ConfigType.Parse
