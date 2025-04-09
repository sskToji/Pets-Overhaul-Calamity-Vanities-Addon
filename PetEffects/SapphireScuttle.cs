using System;
using System.Collections.Generic;
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
using PetsOverhaul.PetEffects;

namespace POCalValAddon.PetEffects
{
    public sealed class SapphireScuttle : PetEffect
    {
        public override int PetItemID => ModContent.ItemType<SapphireGeode>();
        public override PetClasses PetClassPrimary => PetClasses.Defensive;
        public override PetClasses PetClassSecondary => PetClasses.Mining;

        public int defenseStat = 5;
        public int robeDef = 3;
        public int robeMana = 10;
        public int scuttleGemMult = 500;
        public float weaponDmg = 0.25f;
        public int sapphireMana = 50;

        public override void PostUpdateMiscEffects() //Defense increase from Scuttle
        {
            if (PetIsEquipped())
            {
                Player.statDefense += defenseStat;
            }
        }

        public override void ModifyWeaponDamage(Item item, ref StatModifier damage) //Buffs to equipment and changing tooltips of the items
        {
            if (PetIsEquipped() && (item.type == ItemID.SapphireStaff || item.type == ItemID.BluePhaseblade))
            {
                damage += weaponDmg;
            }
        }
        public sealed class SapScuttleArmor : GlobalItem
        {
            public override bool AppliesToEntity(Item entity, bool lateInstantation)
            {
                return entity.type == ItemID.SapphireRobe;
            }
            public override void UpdateEquip(Item item, Player player)
            {
                SapphireScuttle sapScuttle = Main.LocalPlayer.GetModPlayer<SapphireScuttle>();
                if (sapScuttle.PetIsEquipped())
                {
                    SapphireScuttle sap = player.GetModPlayer<SapphireScuttle>();
                    if (item.type == ItemID.SapphireRobe)
                    {
                        player.statDefense += sap.robeDef;
                        player.statManaMax2 += sap.robeMana;
                    }
                }
            }
            public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
            {
                SapphireScuttle sap = Main.LocalPlayer.GetModPlayer<SapphireScuttle>();
                if (sap.PetIsEquipped())
                {
                    int def = item.defense;
                    if (item.type == ItemID.SapphireRobe)
                    {
                        def = sap.robeDef + item.defense;
                    }
                    if (tooltips.Find(x => x.Name == "Defense") != null)
                        tooltips.Find(x => x.Name == "Defense").Text = def.ToString() + " defense";

                    if (tooltips.Find(x => x.Name == "Tooltip0") != null)
                        tooltips.Find(x => x.Name == "Tooltip0").Text = Language.GetTextValue("ItemTooltip.BandofStarpower").Replace("20", sap.sapphireMana.ToString());
                }
            }
        }
        
        public override void Load() //Increase in Droprate of Gemtype
        {
            PetsOverhaul.PetsOverhaul.OnPickupActions += PreOnPickup;
        }

        public static void PreOnPickup(Item item, Player player)
        {
            GlobalPet PickerPet = player.GetModPlayer<GlobalPet>();
            SapphireScuttle sapgeode = player.GetModPlayer<SapphireScuttle>();
            if (PickerPet.PickupChecks(item, sapgeode.PetItemID, out ItemPet itemChck) && itemChck.oreBoost && item.type == ItemID.Sapphire)
            {
                for (int i = 0; i < GlobalPet.Randomizer(sapgeode.scuttleGemMult * item.stack, 1000); i++)
                {
                    player.QuickSpawnItem(GlobalPet.GetSource_Pet(EntitySourcePetIDs.MiningItem), item.type, 1);
                }
            }
        }
        
        public sealed class SapphireScuttlePetitem : PetTooltip //Tooltip
        {
            public override PetEffect PetsEffect => sapScuttle;
            public static SapphireScuttle sapScuttle
            {
                get
                {
                    if (Main.LocalPlayer.TryGetModPlayer(out SapphireScuttle pet))
                        return pet;
                    else
                        return ModContent.GetInstance<SapphireScuttle>();
                }
            }
            public override string PetsTooltip => Language.GetTextValue("Mods.POCalValAddon.PetTooltips.Scuttlers.SapphireScuttle");
        }
    }
}
