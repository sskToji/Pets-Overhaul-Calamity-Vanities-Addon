using CalamityMod.BiomeManagers;
using CalValEX.Items.Pets;
using PetsOverhaul.Systems;
using POCalValAddon.Systems;
using Terraria;
using Terraria.ModLoader;

namespace POCalValAddon.PetEffects
{
    public sealed class George : PetEffect
    {
        public override int PetItemID => ModContent.ItemType<BubbleGum>();
        public override PetClasses PetClassPrimary => PetClasses.Defensive;
        public override PetClasses PetClassSecondary => PetClasses.Utility;

        public int georgeDef = 5;
        public int georgeAggro = 250;

        public override void PostUpdateMiscEffects()
        {
            if (PetIsEquipped() && (Player.InModBiome<SulphurousSeaBiome>() || Player.InModBiome<AbyssLayer1Biome>() || Player.InModBiome<AbyssLayer2Biome>() || Player.InModBiome<AbyssLayer3Biome>()))
            {
                Player.statDefense += georgeDef;
                Player.aggro -= georgeAggro;
            }
        }

        public sealed class GeorgePetItem : PetTooltip
        {
            public override PetEffect PetsEffect => george;
            public static George george
            {
                get
                {
                    if (Main.LocalPlayer.TryGetModPlayer(out George pet))
                        return pet;
                    else
                        return ModContent.GetInstance<George>();
                }
            }
            public override string PetsTooltip => PetUtil.LocVal("PetTooltips.George")
                .Replace("<def>", george.georgeDef.ToString())
                .Replace("<aggro>", george.georgeAggro.ToString());
            public override string SimpleTooltip => PetUtil.LocVal("SimplePetTooltips.George");
        }
    }
}
