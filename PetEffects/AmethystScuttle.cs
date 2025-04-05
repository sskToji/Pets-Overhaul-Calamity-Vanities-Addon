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
    public sealed class AmethystScuttle : PetEffect
    {
        public override int PetItemID => ModContent.ItemType<AmethystGeode>();
        public override PetClasses PetClassPrimary => PetClasses.Defensive;
        public override PetClasses PetClassSecondary => PetClasses.Mining;

        public int defenseStat = 10;
        public int robeDef = 2;
        public int robeMana = 20;
        public int scuttleGemMult = 500;
        public float staffDmg = 0.25f;

        public override void PostUpdateMiscEffects()
        {
            if (PetIsEquipped())
            {
                //Defense increase from Scuttle
                Player.statDefense += defenseStat;
            }
        }

        //Buffs to equipment and changing tooltips of the items
        public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
        {
            if (PetIsEquipped() && item.type == ItemID.AmethystStaff)
            {
                damage += staffDmg;
            }
        }
        public sealed class AmeScuttleArmor : GlobalItem
        {
            public override bool AppliesToEntity(Item entity, bool lateInstantation)
            {
                return entity.type == ItemID.AmethystRobe;
            }
            public override void UpdateEquip(Item item, Player player)
            {
                AmethystScuttle ame = player.GetModPlayer<AmethystScuttle>();
                if (item.type == ItemID.AmethystRobe)
                {
                    player.statDefense += ame.robeDef;
                    player.statManaMax2 += ame.robeMana;
                }
            }
            public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
            {
                AmethystScuttle ame = Main.LocalPlayer.GetModPlayer<AmethystScuttle>();
                if (ame.PetIsEquipped())
                {
                    int def = item.defense;
                    if (item.type == ItemID.AmethystRobe)
                    {
                        def = ame.robeDef + item.defense;
                    }
                    if (tooltips.Find(x => x.Name == "Defense") != null)
                        tooltips.Find(x => x.Name == "Defense").Text = def.ToString() + " defense";

                    if (tooltips.Find(x => x.Name == "Tooltip0") != null)
                        tooltips.Find(x => x.Name == "Tooltip0").Text = Language.GetTextValue("Mods.POCalValAddon.ItemTooltips.AmethystRobe");
                }
            }
        }

        //Increase in Droprate of Gemtype
        public override void Load()
        {
            PetsOverhaul.PetsOverhaul.OnPickupActions += PreOnPickup;
        }

        public static void PreOnPickup(Item item, Player player)
        {
            GlobalPet PickerPet = player.GetModPlayer<GlobalPet>();
            AmethystScuttle amegeode = player.GetModPlayer<AmethystScuttle>();
            if (PickerPet.PickupChecks(item, amegeode.PetItemID, out ItemPet itemChck) && itemChck.oreBoost && item.type == ItemID.Amber)
            {
                for (int i = 0; i < GlobalPet.Randomizer(amegeode.scuttleGemMult * item.stack, 1000); i++)
                {
                    player.QuickSpawnItem(GlobalPet.GetSource_Pet(EntitySourcePetIDs.MiningItem), item.type, 1);
                }
            }
        }
        //Tooltip
        public sealed class AmethystScuttlePetItem : PetTooltip
        {
            public override PetEffect PetsEffect => ameScuttle;
            public static AmethystScuttle ameScuttle
            {
                get
                {
                    if (Main.LocalPlayer.TryGetModPlayer(out AmethystScuttle pet))
                        return pet;
                    else
                        return ModContent.GetInstance<AmethystScuttle>();
                }
            }
            public override string PetsTooltip => Language.GetTextValue("Mods.POCalValAddon.PetTooltips.Scuttlers.AmethystScuttle");
        }
    }
}
