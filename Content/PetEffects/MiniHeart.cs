using System;
using Microsoft.Xna.Framework;
using PetsOverhaul.Systems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalValEX.Items.Pets.Elementals;
using CalamityMod.Dusts;
using CalamityMod.Projectiles.Healing;

/*
    Miniature Elemental Heart
    Combines Elemental Pet Effects and makes these effects better
*/

namespace POCalValAddon.Content.PetEffects
{
    public sealed class MiniHeart : PetEffect
    {
        public override int PetItemID => ModContent.ItemType<MiniatureElementalHeart>();
        public override PetClasses PetClassPrimary => PetClasses.Offensive;
        public override PetClasses PetClassSecondary => PetClasses.Defensive;
        public override int PetAbilityCooldown => 600;
       
        public float radiusFire = 160f;
        public float radiusHeal = 480f;
        public float radiusStorm = 240f;
        public float radiusWater = 240f;
        public float movementSpeed = 0.25f;

        
        public override void PostUpdateMiscEffects()
        {
            if (PetIsEquipped())
            {
                //Effects and visuals gained from Brimstone Elemental (Circle of Fire)
                GlobalPet.CircularDustEffect(Player.Center, ModContent.DustType<BrimstoneFlame>(), (int)radiusFire, dustAmount: 64);
                GlobalPet.CircularDustEffect(Player.Center, DustID.Torch, (int)radiusFire, dustAmount: 32);
                foreach (NPC item in Main.ActiveNPCs)
                {
                    if (item.Distance(Player.Center) <= radiusFire)
                    {
                        item.AddBuff(BuffID.OnFire, 300);
                    }
                }
                //Effects gained from Cloud Elemental (Movement Speed)
                Player.moveSpeed += movementSpeed;

                //Effects gained from Rare Sand Elemental (Healing orb)
                if (Pet.timer <= 0)
                {
                    Projectile.NewProjectileDirect(GlobalPet.GetSource_Pet(EntitySourcePetIDs.PetProjectile), Player.Center - Main.rand.NextVector2Circular(radiusHeal, radiusHeal), -Vector2.UnitY * 6f, ModContent.ProjectileType<CactusHealOrb>(), 0, 0, Player.whoAmI);
                    Pet.timer = Pet.timerMax;
                }
                //Effects Gained from Anahita (Water Elemental)
                GlobalPet.CircularDustEffect(Player.Center, DustID.Water, (int)radiusWater, dustAmount: 16);
                foreach (NPC item in Main.ActiveNPCs)
                {
                    if (item.Distance(Player.Center) <= radiusWater)
                    {
                        item.AddBuff(BuffID.Slow, 300);
                    }
                }

            }
        }
        //To check if Player is damaged for Sand Elemental, and applying Confused Debuff
        public override void OnHurt(Player.HurtInfo info)
        {
            if (PetIsEquipped())
            {
                GlobalPet.CircularDustEffect(Player.Center, DustID.Sand, (int)radiusStorm, dustAmount: 256);
                foreach (NPC item in Main.ActiveNPCs)
                {
                    if (item.Distance(Player.Center) <= radiusStorm)
                    {
                        item.AddBuff(BuffID.Confused, 300);
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
            public override string PetsTooltip => miniHeart.movementSpeed.ToString() + "<class>";
        }
    }
}
