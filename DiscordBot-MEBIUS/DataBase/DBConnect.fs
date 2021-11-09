module DiscordBot_MEBIUS.DataBase.DBConnect

open DiscordBot_MEBIUS.ReadJson

let ConnectionString =
    let config = ReadConfig.Database
    $"Server={config.Ip};Database={config.Dbname};Uid={config.UserName};Pwd={config.Password}"