module TestProject

open NUnit.Framework
open DiscordBot_MEBIUS.DataBase.DBConnect
open DiscordBot_MEBIUS.Monad
open NUnit.Framework
open NUnit.Framework

[<SetUp>]
let Setup () =
    ()

[<Test>]
let Test1 () =
    match GetDBVersion with
    | Right _ -> Assert.Pass()
    | Left x ->
        printfn $"{x}"
        Assert.Fail()
        
[<Test>]
let GetTokenTest ()=
    match GetDBuuidFromToken 40725 with
    |Right x->
        printfn $"{x.Value}"
        Assert.Pass()
    |Left x ->
        printfn $"{x}"
        Assert.Fail()