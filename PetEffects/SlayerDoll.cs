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
    public sealed class SlayerDoll : PetEffect
    {
        public override int PetItemID => ModContent.ItemType<GodSlayerDoll>();
        public override PetClasses PetClassPrimary => PetClasses.Defensive;
        public override PetClasses PetClassSecondary => PetClasses.Utility;

        //Mirror Matter 
        public int mirrorHit = 0;
        public int mirrorHitMax = 5;
        public int mirrorCooldown = 5400;
        public float mirrorReduction = 0.55f;
        public float mirrorReflect = 0.55f;
        public bool mirrorBool = false;
        //Stasis Drone
        public float droneMovespeed = 0.2f;
        public float droneAccelspeed = 0.15f;
        public float droneWingspeed = 0.25f;
        //Baby Signus
        public int signusActiveDmg = 150;
        public int trigger = -1;

        public override int PetAbilityCooldown => mirrorCooldown;
        public override int PetStackCurrent => mirrorHit;
        public override int PetStackMax => mirrorHitMax;
        public override string PetStackText => "shielded hits";

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (Pet.AbilityPressCheck() && PetIsEquipped())
            {
                if (Pet.timer <= 0)
                {
                    mirrorHit = mirrorHitMax;
                    trigger = 80;
                    Pet.timer = Pet.timerMax;
                }
            }
        }
        public override void ModifyHurt(ref Player.HurtModifiers modifiers)
        {
            if (PetIsEquipped() && mirrorHit > 0)
            {
                modifiers.FinalDamage *= mirrorReduction;
                mirrorBool = true;
                mirrorHit--;
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
        public override void PostUpdateMiscEffects()
        {
            if (PetIsEquipped() && trigger >= 0)
            {
                if (trigger % 20 == 0)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        Projectile petProjectile = Projectile.NewProjectileDirect(GlobalPet.GetSource_Pet(EntitySourcePetIDs.PetProjectile), Player.Center + Main.rand.NextVector2CircularEdge(Player.width, Player.height), Main.rand.NextVector2CircularEdge(10, 10), ModContent.ProjectileType<CalamityMod.Projectiles.Boss.SignusScythe>(), Pet.PetDamage(signusActiveDmg, DamageClass.Summon), 0, Player.whoAmI);
                        petProjectile.DamageType = DamageClass.Summon;
                        petProjectile.CritChance = (int)Player.GetTotalCritChance<GenericDamageClass>();
                        petProjectile.friendly = true;
                        petProjectile.hostile = false;
                    }
                }
                trigger--;
            }
            if (PetIsEquipped() && Player.ZoneRain is not false)
            {
                Player.moveSpeed += droneMovespeed;
            }

        }
        public override void PostUpdateRunSpeeds()
        {
            if (PetIsEquipped() && Player.ZoneRain is not false)
            {
                Player.runAcceleration *= droneAccelspeed + 1f;
            }
        }

        public sealed class SlayerDollWing : GlobalItem
        {
            public override bool InstancePerEntity => true;

            public override void HorizontalWingSpeeds(Item item, Player player, ref float speed, ref float acceleration)
            {
                if (player.TryGetModPlayer(out SlayerDoll slayerDoll) && player.GetModPlayer<GlobalPet>().PetInUseWithSwapCd(ModContent.ItemType<GodSlayerDoll>()) && player.ZoneRain is not false)
                {
                    speed += slayerDoll.droneWingspeed;
                }
            }
        }
        public sealed class SlayerDollPetItem : PetTooltip
        {
            public override PetEffect PetsEffect => slayerDoll;
            public static SlayerDoll slayerDoll
            {
                get
                {
                    if (Main.LocalPlayer.TryGetModPlayer(out SlayerDoll pet))
                        return pet;
                    else
                        return ModContent.GetInstance<SlayerDoll>();
                }
            }
            public override string PetsTooltip => PetUtil.LocVal("PetTooltips.SlayerDoll")
                .Replace("<keybind>", PetTextsColors.KeybindText(PetKeybinds.UsePetAbility))
                .Replace("<reflect>", PetUtil.FloatToPercent(slayerDoll.mirrorReflect))
                .Replace("<hitCount>", slayerDoll.mirrorHitMax.ToString())
                .Replace("<scytheDmg>", slayerDoll.signusActiveDmg.ToString())
                .Replace("<cd>", PetUtil.IntToTime(slayerDoll.mirrorCooldown))
                .Replace("<speed>", PetUtil.FloatToPercent(slayerDoll.droneMovespeed))
                .Replace("<>wing", PetUtil.FloatToPercent(slayerDoll.droneWingspeed))
                .Replace("<run>", PetUtil.FloatToPercent(slayerDoll.droneAccelspeed));
            public override string SimpleTooltip => PetUtil.LocVal("SimplePetTooltips.SlayerDoll").Replace("<keybind>", PetTextsColors.KeybindText(PetKeybinds.UsePetAbility));
        }
    }
}
