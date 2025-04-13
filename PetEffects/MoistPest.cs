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

        public int moistCooldown = 300;
        public float moistRadius = 20f;
        public int moistDmg = 100; //currently unused, is meant to be damage from particles
        public float beachMovement = 0.20f;
        public override int PetAbilityCooldown => moistCooldown;

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (Pet.AbilityPressCheck() && PetIsEquipped())
            {
                if (Pet.timer <= 0)
                {
                    for (int i = 0; i < 25; i++)
                    {
                        Projectile petProjectile = Projectile.NewProjectileDirect(GlobalPet.GetSource_Pet(EntitySourcePetIDs.PetProjectile), Player.Center + Main.rand.NextVector2CircularEdge(Player.width, Player.height), Main.rand.NextVector2CircularEdge(10, 10), ModContent.ProjectileType<CalamityMod.Projectiles.Boss.WaterSpear>(), Pet.PetDamage(moistDmg, DamageClass.Melee), 0, Player.whoAmI);
                        petProjectile.DamageType = DamageClass.Melee;
                        petProjectile.CritChance = (int)Player.GetTotalCritChance<GenericDamageClass>();
                        petProjectile.friendly = true;
                        petProjectile.hostile = false;
                    }
                    Pet.timer = Pet.timerMax;
                }
            }
        }
        public override void PostUpdateMiscEffects()
        {
            if (PetIsEquipped() && Player.ZoneBeach)
            {
                Player.moveSpeed += beachMovement;
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
            public override string PetsTooltip => Language.GetTextValue("Mods.POCalValAddon.PetTooltips.MoistPest")
                .Replace("<keybind>", PetTextsColors.KeybindText(PetKeybinds.UsePetAbility));
        }
    }
}
