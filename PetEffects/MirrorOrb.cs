using System;
using CalValEX.Items.Pets;
using PetsOverhaul.Projectiles;
using PetsOverhaul.Systems;
using POCalValAddon.Systems;
using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;

namespace POCalValAddon.PetEffects
{
    public sealed class MirrorOrb : PetEffect
    {
        public override int PetItemID => ModContent.ItemType<MirrorMatter>();
        public override PetClasses PetClassPrimary => PetClasses.Defensive;
        public override int PetAbilityCooldown => mirrorCooldown;
        public override int PetStackCurrent => mirrorHit;
        public override int PetStackMax => mirrorHitMax;
        public override string PetStackText => "shielded hits";

        public int mirrorHit = 0;
        public int mirrorHitMax = 5;
        public int mirrorCooldown = 7200;
        public float mirrorReduction = 0.4f;
        public float mirrorReflect = 0.4f;
        public bool mirrorBool = false;

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (Pet.AbilityPressCheck() && PetIsEquipped())
            {
                if (Pet.timer <= 0)
                {
                    mirrorHit = mirrorHitMax;
                    Pet.timer = Pet.timerMax;
                }
            }
        }
        public override void ModifyHurt(ref Player.HurtModifiers modifiers)
        {
            if (PetIsEquipped() && mirrorHit > 0)
            {
                modifiers.FinalDamage *= 1f - mirrorReduction; //40% Damage Reduction
                mirrorHit--;
                mirrorBool = true;
            }
            else mirrorBool = false;
        }
        public override void OnHurt(Player.HurtInfo info)
        {
            if (PetIsEquipped() && mirrorBool == true)
            {
                if (info.DamageSource.TryGetCausingEntity(out Entity entity))
                {
                    int damageTaken = Math.Min(info.SourceDamage, Player.statLife);
                    if (entity is Projectile projectile && projectile.TryGetGlobalProjectile(out ProjectileSourceChecks proj) && Main.npc[proj.sourceNpcId].active && Main.npc[proj.sourceNpcId].dontTakeDamage == false)
                    {
                        Main.npc[proj.sourceNpcId].SimpleStrikeNPC(Main.DamageVar(Pet.PetDamage(damageTaken * mirrorReflect, DamageClass.Generic), Player.luck), info.HitDirection, Main.rand.NextBool((int)Math.Min(Player.GetTotalCritChance<GenericDamageClass>(), 100), 100), 1f, DamageClass.Generic);
                    }
                    else if (entity is NPC npc && npc.active == true && npc.dontTakeDamage == false)
                    {
                        npc.SimpleStrikeNPC(Main.DamageVar(Pet.PetDamage(damageTaken * mirrorReflect, DamageClass.Generic), Player.luck), info.HitDirection, Main.rand.NextBool((int)Math.Min(Player.GetTotalCritChance<GenericDamageClass>(), 100), 100), 1f, DamageClass.Generic);
                    }
                }
            }
        }

        public sealed class MirrorOrbPetItem : PetTooltip
        {
            public override PetEffect PetsEffect => mirrorOrb;
            public static MirrorOrb mirrorOrb
            {
                get
                {
                    if (Main.LocalPlayer.TryGetModPlayer(out MirrorOrb pet))
                        return pet;
                    else
                        return ModContent.GetInstance<MirrorOrb>();
                }
            }
            public override string PetsTooltip => PetUtil.LocVal("PetTooltips.MirrorOrb")
                .Replace("<keybind>", PetTextsColors.KeybindText(PetKeybinds.UsePetAbility))
                .Replace("<reflect>", PetUtil.FloatToPercent(mirrorOrb.mirrorReduction))
                .Replace("<hitAmount>", mirrorOrb.mirrorHitMax.ToString());
            public override string SimpleTooltip => PetUtil.LocVal("SimplePetTooltips.MirrorOrb").Replace("<keybind>", PetTextsColors.KeybindText(PetKeybinds.UsePetAbility));
        }
    }
}
