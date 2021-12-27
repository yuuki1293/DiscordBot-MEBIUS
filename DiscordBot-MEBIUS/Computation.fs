module DiscordBot_MEBIUS.Computation

type MaybeBuilder() =
    member _.Return(x) = Some x
    member _.Bind(m:'T option, f) =
        match m with
        | Some x -> f x
        | None -> None

let maybe = MaybeBuilder()

type ResultBuilder() =
    member _.Return(x) = Ok x
    member _.Bind(m, f) =
        match m with
        | Ok x -> f x
        | Error x -> Error x
    
let result = ResultBuilder()