module DiscordBot_MEBIUS.Monad

let (>>=) (m: Option<'T>) f =
    match m with
    | Some x -> f x
    | None -> None

let ret x = Some x

type MaybeBuilder() =
    member _.Return(x) = ret x
    member _.Bind(m, f) = m >>= f

let maybe = new MaybeBuilder()
