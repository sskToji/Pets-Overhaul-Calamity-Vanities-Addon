using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetsOverhaul.Systems;
using Terraria.ModLoader;
using CalValEX.Items.Pets.ExoMechs;
using Terraria;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod.Items.Weapons.Summon;
using CalamityMod.Projectiles.Summon;
using Terraria.Localization;
using CalamityMod.Buffs.DamageOverTime;
using Terraria.DataStructures;

// TODO : Check if Atlas Munitions Beacon acc works on this and other pets

namespace POCalValAddon.PetEffects
{
    public sealed class ExoWorm : PetEffect
    {
        public override int PetItemID => ModContent.ItemType<GunmetalRemote>();
        public override PetClasses PetClassPrimary => PetClasses.Offensive;

        public float wormSummonDmg = 0.3f;
        public float wormMeleeDmg = 0.2f;
        public float wormRogueCrit = 0.15f;
        public int wormNoUse = 0;
        public static List<int> WormWeapons =
        [
            ModContent.ItemType<SpineOfThanatos>(),
            ModContent.ItemType<AtlasMunitionsBeacon>(),
            ModContent.ItemType<RefractionRotor>(),
        ];

        public override void ModifyWeaponCrit(Item item, ref float crit)
        {
            if (PetIsEquipped() && (item.type == ModContent.ItemType<RefractionRotor>()))
            {
                crit += wormRogueCrit;
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
            if (PetIsEquipped() && !WormWeapons.Contains(item.type)) //For nullifying damage of other weapons
            {
                damage *= wormNoUse;
            }
        }

        public override void ModifyHitNPCWithItem(Item item, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (PetIsEquipped() && !WormWeapons.Contains(item.type))
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
            if (PetIsEquipped() && !(WormWeapons.Contains(proj.type) || proj.GetGlobalProjectile<WormProjectile>().wormProj))
            {
                target.takenDamageMultiplier = wormNoUse;
                if (target.lifeRegen < 0)
                {
                    target.lifeRegen = 0;
                }
            }
        }
        public sealed class WormProjectile : GlobalProjectile
        {
            public override bool InstancePerEntity => true;
            public bool wormProj = false;
            public override void OnSpawn(Projectile projectile, IEntitySource source)
            {
                if (source is EntitySource_ItemUse item && item.Item is not null && ExoWorm.WormWeapons.Contains(item.Item.type))
                {
                    wormProj = true;
                }
                if (source is EntitySource_Parent parent && parent.Entity is Projectile proj && proj.GetGlobalProjectile<WormProjectile>().wormProj == true)
                {
                    wormProj = true;
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
            public override string PetsTooltip => Language.GetTextValue("Mods.POCalValAddon.PetTooltips.ExoMechs.ExoWorm");
        }
    }
}
