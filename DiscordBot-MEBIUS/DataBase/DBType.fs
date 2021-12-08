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
        Enchant:Enchant
    }
type Mcid=
    {
        Mcid:string
        Uuid:string
    }
type User=
    {
        Discord_id:uint64
        Mebius_count:int
        Mcid:Mcid
        MEBIUS_data:MEBIUS_data
    }