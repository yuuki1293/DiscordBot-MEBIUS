module DiscordBot_MEBIUS.DataBase.DBType

type Enchant =
    {
        Durability:int
        Protection:int
        Mending:int
        Fire_protection:int
        Projectile_protection:int
        Explosion_protection:int
        Respiration:int
        Water_affinity:int
        Unbreakable:bool
    }
type MEBIUS_data=
    {
        Mebius_id:int
        Name:string
        Durability:int
        Level:int
    }
type User=
    {
        Discord_id:string
        Mebius_count:int
    }
type DBType=
    {
        Enchant:Enchant
        MEBIUS_data:MEBIUS_data
        User:User
    }