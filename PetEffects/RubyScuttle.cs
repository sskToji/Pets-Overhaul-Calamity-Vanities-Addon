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
    public sealed class RubyScuttle : PetEffect
    {
        public override int PetItemID => ModContent.ItemType<RubyGeode>();
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
            if (PetIsEquipped() && item.type == ItemID.RubyStaff)
            {
                damage += staffDmg;
            }
        }
        public sealed class RubScuttleArmor : GlobalItem
        {
            public override bool AppliesToEntity(Item entity, bool lateInstantation)
            {
                return entity.type == ItemID.RubyRobe;
            }
            public override void UpdateEquip(Item item, Player player)
            {
                RubyScuttle rub = player.GetModPlayer<RubyScuttle>();
                if (item.type == ItemID.RubyRobe)
                {
                    player.statDefense += rub.robeDef;
                    player.statManaMax2 += rub.robeMana;
                }
            }
            public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
            {
                RubyScuttle rub = Main.LocalPlayer.GetModPlayer<RubyScuttle>();
                if (rub.PetIsEquipped())
                {
                    int def = item.defense;
                    if (item.type == ItemID.RubyRobe)
                    {
                        def = rub.robeDef + item.defense;
                    }
                    if (tooltips.Find(x => x.Name == "Defense") != null)
                        tooltips.Find(x => x.Name == "Defense").Text = def.ToString() + " defense";

                    if (tooltips.Find(x => x.Name == "Tooltip0") != null)
                        tooltips.Find(x => x.Name == "Tooltip0").Text = Language.GetTextValue("Mods.POCalValAddon.ItemTooltips.RubyRobe");
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
            RubyScuttle rubgeode = player.GetModPlayer<RubyScuttle>();
            if (PickerPet.PickupChecks(item, rubgeode.PetItemID, out ItemPet itemChck) && itemChck.oreBoost && item.type == ItemID.Ruby)
            {
                for (int i = 0; i < GlobalPet.Randomizer(rubgeode.scuttleGemMult * item.stack, 1000); i++)
                {
                    player.QuickSpawnItem(GlobalPet.GetSource_Pet(EntitySourcePetIDs.MiningItem), item.type, 1);
                }
            }
        }
        //Tooltip
        public sealed class RubyScuttlePetItem : PetTooltip
        {
            public override PetEffect PetsEffect => rubScuttle;
            public static RubyScuttle rubScuttle
            {
                get
                {
                    if (Main.LocalPlayer.TryGetModPlayer(out RubyScuttle pet))
                        return pet;
                    else
                        return ModContent.GetInstance<RubyScuttle>();
                }
            }
            public override string PetsTooltip => Language.GetTextValue("Mods.POCalValAddon.PetTooltips.Scuttlers.RubyScuttle");
        }
    }
}
