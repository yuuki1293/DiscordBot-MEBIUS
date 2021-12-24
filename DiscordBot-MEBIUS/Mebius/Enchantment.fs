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
        | _ -> failwith "todo"
