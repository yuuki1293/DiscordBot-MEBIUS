module DiscordBot_MEBIUS.Mebius.Enchantment

type MebiusEnchantment(unlockLevel: int, maxLevel: int, displayName: string) =
    let unlockLevel = unlockLevel
    let maxLevel = maxLevel
    let displayName = displayName

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
    member this.Enchant =
        match this with
        | Protection _ -> MebiusEnchantment(2, 10, "ダメージ軽減")
        | Durability _ -> MebiusEnchantment(1, 10, "耐久力")
        | Mending _ -> MebiusEnchantment(1, 1, "修繕")
        | FireProtection _ -> MebiusEnchantment(6, 10, "火炎耐性")
        | ProjectileProtection _ -> MebiusEnchantment(6, 10, "飛び道具耐性")
        | ExplosionProtection _ -> MebiusEnchantment(6, 10, "爆発耐性")
        | Respiration _ -> MebiusEnchantment(15, 3, "水中呼吸")
        | WaterAffinity _ -> MebiusEnchantment(15, 1, "水中採掘")
        | Unbreakable _ -> MebiusEnchantment(30, 1, "耐久無限")
