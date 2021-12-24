module DiscordBot_MEBIUS.Mebius.Enchantment

type MebiusEnchantment(unlockLevel: int, maxLevel: int, displayName: string) =
    let unlockLevel = unlockLevel
    let maxLevel = maxLevel
    let displayName = displayName

type MebiusEnchantments =
    | Protection
    | Durability
    | Mending
    | FireProtection
    | ProjectileProtection
    | ExplosionProtection
    | Respiration
    | WaterAffinity
    | Unbreakable
    member this.Enchant =
        match this with
        | Protection -> MebiusEnchantment(2, 10, "ダメージ軽減")
        | Durability -> MebiusEnchantment(1, 10, "耐久力")
        | Mending -> MebiusEnchantment(1, 1, "修繕")
        | FireProtection -> MebiusEnchantment(6, 10, "火炎耐性")
        | ProjectileProtection -> MebiusEnchantment(6, 10, "飛び道具耐性")
        | ExplosionProtection -> MebiusEnchantment(6, 10, "爆発耐性")
        | Respiration -> MebiusEnchantment(15, 3, "水中呼吸")
        | WaterAffinity -> MebiusEnchantment(15, 1, "水中採掘")
        | Unbreakable -> MebiusEnchantment(30, 1, "耐久無限")
        | _ -> failwith "todo"
