module DiscordBot_MEBIUS.ReadJson

open System.Text.Json
open FSharp.Data

type ConfigType = JsonProvider<"./config.json">