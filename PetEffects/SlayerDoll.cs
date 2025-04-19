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
        public int mirrorTimer = 0;
        public int mirrorTimerMax = 5;
        public int mirrorCooldown = 5400;
        public bool mirrorReducOn = false;
        public float mirrorReduction = 0.55f;
        //Stasis Drone
        public float droneMovespeed = 0.2f;
        public float droneAccelspeed = 0.15f;
        public float droneWingTimeStore = 0.4f;
        private float droneWingTimeBank = 0;
        //Baby Signus
        public int signusActiveDmg = 150;
        public int trigger = -1;

        public override int PetAbilityCooldown => mirrorCooldown;

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            //Mirror Matter Reduction Bool enabling and Signus Scythe trigger set
            if (Pet.AbilityPressCheck() && PetIsEquipped())
            {
                if (Pet.timer <= 0)
                {
                    mirrorReducOn = true;
                    trigger = 80;
                    Pet.timer = Pet.timerMax;
                }
            }
            //Stasis Drone Horizontal Wingspeed increase
            if (Player.wingTime > 0 && PetIsEquipped() && triggersSet.Jump && Player.dead == false && Player.equippedWings is not null && Player.ZoneRain is not false)
            {
                float total = Math.Abs(Player.velocity.Y) + Math.Abs(Player.velocity.X);
                float xRemain = Math.Abs(Player.velocity.X) / total;
                if (xRemain is float.NaN)
                {
                    xRemain = 0;
                }
                droneWingTimeBank += Math.Abs(xRemain * droneWingTimeStore);
            }
        }
        public override void ModifyHurt(ref Player.HurtModifiers modifiers)
        {
            //Applying Mirror Matter Thorns + Damage Reduction
            if (PetIsEquipped() && mirrorReducOn == true)
            {
                modifiers.FinalDamage *= mirrorReduction; //45% Damage Reduction
                Player.thorns += 6;
                mirrorTimer++;
                if (mirrorTimer >= mirrorTimerMax)
                {
                    mirrorReducOn = false;
                    mirrorTimer = 0;
                }
            }
        }
        public override void PostUpdateMiscEffects()
        {
            //Spawning Signus Scythes
            if (PetIsEquipped() && mirrorReducOn == true && trigger >= 0)
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
                if (droneWingTimeBank >= 1 && Player.wingTime < Player.wingTimeMax)
                {
                    Player.wingTime++;
                    droneWingTimeBank--;
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
