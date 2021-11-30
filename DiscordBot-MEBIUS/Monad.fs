module DiscordBot_MEBIUS.Monad


let (>>=) m f =
    match m with
    | Some x -> f x
    | None -> None

let ret x = Some x

type MaybeBuilder() =
    member _.Return(x) = ret x
    member _.Bind(m, f) = m >>= f

let maybe = MaybeBuilder()

type Either<'T, 'U> =
    | Right of 'T
    | Left of 'U
    static member (>>=)(m, f) =
        match m with
        | Right x -> f x
        | Left _ -> m

    static member ret x = Right x

type EitherBuilder() =
    member _.Return(x) = ret x
    member _.Bind(m, f) = m >>= f

let either = EitherBuilder()