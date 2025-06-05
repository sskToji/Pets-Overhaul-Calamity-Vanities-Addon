using CalValEX.Items.Pets.Elementals;
using PetsOverhaul.Systems;
using POCalValAddon.Systems;
using Terraria;
using Terraria.ModLoader;

namespace POCalValAddon.PetEffects
{
    public sealed class BabyCloud : PetEffect
    {
        public override int PetItemID => ModContent.ItemType<CloudCandy>();
        public override PetClasses PetClassPrimary => PetClasses.Utility;

        public float movementSpeed = 0.2f;

        public override void PostUpdateMiscEffects()
        {
            if (PetIsEquipped())
            {
                Player.moveSpeed += movementSpeed;
            }
        }

        public sealed class BabyCloudPetItem : PetTooltip //Tooltip
        {
            public override PetEffect PetsEffect => babyCloud;
            public static BabyCloud babyCloud
            {
                get
                {
                    if (Main.LocalPlayer.TryGetModPlayer(out BabyCloud pet))
                        return pet;
                    else
                        return ModContent.GetInstance<BabyCloud>();
                }
            }
            public override string PetsTooltip => PetUtil.LocVal("PetTooltips.Elementals.BabyCloud")
                .Replace("<speed>", PetUtil.FloatToPercent(babyCloud.movementSpeed));
            public override string SimpleTooltip => PetUtil.LocVal("SimplePetTooltips.Elementals.BabyCloud");
        }
    }
}
