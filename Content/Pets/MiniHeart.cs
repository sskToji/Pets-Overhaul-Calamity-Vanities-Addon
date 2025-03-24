using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetsOverhaul.Systems;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using CalValEX;
using CalValEX.Items.Pets.Elementals;
using CalamityMod;
using CalamityMod.Dusts;
using CalamityMod.Projectiles.Summon;
using CalamityMod.Projectiles.Healing;
using Microsoft.Xna.Framework;
using CalamityMod.Buffs.Summon;
using CalamityMod.Items.Accessories;

namespace POCalValAddon.Content.Pets
{
    public class pet : PetEffect
    {
        public float radiusfire = 160f;
        public float radiusheal = 480f;
        public float radiusstorm = 240f;
        public float radiuswater = 240f;
        public float movementSpeed = 0.25f;

        public override PetClasses PetClassPrimary => PetClasses.Ranged;
        public override int PetAbilityCooldown => 600;
        public override int PetItemID => ModContent.ItemType<MiniatureElementalHeart>();
        public override void PostUpdateMiscEffects()
        {
            if (PetIsEquipped())
            {
                //Effects and visuals gained from Brimstone Elemental (Circle of Fire)
                GlobalPet.CircularDustEffect(Player.Center, ModContent.DustType<CalamityMod.Dusts.BrimstoneFlame>(), (int)radiusfire, dustAmount: 64);
                GlobalPet.CircularDustEffect(Player.Center, DustID.Torch, (int)radiusfire, dustAmount: 32);
                foreach (NPC item in Main.ActiveNPCs)
                {
                    if (item.Distance(Player.Center) <= radiusfire)
                    {
                        item.AddBuff(BuffID.OnFire, 300);
                    }
                }
                //Effects gained from Cloud Elemental (Movement Speed)
                Player.moveSpeed += movementSpeed;

                //Effects gained from Rare Sand Elemental (Healing orb)
                if (Pet.timer <= 0)
                {
                    Projectile.NewProjectileDirect(GlobalPet.GetSource_Pet(EntitySourcePetIDs.PetProjectile), Player.Center - Main.rand.NextVector2Circular(radiusheal, radiusheal), -Vector2.UnitY * 6f, ModContent.ProjectileType<CactusHealOrb>(), 0, 0, Player.whoAmI);
                    Pet.timer = Pet.timerMax;
                }
                //Effects Gained from Anahita (Water Elemental)
                GlobalPet.CircularDustEffect(Player.Center, DustID.Water, (int)radiuswater, dustAmount: 16);
                foreach (NPC item in Main.ActiveNPCs)
                {
                    if (item.Distance(Player.Center) <= radiuswater)
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
                GlobalPet.CircularDustEffect(Player.Center, DustID.Sand, (int)radiusstorm, dustAmount: 256);
                foreach (NPC item in Main.ActiveNPCs)
                {
                    if (item.Distance(Player.Center) <= radiusstorm)
                    {
                        item.AddBuff(BuffID.Confused, 300);
                    }
                }
            }
        }
    }

    public class tooltip : PetTooltip
    {
        public override string PetsTooltip => MiniHeart.movementSpeed.ToString() + "<class>";
        public override PetEffect PetsEffect => MiniHeart;

        public static pet MiniHeart
        {
            get
            {
                if (Main.LocalPlayer.TryGetModPlayer(out pet pet))
                    return pet;
                else
                    return ModContent.GetInstance<pet>();
            }
        }
    }
}
