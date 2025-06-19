using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod.Items.Weapons.Summon;
using CalValEX.Items.Pets.ExoMechs;
using PetsOverhaul.Systems;
using POCalValAddon.Systems;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

// TODO : Check if Atlas Munitions Beacon acc works on this and other pets

namespace POCalValAddon.PetEffects
{
    public sealed class ExoWorm : PetEffect
    {
        public override int PetItemID => ModContent.ItemType<GunmetalRemote>();
        public override PetClasses PetClassPrimary => PetClasses.Offensive;

        public float wormSummonDmg = 0.3f;
        public float wormMeleeDmg = 0.2f;
        public float wormRogueCrit = 0.5f;
        public int wormNoUse = 0;

        public override void ModifyWeaponCrit(Item item, ref float crit)
        {
            if (PetIsEquipped() && (item.type == ModContent.ItemType<RefractionRotor>()))
            {
                crit *= 1f + wormRogueCrit;
            }
        }
        public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
        {
            if (PetIsEquipped() && (item.type == ModContent.ItemType<SpineOfThanatos>()))
            {
                damage += wormMeleeDmg;
            }
            if (PetIsEquipped() && Player.HasItem(ModContent.ItemType<AtlasMunitionsBeacon>()))
            {
                damage += wormSummonDmg;
            }
            if (PetIsEquipped() && !CalValItemSets.WormWeapons[item.type])
            {
                damage *= wormNoUse;
            }
        }

        public override void ModifyHitNPCWithItem(Item item, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (PetIsEquipped() && !CalValItemSets.WormWeapons[item.type])
            {
                target.takenDamageMultiplier = wormNoUse;
                if (target.lifeRegen < 0)
                {
                    target.lifeRegen = 0;
                }
            }
        }
        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (PetIsEquipped() && !(CalValItemSets.WormWeapons[proj.type] || proj.GetGlobalProjectile<SourceProjectile>().myProj))
            {
                target.takenDamageMultiplier = wormNoUse;
                if (target.lifeRegen < 0)
                {
                    target.lifeRegen = 0;
                }
            }
        }

        public sealed class ExoWormPetItem : PetTooltip
        {
            public override PetEffect PetsEffect => babyWorm;
            public static ExoWorm babyWorm
            {
                get
                {
                    if (Main.LocalPlayer.TryGetModPlayer(out ExoWorm pet))
                        return pet;
                    else
                        return ModContent.GetInstance<ExoWorm>();
                }
            }
            public override string PetsTooltip => PetUtil.LocVal("PetTooltips.ExoMechs.ExoWorm")
                .Replace("<stDmg>", PetUtil.FloatToPercent(babyWorm.wormMeleeDmg))
                .Replace("<ambDmg>", PetUtil.FloatToPercent(babyWorm.wormSummonDmg))
                .Replace("<crit>", PetUtil.FloatToPercent(babyWorm.wormRogueCrit));
            public override string SimpleTooltip => PetUtil.LocVal("SimplePetTooltips.ExoMechs.ExoWorm");
        }
    }
}
