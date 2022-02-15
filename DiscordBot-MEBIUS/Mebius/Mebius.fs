module DiscordBot_MEBIUS.Mebius

open System
open System.Threading.Tasks
open DSharpPlus
open DSharpPlus.Entities
open DSharpPlus.EventArgs
open DiscordBot_MEBIUS.ReadJson

let convertRoman num =
    if num > 10 then num.ToString()
    else
        match num with
        | 1 -> "Ⅰ"
        | 2 -> "Ⅱ"
        | 3 -> "Ⅲ"
        | 4 -> "Ⅳ"
        | 5 -> "Ⅴ"
        | 6 -> "Ⅵ"
        | 7 -> "Ⅶ"
        | 8 -> "Ⅷ"
        | 9 -> "Ⅸ"
        | 10 -> "Ⅹ"
        | _ -> ""

type MebiusEnchantment(unlockLevel: int, maxLevel: int, displayName: string, dbName: string) =
    member val unlockLevel = unlockLevel
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
    member this.level =
        match this with
        | Protection x -> x
        | Durability x -> x
        | Mending x -> x
        | FireProtection x -> x
        | ProjectileProtection x -> x
        | ExplosionProtection x -> x
        | Respiration x -> x
        | WaterAffinity x -> x
        | Unbreakable x -> x
    member this.levelInList (list:MebiusEnchantments list) =
        let result = list |> Seq.filter (fun x -> x.GetType().Name = this.GetType().Name)
                          |> Seq.toList
        if result.IsEmpty then 0
        else
            result[0].level
    
type MebiusId = MebiusId of id: int

type Mebius =
    { id: int
      level: int
      enchants: MebiusEnchantments list
      rand: Random}
    static member New id = {id=id;level=1;enchants=[Durability 3;Mending 1];rand=Random(id)}
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
                enchant.Enchant.maxLevel > enchant.levelInList mebius.enchants)
            |>Seq.toList
        let eIndex = mebius.rand.Next(ableLevelUp.Length)
        match mebius.enchants |> Seq.filter (fun x -> x.GetType().Name = ableLevelUp[eIndex].GetType().Name) |> Seq.isEmpty |> not with
        | true ->
            let after =
                mebius.enchants
                |> Seq.map (fun x -> if x.GetType().Name = ableLevelUp[eIndex].GetType().Name then x.Inc() else x)
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
    task{//TODO:task内にrecを含まないようにする
        if args.Channel.Name = appConf.GachaChannel.ToString() && not args.Author.IsBot then 
            let name = args.Message.Content
            let bytes = System.Text.Encoding.UTF32.GetBytes name
            let seed = bytes |> Seq.map int |> Seq.sum
            let mebius = Mebius.New (int seed)
            let rec maxLevel (mebius:Mebius) =
                if mebius.isMaxLevel() then mebius
                else mebius |> levelUp |> maxLevel
            let content =
                (maxLevel mebius).enchants
                |> Seq.map (fun enchant -> $"{enchant.Enchant.displayName} {convertRoman enchant.level}")
                |> Seq.fold (fun left right-> left + "\n" + right) ""
                
            let embed =
                DiscordEmbedBuilder()
                    .WithColor(DiscordColor.Black)
                    .WithTitle(name)
                    .WithDescription(content)
                    .Build()
            let msg =
                DiscordMessageBuilder()
                    .WithEmbed(embed)
            return! client.SendMessageAsync(args.Channel, msg) :> Task
        else
            return! Task.CompletedTask
    } :>Task