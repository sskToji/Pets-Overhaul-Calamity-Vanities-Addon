using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Dusts;
using CalamityMod.Projectiles.Healing;
using CalValEX.Items.Pets.Elementals;
using Microsoft.Xna.Framework;
using PetsOverhaul.Systems;
using POCalValAddon.Systems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace POCalValAddon.PetEffects
{
    public sealed class MiniHeart : PetEffect
    {
        public override int PetItemID => ModContent.ItemType<MiniatureElementalHeart>();
        public override PetClasses PetClassPrimary => PetClasses.Offensive;
        public override PetClasses PetClassSecondary => PetClasses.Defensive;

        public override int PetAbilityCooldown => miniHeartCooldown;
        public int miniHeartCooldown = 600;
        public float radiusFire = 160f;
        public float radiusHeal = 480f;
        public float radiusStorm = 240f;
        public float radiusWater = 240f;
        public float movementSpeed = 0.25f;
        public int debuffTime = 300;

        public override void PostUpdateMiscEffects()
        {
            if (PetIsEquipped())
            {
                //Brimstone Elemental
                GlobalPet.CircularDustEffect(Player.Center, ModContent.DustType<BrimstoneFlame>(), (int)radiusFire, dustAmount: 64);
                GlobalPet.CircularDustEffect(Player.Center, DustID.Torch, (int)radiusFire, dustAmount: 32);
                foreach (NPC item in Main.ActiveNPCs)
                {
                    if (item.Distance(Player.Center) <= radiusFire && !(item.friendly || item.CountsAsACritter))
                    {
                        item.AddBuff(ModContent.BuffType<BrimstoneFlames>(), debuffTime);
                    }
                }
                //Cloud Elemental
                Player.moveSpeed += movementSpeed;

                //Rare Sand Elemental (heal orbs)
                if (Pet.timer <= 0)
                {
                    Projectile.NewProjectileDirect(GlobalPet.GetSource_Pet(EntitySourcePetIDs.PetProjectile), Player.Center - Main.rand.NextVector2Circular(radiusHeal, radiusHeal), -Vector2.UnitY * 6f, ModContent.ProjectileType<CactusHealOrb>(), 0, 0, Player.whoAmI);
                    Pet.timer = Pet.timerMax;
                }

                //Water Elemental
                GlobalPet.CircularDustEffect(Player.Center, DustID.Water, (int)radiusWater, dustAmount: 32);
                foreach (NPC item in Main.ActiveNPCs)
                {
                    if (item.Distance(Player.Center) <= radiusWater)
                    {
                        item.AddBuff(BuffID.Slow, debuffTime);
                    }
                }

            }
        }
        //Sand Elemental
        public override void OnHurt(Player.HurtInfo info)
        {
            if (PetIsEquipped())
            {
                GlobalPet.CircularDustEffect(Player.Center, DustID.Sand, (int)radiusStorm, dustAmount: 256);
                foreach (NPC item in Main.ActiveNPCs)
                {
                    if (item.Distance(Player.Center) <= radiusStorm)
                    {
                        item.AddBuff(BuffID.Confused, debuffTime);
                    }
                }
            }
        }

        public sealed class MiniHeartPetItem : PetTooltip
        {
            public override PetEffect PetsEffect => miniHeart;
            public static MiniHeart miniHeart
            {
                get
                {
                    if (Main.LocalPlayer.TryGetModPlayer(out MiniHeart pet))
                        return pet;
                    else
                        return ModContent.GetInstance<MiniHeart>();
                }
            }
            public override string PetsTooltip => PetUtil.LocVal("PetTooltips.Elementals.MiniatureElementalHeart")
                .Replace("<pxFire>", miniHeart.radiusFire.ToString())
                .Replace("<pxWater>", miniHeart.radiusWater.ToString())
                .Replace("<pxHeal>", miniHeart.radiusHeal.ToString())
                .Replace("<pxDust>", miniHeart.radiusStorm.ToString())
                .Replace("<secs>", PetUtil.IntToTime(miniHeart.debuffTime))
                .Replace("<speed>", PetUtil.FloatToPercent(miniHeart.movementSpeed))
                .Replace("<cd>", PetUtil.IntToTime(miniHeart.miniHeartCooldown));
            public override string SimpleTooltip => PetUtil.LocVal("SimplePetTooltips.Elementals.MiniatureElementalHeart");
        }
    }
}
