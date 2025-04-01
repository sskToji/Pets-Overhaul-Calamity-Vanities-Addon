using System;
using PetsOverhaul;
using PetsOverhaul.Items;
using PetsOverhaul.Systems;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using CalValEX;
using CalValEX.Items.Pets.Scuttlers;
using System.Security.Cryptography.X509Certificates;

/*
    Diamond Scuttle Pet
    Increases Defense Stat
    Increases Damage done with "Diamond type items"
    Increases droprate of Diamonds from Diamond Ore
*/

namespace POCalValAddon.Content.PetEffects
{
    public sealed class DiamondScuttle : PetEffect
    {
        public override int PetItemID => ModContent.ItemType<DiamondGeode>();
        public override PetClasses PetClassPrimary => PetClasses.Defensive;
        public override PetClasses PetClassSecondary => PetClasses.Utility;

        public int defenseStat = 10;
        public override void PostUpdateMiscEffects()
        {
            if (PetIsEquipped())
            {
                //Defense increase from DiamondScuttle
                Player.statDefense += defenseStat;
            }
        }

        //Increase in Droprate of Diamonds (see https://github.com/BugraPearls/Pets-Overhaul/blob/master/PetEffects/BabyDinosaur.cs)
        public override void Load()
        {
            PetsOverhaul.PetsOverhaul.OnPickupActions += PreOnPickup;
        }
                
        public static void PreOnPickup(Item item, Player player)
        {
            GlobalPet Pet = player.GetModPlayer<GlobalPet>();
        }
    }
}
