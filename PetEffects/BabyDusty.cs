using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using PetsOverhaul.Systems;
using CalValEX.Items.Pets.Elementals;


namespace POCalValAddon.PetEffects
{
    public sealed class BabyDusty : PetEffect
    {
        public override int PetItemID => ModContent.ItemType<SmallSandPail>();
        public override PetClasses PetClassPrimary => PetClasses.Utility;

        public float radiusStorm = 240f;

        public override void OnHurt(Player.HurtInfo info)
        {
            if (PetIsEquipped())
            {
                GlobalPet.CircularDustEffect(Player.Center, DustID.Sand, (int)radiusStorm, dustAmount: 256);
                foreach (NPC item in Main.ActiveNPCs)
                {
                    if (item.Distance(Player.Center) <= radiusStorm)
                    {
                        item.AddBuff(BuffID.Confused, 300);
                    }
                }
            }
        }
        public sealed class BabyDustyPetItem : PetTooltip //Tooltip
        {
            public override PetEffect PetsEffect => babyDusty;
            public static BabyDusty babyDusty
            {
                get
                {
                    if (Main.LocalPlayer.TryGetModPlayer(out BabyDusty pet))
                        return pet;
                    else
                        return ModContent.GetInstance<BabyDusty>();
                }
            }
            public override string PetsTooltip => Language.GetTextValue("Mods.POCalValAddon.PetTooltips.Elementals.BabyDusty");
        }
    }
}
