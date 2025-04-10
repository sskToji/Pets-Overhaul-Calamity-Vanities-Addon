using System;
using System.Collections.Generic;
using PetsOverhaul.Items;
using PetsOverhaul.Systems;
using PetsOverhaul.PetEffects;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using CalValEX;
using CalValEX.Items.Pets;

//Idea by @iamnamedmuffin

namespace POCalValAddon.PetEffects
{
    public sealed class AeroSlime : PetEffect
    {
        public override int PetItemID => ModContent.ItemType<AerialiteBubble>();
        public override PetClasses PetClassPrimary => PetClasses.Utility;

        public override void PostUpdateMiscEffects()
        {
            if (PetIsEquipped())
            {
                Player.wingTimeMax += (int)(Player.wingTimeMax * 0.1);
                Player.AddBuff(BuffID.Featherfall, 1);
            }
        }

        public sealed class AeroSlimePetItem : PetTooltip
        {
            public override PetEffect PetsEffect => aeroSlime;
            public static AeroSlime aeroSlime
            {
                get
                {
                    if (Main.LocalPlayer.TryGetModPlayer(out AeroSlime pet))
                        return pet;
                    else 
                        return ModContent.GetInstance<AeroSlime>();
                }
            }
        public override string PetsTooltip => Language.GetTextValue("Mods.POCalValAddon.PetTooltips.AeroSlime");
        }
    }
}
