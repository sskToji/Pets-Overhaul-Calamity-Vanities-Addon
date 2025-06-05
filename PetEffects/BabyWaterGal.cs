using CalValEX.Items.Pets.Elementals;
using PetsOverhaul.Systems;
using POCalValAddon.Systems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace POCalValAddon.PetEffects
{
    public sealed class BabyWaterGal : PetEffect
    {
        public override int PetItemID => ModContent.ItemType<StrangeMusicNote>();
        public override PetClasses PetClassPrimary => PetClasses.Utility;

        public float radiusWater = 240f;
        public int debuffTime = 300;

        public override void PostUpdateMiscEffects()
        {
            if (PetIsEquipped())
            {
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

        public sealed class BabyWaterGalPetItem : PetTooltip
        {
            public override PetEffect PetsEffect => babyWater;
            public static BabyWaterGal babyWater
            {
                get
                {
                    if (Main.LocalPlayer.TryGetModPlayer(out BabyWaterGal pet))
                        return pet;
                    else
                        return ModContent.GetInstance<BabyWaterGal>();
                }
            }
            public override string PetsTooltip => PetUtil.LocVal("PetTooltips.Elementals.BabyWaterGal")
                .Replace("<px>", babyWater.radiusWater.ToString())
                .Replace("<secs>", PetUtil.IntToTime(babyWater.debuffTime));
            public override string SimpleTooltip => PetUtil.LocVal("SimplePetTooltips.Elementals.BabyWaterGal");
        }
    }
}
