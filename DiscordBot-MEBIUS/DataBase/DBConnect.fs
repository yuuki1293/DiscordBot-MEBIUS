module DiscordBot_MEBIUS.DataBase.DBConnect

open DiscordBot_MEBIUS.Computation
open DiscordBot_MEBIUS.ReadJson
open MySql.Data.MySqlClient
open DiscordBot_MEBIUS.DataBase.DBType

let connectionString =
    let config = appConf.Database
    $"Server={config.Ip};Database={config.Dbname};Uid={config.UserName};Pwd={config.Password}"

let getDbVersion =
    use connection = new MySqlConnection(connectionString)

    use command =
        new MySqlCommand("SELECT version()", connection)

    try
        connection.Open()
        command.ExecuteScalar() |> string |> Right
    with
    | x -> Left x

let getDBUuidFromToken (token: int) =
    use connection = new MySqlConnection(connectionString)

    use command =
        new MySqlCommand($"SELECT uuid FROM token WHERE token={token}", connection)

    try
        connection.Open()

        match command.ExecuteScalar() with
        | null -> Right None
        | x -> Right(Some(x |> string))
    with
    | x -> Left x

let IsUuidDuplicate (uuid:string)(connection:MySqlConnection)=
    use uuidDuplicateCheckCommand =
        new MySqlCommand($"SELECT * FROM mcid WHERE uuid={uuid}",connection)
    try
        match uuidDuplicateCheckCommand.ExecuteScalar() with
        | x when (isNull x) -> Some false
        | _ -> Some true
    with
    | x ->
        printfn $"{x.Message}"
        None

//TODO: まだできてないよ
let addUserData (mcid: Mcid) =
    use connection = new MySqlConnection(connectionString)
    
    use userCommand =
        new MySqlCommand($"INSERT INTO user VALUES ('{mcid.User.Discord_id}')", connection)
    
    use userExist =
        new MySqlCommand($"SElECT * FROM user WHERE discord_id = {mcid.User.Discord_id}",connection)
    
    use mcidCommand =
        new MySqlCommand($"INSERT INTO mcid VALUES ('{mcid.Mcid}', '{mcid.Uuid}','{mcid.User.Discord_id}')", connection)
    
    use mcidExist =
        new MySqlCommand($"SELECT * FROM mcid WHERE mcid = {mcid.Mcid}")
    
    try
        connection.Open()
        
        if mcidExist.ExecuteReader().FieldCount > 0 then
            mcidCommand.ExecuteNonQuery() |> ignore
            if userExist.ExecuteReader().FieldCount > 0 then
                userCommand.ExecuteNonQuery() |> ignore
            Right None
        else Right (Some "そのmcidはすでに登録されています")
    with
    | :? System.InvalidOperationException as x -> Left x.InnerException
    | x -> Left x
