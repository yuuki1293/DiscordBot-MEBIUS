module DiscordBot_MEBIUS.DataBase.DBConnect

open DiscordBot_MEBIUS.Computation
open DiscordBot_MEBIUS.ReadJson
open MySql.Data.MySqlClient
open DiscordBot_MEBIUS.DataBase.DBType

let connectionString =
    let config = readConfig.Database
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

//TODO: まだできてないよ
let addUserData (mcid: Mcid) =
    use connection = new MySqlConnection(connectionString)

    use userCommand =
        new MySqlCommand($"INSERT INTO user VALUES ('{mcid.User.Discord_id}')", connection)

    use mcidCommand =
        new MySqlCommand($"INSERT INTO mcid VALUES ('{mcid.Mcid}', '{mcid.Uuid}','{mcid.User.Discord_id}')", connection)

    try
        connection.Open()

        userCommand.ExecuteNonQuery() |> ignore
        mcidCommand.ExecuteNonQuery() |> ignore
        Right 0
    with
    | x -> Left x
