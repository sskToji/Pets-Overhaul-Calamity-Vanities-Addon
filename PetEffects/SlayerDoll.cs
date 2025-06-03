using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalValEX.Items.Pets;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using PetsOverhaul.Systems;
using Terraria;
using Terraria.GameInput;
using Terraria.Localization;
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
        //Stasis Drone
        public float droneMovespeed = 0.2f;
        public float droneAccelspeed = 0.15f;
        public float droneWingSpeed = 0.25f;
        //Baby Signus
        public int signusActiveDmg = 150;
        public int trigger = -1;

        public override int PetAbilityCooldown => mirrorCooldown;
        public override int PetStackCurrent => mirrorHit;
        public override int PetStackMax => mirrorHitMax;
        public override string PetStackText => "shielded hits";

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            //Mirror Matter Reduction Bool enabling and Signus Scythe trigger set
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
            //Applying Mirror Matter Thorns + Damage Reduction
            if (PetIsEquipped() && mirrorHit > 0)
            {
                modifiers.FinalDamage *= mirrorReduction; //45% Damage Reduction
                Player.thorns += 6;
                mirrorHit--;
            }
        }
        public override void PostUpdateMiscEffects()
        {
            //Spawning Signus Scythes
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
            //Applying Stasis Drone Movespeed Increase
            if (PetIsEquipped() && Player.ZoneRain is not false)
            {
                Player.moveSpeed += droneMovespeed;
            }
            
        }
        public override void PostUpdateRunSpeeds()
        {
            //Applying Stasis Drone Run Acceleration
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
                    speed += slayerDoll.droneWingSpeed;
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
            public override string PetsTooltip => Language.GetTextValue("Mods.POCalValAddon.PetTooltips.SlayerDoll")
                .Replace("<keybind>", PetTextsColors.KeybindText(PetKeybinds.UsePetAbility));
        }
    }
}
