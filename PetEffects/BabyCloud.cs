using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetsOverhaul.Systems;
using PetsOverhaul;
using PetsOverhaul.Items;
using Terraria.ModLoader;
using CalValEX.Items.Pets.Elementals;
using Terraria;
using Terraria.Localization;

namespace POCalValAddon.PetEffects
{
    public sealed class BabyCloud : PetEffect
    {
        public override int PetItemID => ModContent.ItemType<CloudCandy>();
        public override PetClasses PetClassPrimary => PetClasses.Utility;

        public float movementSpeed = 0.20f;

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
            public override string PetsTooltip => Language.GetTextValue("Mods.POCalValAddon.PetTooltips.Elementals.BabyCloud");
        }
    }
}
