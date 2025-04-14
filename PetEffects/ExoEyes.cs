using System;
using System.Collections.Generic;
using PetsOverhaul.Systems;
using Terraria.ModLoader;
using CalValEX.Items.Pets.ExoMechs;
using Terraria;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Items.Weapons.Rogue;
using Terraria.Localization;
using Terraria.DataStructures;

// TODO : Check for Tooltip updates on Items (CritChance on Atom Splitter)

namespace POCalValAddon.PetEffects
{
    public sealed class ExoEyes : PetEffect
    {
        public override int PetItemID => ModContent.ItemType<GeminiMarkImplants>();
        public override PetClasses PetClassPrimary => PetClasses.Offensive;

        public float eyesRogueCrit = 0.15f;
        public float eyesRangedUse = 0.2f;
        public float eyesRangedSpeed = 0.1f;
        public int eyesNoUse = 0;
        public static bool eyesWeaponInUse = true;
        public static List<int> EyesWeapons =
        [
            ModContent.ItemType<TheAtomSplitter>(),
            ModContent.ItemType<SurgeDriver>(),
        ];
        public override void ModifyWeaponCrit(Item item, ref float crit)
        {
            if (PetIsEquipped() && (item.type == ModContent.ItemType<TheAtomSplitter>()))
            {
                crit += eyesRogueCrit;
            }
        }
        public override void PostUpdateMiscEffects()
        {
            if (PetIsEquipped() && Player.HasItem(ModContent.ItemType<SurgeDriver>()))
            {
                Player.GetAttackSpeed<RangedDamageClass>() += eyesRangedUse;
                Player.moveSpeed -= eyesRangedSpeed;
            }
        }

        public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
        {
            if(PetIsEquipped() && !EyesWeapons.Contains(item.type))
            {
                damage *= eyesNoUse;
            }
        }
        public override void ModifyHitNPCWithItem(Item item, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (PetIsEquipped() && !EyesWeapons.Contains(item.type))
            {
                target.takenDamageMultiplier = eyesNoUse;
                if (target.lifeRegen < 0)
                {
                    target.lifeRegen = 0;
                }
            }
        }
        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (PetIsEquipped() && !(EyesWeapons.Contains(proj.type) || proj.GetGlobalProjectile<EyesProjectile>().eyesProj))
            {
                target.takenDamageMultiplier = eyesNoUse;
                if (target.lifeRegen < 0)
                {
                    target.lifeRegen = 0;
                }
            }
        }
        public sealed class EyesProjectile : GlobalProjectile
        {
            public override bool InstancePerEntity => true;
            public bool eyesProj = false;
            public override void OnSpawn(Projectile projectile, IEntitySource source)
            {
                if (source is EntitySource_ItemUse item && item.Item is not null && ExoEyes.EyesWeapons.Contains(item.Item.type))
                {
                    eyesProj = true;
                }
                if (source is EntitySource_Parent parent && parent.Entity is Projectile proj && proj.GetGlobalProjectile<EyesProjectile>().eyesProj == true)
                {
                    eyesProj = true;
                }
            }
        }
        public sealed class ExoEyesPetItem : PetTooltip
        {
            public override PetEffect PetsEffect => eyesBaby;
            public static ExoEyes eyesBaby
            {
                get
                {
                    if (Main.LocalPlayer.TryGetModPlayer(out ExoEyes pet))
                        return pet;
                    else
                        return ModContent.GetInstance<ExoEyes>();
                }
            }
            public override string PetsTooltip => Language.GetTextValue("Mods.POCalValAddon.PetTooltips.ExoMechs.ExoEyes");
        }
    }
}
