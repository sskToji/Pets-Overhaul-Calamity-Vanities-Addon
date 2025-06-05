using System;
using CalValEX.Items.Pets;
using Microsoft.Xna.Framework;
using PetsOverhaul.Systems;
using POCalValAddon.Systems;
using Terraria;
using Terraria.ModLoader;

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
                int shieldAmount = (int)(Player.statLifeMax * baseHpShield);

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
            public override string PetsTooltip => PetUtil.LocVal("PetTooltips.DriedPest")
                .Replace("<health>", PetUtil.FloatToPercent(desertPest.hpTreshold))
                .Replace("<shield>", PetUtil.FloatToPercent(desertPest.baseHpShield))
                .Replace("<cd>", PetUtil.IntToTime(desertPest.driedCooldown))
                .Replace("<speed>", PetUtil.FloatToPercent(desertPest.desertMovement));
            public override string SimpleTooltip => PetUtil.LocVal("SimplePetTooltips.DriedPest");
        }
    }
}
