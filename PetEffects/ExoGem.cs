using CalamityMod;
using CalValEX.Items.Pets.ExoMechs;
using PetsOverhaul.Systems;
using POCalValAddon.Systems;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace POCalValAddon.PetEffects
{
    public sealed class ExoGem : PetEffect
    {
        public override int PetItemID => ModContent.ItemType<ExoGemstone>();
        public override PetClasses PetClassPrimary => PetClasses.Offensive;

        public float gemMeleeDmg = 0.3f;
        public float gemMeleeCrit = 0.5f;
        public float gemRogueDmg = 0.3f;
        public float gemRogueCrit = 0.5f;
        public float gemRangedDmg = 0.3f;
        public float gemRangedUse = 0.4f;
        public float gemSummonDmg = 0.3f;
        public float gemWeaponDmg = 0.15f;
        public float gemWeaponCrit = 0.25f;

        public override void ModifyWeaponCrit(Item item, ref float crit)
        {
            if (PetIsEquipped() && CalValItemSets.GemMelee[item.type])
            {
                crit *= 1f + gemMeleeCrit;
            }
            if (PetIsEquipped() && CalValItemSets.GemRogue[item.type])
            {
                crit *= 1f + gemRogueCrit;
            }
            if (PetIsEquipped() && (item.CountsAsClass<MeleeDamageClass>() || item.CountsAsClass<RogueDamageClass>()) && !(CalValItemSets.GemRogue[item.type] || CalValItemSets.GemMelee[item.type]))
            {
                crit *= 1f + gemWeaponCrit;
            }
        }
        public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
        {
            if (PetIsEquipped() && CalValItemSets.GemMelee[item.type])
            {
                damage += gemMeleeDmg;
            }
            if (PetIsEquipped() && CalValItemSets.GemRogue[item.type])
            {
                damage += gemRogueDmg;
            }
            if (PetIsEquipped() && CalValItemSets.GemRanged[item.type])
            {
                damage += gemRangedDmg;
            }
            if (PetIsEquipped() && CalValItemSets.GemSummon[item.type])
            {
                damage += gemSummonDmg;
            }
            if (PetIsEquipped() && (item.CountsAsClass<MeleeDamageClass>() || item.CountsAsClass<RogueDamageClass>() || item.CountsAsClass<SummonDamageClass>() || item.CountsAsClass<RangedDamageClass>()) && !(CalValItemSets.GemRogue[item.type] || CalValItemSets.GemMelee[item.type]))
            {
                damage += gemWeaponDmg;
            }
        }
        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (PetIsEquipped() && proj.GetGlobalProjectile<GemProjectile>().gemProj == true && (proj.CountsAsClass<SummonDamageClass>() || proj.CountsAsClass<RangedDamageClass>()))
            {
                modifiers.FinalDamage += gemWeaponDmg;
            }
        }
        public override void PostUpdateMiscEffects()
        {
            if (PetIsEquipped())
            {
                Player.GetAttackSpeed<RangedDamageClass>() += gemRangedUse;
            }
        }
        public sealed class GemProjectile : GlobalProjectile
        {
            public override bool InstancePerEntity => true;
            public bool gemProj = false;
            public override void OnSpawn(Projectile projectile, IEntitySource source)
            {
                if (source is EntitySource_ItemUse item && item.Item is not null)
                {
                    gemProj = true;
                }
                if (source is EntitySource_Parent parent && parent.Entity is Projectile proj && proj.GetGlobalProjectile<GemProjectile>().gemProj == true)
                {
                    gemProj = true;
                }
            }
        }
        public sealed class ExoGemPetItem : PetTooltip
        {
            public override PetEffect PetsEffect => gemBabies;
            public static ExoGem gemBabies
            {
                get
                {
                    if (Main.LocalPlayer.TryGetModPlayer(out ExoGem pet))
                        return pet;
                    else
                        return ModContent.GetInstance<ExoGem>();
                }
            }
            public override string PetsTooltip => PetUtil.LocVal("PetTooltips.ExoMechs.ExoGemstone")
                .Replace("<dmg>", PetUtil.FloatToPercent(gemBabies.gemWeaponDmg))
                .Replace("<crit>", PetUtil.FloatToPercent(gemBabies.gemWeaponCrit))
                .Replace("<meleeDmg>", PetUtil.FloatToPercent(gemBabies.gemMeleeDmg))
                .Replace("<meleeCrit>", PetUtil.FloatToPercent(gemBabies.gemMeleeCrit))
                .Replace("<rogueDmg>", PetUtil.FloatToPercent(gemBabies.gemRogueDmg))
                .Replace("<rogueCrit>", PetUtil.FloatToPercent(gemBabies.gemRogueCrit))
                .Replace("<rangedDmg>", PetUtil.FloatToPercent(gemBabies.gemRangedDmg))
                .Replace("<use>", PetUtil.FloatToPercent(gemBabies.gemRangedUse))
                .Replace("<summonDmg>", PetUtil.FloatToPercent(gemBabies.gemSummonDmg));
            public override string SimpleTooltip => PetUtil.LocVal("SimplePetTooltips.ExoMechs.ExoGemstone");
        }
    }
}
