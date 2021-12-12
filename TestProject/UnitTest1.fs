module TestProject

open NUnit.Framework
open DiscordBot_MEBIUS.DataBase.DBConnect
open DiscordBot_MEBIUS.Computation

[<SetUp>]
let Setup () =
    ()

[<Test>]
let Test1 () =
    match getDbVersion with
    | Right _ -> Assert.Pass()
    | Left x ->
        printfn $"{x}"
        Assert.Fail()
        
[<Test>]
let GetTokenTest ()=
    match getDBUuidFromToken 432 with
    |Right x->
        match x with
        | Some x ->printfn $"{x}"
        | None -> printfn "None"
        Assert.Pass()
    |Left x ->
        printfn $"{x}"
        Assert.Fail()
        