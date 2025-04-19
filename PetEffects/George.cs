using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalamityMod.BiomeManagers;
using CalamityMod.World;
using CalValEX.Items.Pets;
using PetsOverhaul.Systems;
using Terraria;
using Terraria.Localization;
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
            public override string PetsTooltip => Language.GetTextValue("Mods.POCalValAddon.PetTooltips.George");
        }
    }
}
