module DiscordBot_MEBIUS.DataBase.DBConnect

open DiscordBot_MEBIUS.Monad
open DiscordBot_MEBIUS.ReadJson
open MySql.Data.MySqlClient

let ConnectionString =
    let config = ReadConfig.Database
    $"Server={config.Ip};Database={config.Dbname};Uid={config.UserName};Pwd={config.Password}"

let GetDBVersion =
    use connection = new MySqlConnection(ConnectionString)

    use command =
        new MySqlCommand("SELECT version()", connection)

    try
        connection.Open()
        command.ExecuteScalar() |> string |> Right
    with
    | x -> Left x

let GetDBUuidFromToken (token: int) =
    use connection = new MySqlConnection(ConnectionString)

    use command =
        new MySqlCommand($"SELECT uuid FROM token WHERE token={token}", connection)

    try
        connection.Open()

        match command.ExecuteScalar() with
        | null -> Right None
        | x -> Right(Some x)
    with
    | x -> Left x
