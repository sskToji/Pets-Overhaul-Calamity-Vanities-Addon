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
using POCalValAddon.Systems;
using System.Collections.Generic;

/*
    <summary>
    Diamond Scuttle Pet
    Increases Defense Stat
    Increases Damage done with "Diamond type items"
    Increases chance to gain an extra diamond by 50%
    </summary>
*/

namespace POCalValAddon.PetEffects
{
    public sealed class DiamondScuttle : PetEffect
    {
        public override int PetItemID => ModContent.ItemType<DiamondGeode>();
        public override PetClasses PetClassPrimary => PetClasses.Defensive;
        public override PetClasses PetClassSecondary => PetClasses.Mining;

        public int defenseStat = 10;
        public float staffDmg = 0.25f;
        public int robeDef = 5;
        public int robeMana = 20;
        public static List<int> diamondItems = [ItemID.DiamondStaff, ItemID.DiamondRobe];
        public override void PostUpdateMiscEffects()
        {
            if (PetIsEquipped())
            {
                //Defense increase from DiamondScuttle
                Player.statDefense += defenseStat;
            }
        }

        //Damage increase to Diamond Staff if Diamond Scuttle is equipped
        public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
        {
            if (PetIsEquipped() && diamondItems.Contains(item.type))
            {
                damage += staffDmg;
            }
        }

        /* <Commented out because not working/done>
        public int chance = 1000; //this is a 50% boost, standard is 1000
        public static void AddItemsToPool()
        {
            GlobalPet.ItemWeight(ItemID.StoneBlock, 100);
        }

        //Increase in Droprate of Diamonds(see https://github.com/BugraPearls/Pets-Overhaul/blob/master/PetEffects/BabyDinosaur.cs)

        public override void Load()
        {
            PetsOverhaul.PetsOverhaul.OnPickupActions += PreOnPickup;
        }

        public static void PreOnPickup(Item item, Player player)
        {
            GlobalPet Pet = player.GetModPlayer<GlobalPet>();
            DiamondScuttle diaScuttle = player.GetModPlayer<DiamondScuttle>();
            if (Pet.PickupChecks(item, diaScuttle.PetItemID, out ItemPet itemChck) && itemChck.oreBoost)
            {
                AddItemsToPool();
                if (GlobalPet.ItemPool.Count > 0)
                {
                    for (int i = 0; i < GlobalPet.Randomizer(diaScuttle.chance * item.stack, 1000); i++)
                    {
                        player.QuickSpawnItem(GlobalPet.GetSource_Pet(EntitySourcePetIDs.MiningItem), GlobalPet.ItemPool[Main.rand.Next(GlobalPet.ItemPool.Count)], 1);
                    }
                }
            }
        } 
        */
    }
}
