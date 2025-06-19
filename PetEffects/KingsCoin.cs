using System;
using CalValEX.Items.Pets;
using Microsoft.Xna.Framework;
using PetsOverhaul.Systems;
using POCalValAddon.Systems;
using Terraria;
using Terraria.ModLoader;

namespace POCalValAddon.PetEffects
{
    public sealed class KingsCoin : PetEffect
    {
        public override int PetItemID => ModContent.ItemType<TheSeaKingsCoin>();
        public override PetClasses PetClassPrimary => PetClasses.Mobility;
        public override PetClasses PetClassSecondary => PetClasses.Utility;

        public float kingMovement = 0.25f;
        public int kingCooldown = 300;
        public float hpTreshold = 0.2f;
        public float baseHpShield = 0.1f;
        public int kingShieldDuration = 900;
        private int lifeguardMultTimer = 0;
        public float moistRadius = 20f;
        public int trigger = -1;
        public int moistDmg = 100;
        public override int PetAbilityCooldown => kingCooldown;

        public override void PostUpdateMiscEffects()
        {
            if (PetIsEquipped() && (Player.ZoneBeach || Player.ZoneDesert))
            {
                Player.moveSpeed += kingMovement;
            }
            if (PetIsEquipped() && trigger >= 0)
            {
                if (trigger % 20 == 0)
                {
                    for (int i = 0; i < 25; i++)
                    {
                        Projectile petProjectile = Projectile.NewProjectileDirect(GlobalPet.GetSource_Pet(EntitySourcePetIDs.PetProjectile), Player.Center + Main.rand.NextVector2CircularEdge(Player.width, Player.height), Main.rand.NextVector2CircularEdge(10, 10), ModContent.ProjectileType<CalamityMod.Projectiles.Boss.WaterSpear>(), Pet.PetDamage(moistDmg, DamageClass.Melee), 0, Player.whoAmI);
                        petProjectile.DamageType = DamageClass.Melee;
                        petProjectile.CritChance = (int)Player.GetTotalCritChance<GenericDamageClass>();
                        petProjectile.friendly = true;
                        petProjectile.hostile = false;
                    }
                }
                trigger--;
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
                        Pet.AddShield(shieldAmount - reduce, kingShieldDuration, false);

                        trigger = 100;
                        Pet.timer = Pet.timerMax;
                        lifeguardMultTimer = kingShieldDuration;
                    }
                };
            }
        }

        public sealed class KingsCoinPetItem : PetTooltip
        {
            public override PetEffect PetsEffect => kingCoin;
            public static KingsCoin kingCoin
            {
                get
                {
                    if (Main.LocalPlayer.TryGetModPlayer(out KingsCoin pet))
                        return pet;
                    else
                        return ModContent.GetInstance<KingsCoin>();
                }
            }
            public override string PetsTooltip => PetUtil.LocVal("PetTooltips.KingsCoin")
                .Replace("<minHp>", PetUtil.FloatToPercent(kingCoin.hpTreshold))
                .Replace("<health>", PetUtil.FloatToPercent(kingCoin.baseHpShield))
                .Replace("<spear>", kingCoin.moistDmg.ToString())
                .Replace("<cd>", PetUtil.IntToTime(kingCoin.kingCooldown))
                .Replace("<shield>", PetUtil.IntToTime(kingCoin.kingShieldDuration))
                .Replace("<speed>", PetUtil.FloatToPercent(kingCoin.kingMovement));
            public override string SimpleTooltip => PetUtil.LocVal("SimplePetTooltips.KingsCoin");
        }
    }
}
