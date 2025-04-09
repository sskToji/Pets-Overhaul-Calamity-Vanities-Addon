using System;
using Microsoft.Xna.Framework;
using Steamworks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using PetsOverhaul.Systems;
using CalValEX.Items.Pets.Elementals;
using CalamityMod.Projectiles.Healing;

namespace POCalValAddon.PetEffects
{
    public sealed class BabyRareDusty : PetEffect
    {
        public override int PetItemID => ModContent.ItemType<SmallSandPlushie>();
        public override PetClasses PetClassPrimary => PetClasses.Utility;

        public int rareDustyCooldown = 600;
        public float radiusHeal = 480f;
        public override int PetAbilityCooldown => rareDustyCooldown;

        public override void PostUpdateMiscEffects()
        {
            if (PetIsEquipped())
            {
                if (Pet.timer <= 0)
                {
                    Projectile.NewProjectileDirect(GlobalPet.GetSource_Pet(EntitySourcePetIDs.PetProjectile), Player.Center - Main.rand.NextVector2Circular(radiusHeal, radiusHeal), -Vector2.UnitY * 6f, ModContent.ProjectileType<CactusHealOrb>(), 0, 0, Player.whoAmI);
                    Pet.timer = Pet.timerMax;
                }
            }
        }
        public sealed class BabyRareDustyPetItem : PetTooltip
        {
            public override PetEffect PetsEffect => babyRare;
            public static BabyRareDusty babyRare
            {
                get
                {
                    if (Main.LocalPlayer.TryGetModPlayer(out BabyRareDusty pet))
                        return pet;
                    else
                        return ModContent.GetInstance<BabyRareDusty>();
                }
            }
            public override string PetsTooltip => Language.GetTextValue("Mods.POCalValAddon.PetTooltips.Elementals.BabyRareDusty");
        }
    }
}
