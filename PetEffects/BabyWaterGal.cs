using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Steamworks;
using Terraria.ModLoader;
using PetsOverhaul.Systems;
using CalValEX.Items.Pets.Elementals;
using Terraria.ID;
using Terraria;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Terraria.Localization;
using Terraria.WorldBuilding;

namespace POCalValAddon.PetEffects
{
    public sealed class BabyWaterGal : PetEffect
    {
        public override int PetItemID => ModContent.ItemType<StrangeMusicNote>();
        public override PetClasses PetClassPrimary => PetClasses.Utility;

        public float radiusWater = 240f;

        public override void PostUpdateMiscEffects()
        {
            if (PetIsEquipped())
            {
                GlobalPet.CircularDustEffect(Player.Center, DustID.Water, (int)radiusWater, dustAmount: 32);
                foreach (NPC item in Main.ActiveNPCs)
                {
                    if (item.Distance(Player.Center) <= radiusWater)
                    {
                        item.AddBuff(BuffID.Slow, 300);
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
            public override string PetsTooltip => Language.GetTextValue("Mods.POCalValAddon.PetTooltips.Elementals.BabyWaterGal");
        }
    }
}
