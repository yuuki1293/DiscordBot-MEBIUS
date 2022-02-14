module DiscordBot_MEBIUS.Mebius

open System
open System.Threading.Tasks
open DSharpPlus
open DSharpPlus.EventArgs

type MebiusEnchantment(unlockLevel: int, maxLevel: int, displayName: string, dbName: string) =
    member val unlockLevel = unlockLevel with get
    member val maxLevel = maxLevel
    member val displayName = displayName
    member val dbName = dbName

type MebiusEnchantments =
    | Protection of level: int
    | Durability of level: int
    | Mending of level: int
    | FireProtection of level: int
    | ProjectileProtection of level: int
    | ExplosionProtection of level: int
    | Respiration of level: int
    | WaterAffinity of level: int
    | Unbreakable of level: int
    member this.Enchant=
        match this with
        | Protection _ -> MebiusEnchantment(2, 10, "ダメージ軽減","protection")
        | Durability _ -> MebiusEnchantment(1, 10, "耐久力","durability")
        | Mending _ -> MebiusEnchantment(1, 1, "修繕","mending")
        | FireProtection _ -> MebiusEnchantment(6, 10, "火炎耐性","fire_protection")
        | ProjectileProtection _ -> MebiusEnchantment(6, 10, "飛び道具耐性","projectile_protection")
        | ExplosionProtection _ -> MebiusEnchantment(6, 10, "爆発耐性","explosion_protection")
        | Respiration _ -> MebiusEnchantment(15, 3, "水中呼吸","respiration")
        | WaterAffinity _ -> MebiusEnchantment(15, 1, "水中採掘","water_affinity")
        | Unbreakable _ -> MebiusEnchantment(30, 1, "耐久無限","unbreakable")
    static member itr = [Protection 1;Durability 1;Mending 1;FireProtection 1;ProjectileProtection 1;ExplosionProtection 1;Respiration 1;WaterAffinity 1;Unbreakable 1]
    member this.Inc() =
        match this with
        | Protection x -> Protection (x+1)
        | Durability x -> Durability (x+1)
        | Mending x -> Mending (x+1)
        | FireProtection x -> FireProtection (x+1)
        | ProjectileProtection x -> ProjectileProtection (x+1)
        | ExplosionProtection x -> ExplosionProtection (x+1)
        | Respiration x -> Respiration (x+1)
        | WaterAffinity x -> WaterAffinity (x+1)
        | Unbreakable x -> Unbreakable (x+1)
    
type MebiusId = MebiusId of id: int

type Mebius =
    { id: int
      level: int
      enchants: MebiusEnchantments list
      rand: Random}
    static member New id = {id=id;level=0;enchants=[Durability 3];rand=Random(id)}
    member this.isMaxLevel() = this.level >= 30 

let strToSeed (str:string) =
    let bytes = System.Text.Encoding.UTF32.GetBytes(str)
    bytes
    |> Seq.map int
    |> Seq.sum

let levelUp (mebius:Mebius)=
    if mebius.level < 29 then
        let ableLevelUp =
            MebiusEnchantments.itr
            |>Seq.filter (fun (enchant:MebiusEnchantments) ->
                enchant.Enchant.unlockLevel <= mebius.level &&
                enchant.Enchant.maxLevel > mebius.level)
            |>Seq.toList
        let eIndex = mebius.rand.Next(ableLevelUp.Length)
        match mebius.enchants|>Seq.contains ableLevelUp[eIndex] with
        | true ->
            let after =
                mebius.enchants
                |> Seq.map (fun x -> if x = ableLevelUp[eIndex] then x.Inc() else x)
                |> Seq.toList
            {mebius with level = mebius.level+1; enchants = after}
        | false ->
            let after =
                ableLevelUp[eIndex]::mebius.enchants
            {mebius with level = mebius.level+1; enchants = after}
    else if mebius.level = 29 then
        let after = Unbreakable(1)::mebius.enchants
        {mebius with level = mebius.level+1; enchants = after}
    else mebius
    
let messageCreatedEvent(client:DiscordClient)(args:MessageCreateEventArgs) =
    task{
        let bytes = System.Text.Encoding.UTF32.GetBytes name
        let seed = bytes |> Seq.sum
        let mebius = Mebius.Mebius.New (int seed)
        
    }:>Task