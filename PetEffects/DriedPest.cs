using System;
using Microsoft.Xna.Framework;
using Steamworks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using PetsOverhaul;
using PetsOverhaul.Items;
using PetsOverhaul.Systems;
using CalValEX.Items.Pets;
using System.Security.Cryptography.X509Certificates;
using Terraria.DataStructures;
using CalamityMod;

namespace POCalValAddon.PetEffects
{
    public sealed class DriedPest : PetEffect
    {
        public override int PetItemID => ModContent.ItemType<DriedLocket>();
        public override PetClasses PetClassPrimary => PetClasses.Defensive;
        public override PetClasses PetClassSecondary => PetClasses.Mobility;

        public float desertMovement = 0.20f;
        public float hpTreshold = 0.2f;
        public float baseHpShield = 0.1f;
        public int driedCooldown = 300;
        public int driedShieldDuration = 900;
        private int lifeguardMultTimer = 0;
        public override int PetAbilityCooldown => driedCooldown;

        public override void PostUpdateMiscEffects()
        {
            if (PetIsEquipped() && Player.ZoneDesert)
            {
                Player.moveSpeed += desertMovement;
            }
        }

        public override void ExtraPreUpdate()
        {
            lifeguardMultTimer--;
            if (lifeguardMultTimer < 0)
            {
                lifeguardMultTimer = 0;
            }
        }
        public override void ModifyHurt(ref Player.HurtModifiers modifiers)
        {
            if (PetIsEquipped() && Pet.timer <= 0)
            {
                int shieldAmount = (int)((Player.statLifeMax * baseHpShield));

                modifiers.ModifyHurtInfo += (ref Player.HurtInfo info) =>
                {
                    if (info.Damage >= (Player.statLife - Player.statLifeMax2 * hpTreshold))
                    {
                        int belowHpDamage = (int)(Player.statLifeMax2 * hpTreshold - (Player.statLife - info.Damage));
                        int reduce = info.Damage - 1;

                        reduce = Math.Min(reduce, belowHpDamage);
                        reduce = Math.Min(reduce, shieldAmount);

                        if (reduce > 0)
                        {
                            CombatText.NewText(Player.getRect(), Color.DarkGreen, -reduce);
                        }
                        info.Damage -= reduce;
                        Pet.AddShield(shieldAmount - reduce, driedShieldDuration, false);
                        Pet.timer = Pet.timerMax;
                        lifeguardMultTimer = driedShieldDuration;
                    }
                };
            }
        }

        public sealed class DriedPestPetItem : PetTooltip
        {
            public override PetEffect PetsEffect => desertPest;
            public static DriedPest desertPest
            {
                get
                {
                    if (Main.LocalPlayer.TryGetModPlayer(out DriedPest pet))
                        return pet;
                    else
                        return ModContent.GetInstance<DriedPest>();
                }
            }
            public override string PetsTooltip => Language.GetTextValue("Mods.POCalValAddon.PetTooltips.DriedPest");
        }
    }
}
