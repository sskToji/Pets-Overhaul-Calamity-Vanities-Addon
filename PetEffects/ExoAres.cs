using System;
using System.Collections.Generic;
using PetsOverhaul.Systems;
using Terraria.ModLoader;
using CalValEX.Items.Pets.ExoMechs;
using Terraria;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Items.Weapons.Summon;
using Terraria.Localization;
using Terraria.DataStructures;
using CalamityMod.Projectiles.Ranged;

namespace POCalValAddon.PetEffects
{
    public sealed class ExoAres : PetEffect
    {
        public override int PetItemID => ModContent.ItemType<OminousCore>();
        public override PetClasses PetClassPrimary => PetClasses.Offensive;

        public float aresMeleeDmg = 0.2f;
        public float aresRangedHurtReduction = 0.9f;
        public float aresSummonDmg = 0.25f;
        public int aresNoUse = 0;
        public bool aresWeaponInUse = true;
        public static List<int> AresWeapons =
        [
            ModContent.ItemType<AresExoskeleton>(),
            ModContent.ItemType<PhotonRipper>(),
            ModContent.ItemType<TheJailor>(),
            ModContent.ProjectileType<PrismMine>(),
        ];

        public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
        {
            if (PetIsEquipped() && (item.type == ModContent.ItemType<PhotonRipper>()))
            {
                damage += aresMeleeDmg;
            }
            if (PetIsEquipped() && (item.type == ModContent.ItemType<AresExoskeleton>()))
            {
                damage += aresSummonDmg;
            }
            if (PetIsEquipped() && !AresWeapons.Contains(item.type)) //For nullifying damage from other weapons
            {
                damage *= aresNoUse;
            }
        }
        public override void ModifyHurt(ref Player.HurtModifiers modifiers)
        {
            if (PetIsEquipped() && Player.HasItem(ModContent.ItemType<TheJailor>()))
            {
                modifiers.FinalDamage *= aresRangedHurtReduction;
            }
        }

        public override void ModifyHitNPCWithItem(Item item, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (PetIsEquipped() && !AresWeapons.Contains(item.type))
            {
                target.takenDamageMultiplier = aresNoUse;
                if (target.lifeRegen < 0)
                {
                    target.lifeRegen = 0;
                }
            }
        }
        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (PetIsEquipped() && !(AresWeapons.Contains(proj.type) || proj.GetGlobalProjectile<AresProjectile>().aresProj == true))
            {
                target.takenDamageMultiplier = aresNoUse;
                if (target.lifeRegen < 0)
                {
                    target.lifeRegen = 0;
                }
            }
        }
        public sealed class AresProjectile : GlobalProjectile
        {
            public override bool InstancePerEntity => true;
            public bool aresProj = false;
            public override void OnSpawn(Projectile projectile, IEntitySource source)
            {
                if (source is EntitySource_ItemUse item && item.Item is not null && ExoAres.AresWeapons.Contains(item.Item.type))
                {
                    aresProj = true;
                }
                if (source is EntitySource_Parent parent && parent.Entity is Projectile proj && proj.GetGlobalProjectile<AresProjectile>().aresProj == true)
                {
                    aresProj = true;
                }
            }
        }
        public sealed class ExoAresPetItem : PetTooltip
        {
            public override PetEffect PetsEffect => aresBaby;
            public static ExoAres aresBaby
            {
                get
                {
                    if (Main.LocalPlayer.TryGetModPlayer(out ExoAres pet))
                        return pet;
                    else
                        return ModContent.GetInstance<ExoAres>();
                }
            }
            public override string PetsTooltip => Language.GetTextValue("Mods.POCalValAddon.PetTooltips.ExoMechs.ExoAres");
        }
    }
}
