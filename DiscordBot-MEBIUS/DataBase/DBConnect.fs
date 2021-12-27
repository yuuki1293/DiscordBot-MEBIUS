module DiscordBot_MEBIUS.DataBase.DBConnect

open DiscordBot_MEBIUS
open DiscordBot_MEBIUS.ReadJson
open MySql.Data.MySqlClient
open MojangConnect
open FSharp.Core.CompilerServices

let connectionString =
    let config = appConf.Database
    $"Server={config.Ip};Database={config.Dbname};Uid={config.UserName};Pwd={config.Password}"

let connection = new MySqlConnection(connectionString)

let getDbVersion =
    use command =
        new MySqlCommand("SELECT version()", connection)

    try
        connection.Open()
        command.ExecuteScalar() |> string |> Ok
    with
    | x -> Error x

let getDBUuidFromToken (token: int) =
    use command =
        new MySqlCommand($"SELECT uuid FROM token WHERE token={token}", connection)

    try
        connection.Open()

        match command.ExecuteScalar() with
        | null -> Ok None
        | x -> Ok(Some(x |> string))
    with
    | x -> Error x

let IsUuidDuplicate (uuid: string) (connection: MySqlConnection) =
    use uuidDuplicateCheckCommand =
        new MySqlCommand($"SELECT * FROM mcid WHERE uuid={uuid}", connection)

    try
        match uuidDuplicateCheckCommand.ExecuteScalar() with
        | x when (isNull x) -> Some false
        | _ -> Some true
    with
    | x ->
        printfn $"{x.Message}"
        None

let addUserData (uuid: string)(discordId:uint64) =
    let mcidResult = (getMcidFromUuid uuid).Result
    match mcidResult with
    | Ok mcid ->
        use userCommand =
            new MySqlCommand($"INSERT INTO user VALUES ('{discordId}')", connection)

        use userExist =
            new MySqlCommand($"SElECT * FROM user WHERE discord_id = {discordId}", connection)

        use mcidCommand =
            new MySqlCommand($"INSERT INTO mcid VALUES ('{mcid}', '{uuid}','{discordId}')", connection)

        use mcidExist =
            new MySqlCommand($"SELECT * FROM mcid WHERE mcid = {mcid}")

        try
            connection.Open()

            if mcidExist.ExecuteReader().FieldCount > 0 then
                mcidCommand.ExecuteNonQuery() |> ignore

                if userExist.ExecuteReader().FieldCount > 0 then
                    userCommand.ExecuteNonQuery() |> ignore

                Ok None
            else
                Ok(Some "そのmcidはすでに登録されています")
        with
        | x -> Error x.Message
    | Error (_, msg)->Error msg
        

let getDBMebiusIDs (discordId: uint64) =
    use getIDsCommand =
        new MySqlCommand($"SELECT mebius_id FROM MEBIUS_data WHERE user_discord_id = {discordId}")

    try
        use reader = getIDsCommand.ExecuteReader()
        
        let mutable col = ListCollector<int>()
        
        while reader.Read() do
            col.Add(reader.GetInt32 0)
        
        col.Close()
        |> Ok
    with
    | x -> Error x
