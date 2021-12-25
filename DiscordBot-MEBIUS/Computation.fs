module DiscordBot_MEBIUS.Computation

type MaybeBuilder() =
    member _.Return(x) = Some x
    member _.Bind(m:'T option, f) =
        match m with
        | Some x -> f x
        | None -> None

let maybe = MaybeBuilder()

type Either<'T, 'U> =
    | Left of 'T
    | Right of 'U
    static member (>>=)(m, f) =
        match m with
        | Right x -> f x
        | Left x -> Left x

    static member ret x = Right x

type EitherBuilder() =
    member _.Return(x) = Right x
    member _.Bind(m:Either<'T, 'U>, f) = m >>= f

let either = EitherBuilder()