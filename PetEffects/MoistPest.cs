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
using Terraria.GameInput;
using CalamityMod;
using CalamityMod.Projectiles.Ranged;
using CalamityMod.Projectiles.Healing;
using CalamityMod.Projectiles.Melee;
using CalamityMod.BiomeManagers;

namespace POCalValAddon.PetEffects
{
    public sealed class MoistPest : PetEffect
    {
        public override int PetItemID => ModContent.ItemType<MoistLocket>();
        public override PetClasses PetClassPrimary => PetClasses.Offensive;
        public override PetClasses PetClassSecondary => PetClasses.Mobility;

        public int moistLocketCooldown = 300;
        public float radiusMoist = 20f;
        public int moistDmg = 100;
        public float oceanMovement = 0.20f;
        public override int PetAbilityCooldown => moistLocketCooldown;

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (Pet.AbilityPressCheck() && PetIsEquipped())
            {
                if (Pet.timer <= 0)
                {
                    Projectile.NewProjectileDirect(GlobalPet.GetSource_Pet(EntitySourcePetIDs.PetProjectile), Player.Center - Main.rand.NextVector2Circular(radiusMoist, radiusMoist), -Vector2.UnitY * 6f, ModContent.ProjectileType<UrchinSpikeFugu>(), 0, 0, Player.whoAmI);
                    Pet.timer = Pet.timerMax;
                }
            }
        }
        public override void PostUpdateMiscEffects()
        {
            if (PetIsEquipped() && Player.ZoneBeach)
            {
                Player.moveSpeed += oceanMovement;
            }
        }

        public sealed class MoistPestPetItem : PetTooltip
        {
            public override PetEffect PetsEffect => babyMoist;
            public static MoistPest babyMoist
            {
                get
                {
                    if (Main.LocalPlayer.TryGetModPlayer(out MoistPest pet))
                        return pet;
                    else
                        return ModContent.GetInstance<MoistPest>();
                }
            }
            public override string PetsTooltip => Language.GetTextValue("Mods.POCalValAddon.PetTooltips.MoistPest");
        }
    }
}
