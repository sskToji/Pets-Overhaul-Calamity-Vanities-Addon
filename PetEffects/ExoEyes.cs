using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Items.Weapons.Rogue;
using CalValEX.Items.Pets.ExoMechs;
using PetsOverhaul.Systems;
using POCalValAddon.Systems;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

// TODO : Check for Tooltip updates on Items (CritChance on Atom Splitter)

namespace POCalValAddon.PetEffects
{
    public sealed class ExoEyes : PetEffect
    {
        public override int PetItemID => ModContent.ItemType<GeminiMarkImplants>();
        public override PetClasses PetClassPrimary => PetClasses.Offensive;

        public float eyesRogueCrit = 0.5f;
        public float eyesRangedUse = 0.2f;
        public float eyesRangedSpeed = 0.1f;
        public int eyesNoUse = 0;
        public static bool eyesWeaponInUse = true;

        public override void ModifyWeaponCrit(Item item, ref float crit)
        {
            if (PetIsEquipped() && (item.type == ModContent.ItemType<TheAtomSplitter>()))
            {
                crit *= 1f + eyesRogueCrit;
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
            if (PetIsEquipped() && !CalValItemSets.EyesWeapons[item.type])
            {
                damage *= eyesNoUse;
            }
        }
        public override void ModifyHitNPCWithItem(Item item, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (PetIsEquipped() && !CalValItemSets.EyesWeapons[item.type])
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
            if (PetIsEquipped() && !(CalValItemSets.EyesWeapons[proj.type] || proj.GetGlobalProjectile<SourceProjectile>().myProj))
            {
                target.takenDamageMultiplier = eyesNoUse;
                if (target.lifeRegen < 0)
                {
                    target.lifeRegen = 0;
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
            public override string PetsTooltip => PetUtil.LocVal("PetTooltips.ExoMechs.ExoEyes")
                .Replace("<crit>", PetUtil.FloatToPercent(eyesBaby.eyesRogueCrit))
                .Replace("<use>", PetUtil.FloatToPercent(eyesBaby.eyesRangedUse))
                .Replace("<speed>", PetUtil.FloatToPercent(eyesBaby.eyesRangedSpeed));
            public override string SimpleTooltip => PetUtil.LocVal("SimplePetTooltips.ExoMechs.ExoEyes");
        }
    }
}
