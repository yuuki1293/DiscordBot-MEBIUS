module DiscordBot_MEBIUS.DataBase.DBConnect

open DiscordBot_MEBIUS
open DiscordBot_MEBIUS.Computation
open DiscordBot_MEBIUS.ReadJson
open MySql.Data.MySqlClient
open MojangConnect

let connectionString =
    let config = appConf.Database
    $"Server={config.Ip};Database={config.Dbname};Uid={config.UserName};Pwd={config.Password}"

let connection = new MySqlConnection(connectionString)

let getDbVersion =
    use command =
        new MySqlCommand("SELECT version()", connection)

    try
        connection.Open()
        command.ExecuteScalar() |> string |> Right
    with
    | x -> Left x

let getDBUuidFromToken (token: int) =
    use command =
        new MySqlCommand($"SELECT uuid FROM token WHERE token={token}", connection)

    try
        connection.Open()

        match command.ExecuteScalar() with
        | null -> Right None
        | x -> Right(Some(x |> string))
    with
    | x -> Left x

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
    let mcid = getMcidFromUuid uuid
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

            Right None
        else
            Right(Some "そのmcidはすでに登録されています")
    with
    | x -> Left x

let getDBMebiusIDs (discordId: uint64) =
    use getIDsCommand =
        new MySqlCommand($"SELECT mebius_id FROM MEBIUS_data WHERE user_discord_id = {discordId}")

    try
        use reader = getIDsCommand.ExecuteReader()

        seq {
            while reader.Read() do
                yield reader.GetInt32(0)
        }
        |> Right
    with
    | x -> Left x
