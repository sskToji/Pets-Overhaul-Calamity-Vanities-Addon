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
    public sealed class AmberScuttle : PetEffect
    {
        public override int PetItemID => ModContent.ItemType<AmberGeode>();
        public override PetClasses PetClassPrimary => PetClasses.Defensive;
        public override PetClasses PetClassSecondary => PetClasses.Mining;

        public int defenseStat = 10;
        public int robeDef = 4;
        public int robeMana = 10;
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
            if (PetIsEquipped() && item.type == ItemID.AmberStaff)
            {
                damage += staffDmg;
            }
        }
        public sealed class AmbScuttleArmor : GlobalItem
        {
            public override bool AppliesToEntity(Item entity, bool lateInstantation)
            {
                return entity.type == ItemID.AmberRobe;
            }
            public override void UpdateEquip(Item item, Player player)
            {
                AmberScuttle amb = player.GetModPlayer<AmberScuttle>();
                if (item.type == ItemID.AmberRobe)
                {
                    player.statDefense += amb.robeDef;
                    player.statManaMax2 += amb.robeMana;
                }
            }
            public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
            {
                AmberScuttle amb = Main.LocalPlayer.GetModPlayer<AmberScuttle>();
                if (amb.PetIsEquipped())
                {
                    int def = item.defense;
                    if (item.type == ItemID.AmberRobe)
                    {
                        def = amb.robeDef + item.defense;
                    }
                    if (tooltips.Find(x => x.Name == "Defense") != null)
                        tooltips.Find(x => x.Name == "Defense").Text = def.ToString() + " defense";

                    if (tooltips.Find(x => x.Name == "Tooltip0") != null)
                        tooltips.Find(x => x.Name == "Tooltip0").Text = Language.GetTextValue("Mods.POCalValAddon.ItemTooltips.AmberRobe");
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
            AmberScuttle ambgeode = player.GetModPlayer<AmberScuttle>();
            if (PickerPet.PickupChecks(item, ambgeode.PetItemID, out ItemPet itemChck) && itemChck.oreBoost && item.type == ItemID.Amber)
            {
                for (int i = 0; i < GlobalPet.Randomizer(ambgeode.scuttleGemMult * item.stack, 1000); i++)
                {
                    player.QuickSpawnItem(GlobalPet.GetSource_Pet(EntitySourcePetIDs.MiningItem), item.type, 1);
                }
            }
        }
        //Tooltip
        public sealed class AmberScuttlePetItem : PetTooltip
        {
            public override PetEffect PetsEffect => ambScuttle;
            public static AmberScuttle ambScuttle
            {
                get
                {
                    if (Main.LocalPlayer.TryGetModPlayer(out AmberScuttle pet))
                        return pet;
                    else
                        return ModContent.GetInstance<AmberScuttle>();
                }
            }
            public override string PetsTooltip => Language.GetTextValue("Mods.POCalValAddon.PetTooltips.Scuttlers.AmberScuttle");
        }
    }
}

