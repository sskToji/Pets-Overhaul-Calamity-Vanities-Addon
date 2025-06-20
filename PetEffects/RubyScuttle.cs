﻿using System.Collections.Generic;
using CalValEX.Items.Pets.Scuttlers;
using PetsOverhaul.Items;
using PetsOverhaul.Systems;
using POCalValAddon.Systems;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace POCalValAddon.PetEffects
{
    public sealed class RubyScuttle : PetEffect
    {
        public override int PetItemID => ModContent.ItemType<RubyGeode>();
        public override PetClasses PetClassPrimary => PetClasses.Defensive;
        public override PetClasses PetClassSecondary => PetClasses.Mining;

        public int defenseStat = 5;
        public int robeDef = 4;
        public int robeMana = 10;
        public int scuttleGemMult = 50;
        public float weaponDmg = 0.25f;
        public int rubyMana = 70;

        public override void PostUpdateMiscEffects()
        {
            if (PetIsEquipped())
            {
                Player.statDefense += defenseStat;
            }
        }

        public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
        {
            if (PetIsEquipped() && (item.type == ItemID.RubyStaff || item.type == ItemID.RedPhaseblade))
            {
                damage += weaponDmg;
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
                RubyScuttle rubScuttle = Main.LocalPlayer.GetModPlayer<RubyScuttle>();
                if (rubScuttle.PetIsEquipped())
                {
                    RubyScuttle rub = player.GetModPlayer<RubyScuttle>();
                    if (item.type == ItemID.RubyRobe)
                    {
                        player.statDefense += rub.robeDef;
                        player.statManaMax2 += rub.robeMana;
                    }
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
                        tooltips.Find(x => x.Name == "Tooltip0").Text = Language.GetTextValue("ItemTooltip.BandofStarpower").Replace("20", rub.rubyMana.ToString());
                }
            }
        }

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
                for (int i = 0; i < GlobalPet.Randomizer(rubgeode.scuttleGemMult * item.stack, 100); i++)
                {
                    player.QuickSpawnItem(GlobalPet.GetSource_Pet(EntitySourcePetIDs.MiningItem), item.type, 1);
                }
            }
        }

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
            public override string PetsTooltip => PetUtil.LocVal("PetTooltips.Scuttlers.GenericScuttle")
                .Replace("<gem>", "Ruby")
                .Replace("<color>", "Red")
                .Replace("<def>", rubScuttle.defenseStat.ToString())
                .Replace("<dmg>", PetUtil.FloatToPercent(rubScuttle.weaponDmg))
                .Replace("<robeDef>", rubScuttle.robeDef.ToString())
                .Replace("<mana>", rubScuttle.robeMana.ToString())
                .Replace("<chance>", rubScuttle.scuttleGemMult.ToString() + "%");
            public override string SimpleTooltip => PetUtil.LocVal("SimplePetTooltips.Scuttlers.GenericScuttle")
                .Replace("<gem>", "Ruby")
                .Replace("<color>", "Red");
        }
    }
}
