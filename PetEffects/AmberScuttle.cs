using System.Collections.Generic;
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
    public sealed class AmberScuttle : PetEffect
    {
        public override int PetItemID => ModContent.ItemType<AmberGeode>();
        public override PetClasses PetClassPrimary => PetClasses.Defensive;
        public override PetClasses PetClassSecondary => PetClasses.Mining;

        public int defenseStat = 5;
        public int robeDef = 4;
        public int robeMana = 10;
        public int scuttleGemMult = 50;
        public float weaponDmg = 0.25f;
        public int amberMana = 70;

        public override void PostUpdateMiscEffects() //Defense increase from Scuttle
        {
            if (PetIsEquipped())
            {
                Player.statDefense += defenseStat;
            }
        }

        public override void ModifyWeaponDamage(Item item, ref StatModifier damage) //Buffs to equipment and changing tooltips of the items
        {
            if (PetIsEquipped() && (item.type == ItemID.AmberStaff || item.type == ItemID.OrangePhaseblade))
            {
                damage += weaponDmg;
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
                AmberScuttle ambScuttle = Main.LocalPlayer.GetModPlayer<AmberScuttle>();
                if (ambScuttle.PetIsEquipped())
                {
                    AmberScuttle amb = player.GetModPlayer<AmberScuttle>();
                    if (item.type == ItemID.AmberRobe)
                    {
                        player.statDefense += amb.robeDef;
                        player.statManaMax2 += amb.robeMana;
                    }
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
                        tooltips.Find(x => x.Name == "Tooltip0").Text = Language.GetTextValue("ItemTooltip.BandofStarpower").Replace("20", amb.amberMana.ToString());
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
            AmberScuttle ambgeode = player.GetModPlayer<AmberScuttle>();
            if (PickerPet.PickupChecks(item, ambgeode.PetItemID, out ItemPet itemChck) && itemChck.oreBoost && item.type == ItemID.Amber)
            {
                for (int i = 0; i < GlobalPet.Randomizer(ambgeode.scuttleGemMult * item.stack, 100); i++)
                {
                    player.QuickSpawnItem(GlobalPet.GetSource_Pet(EntitySourcePetIDs.MiningItem), item.type, 1);
                }
            }
        }

        public sealed class AmberScuttlePetItem : PetTooltip //Tooltip
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
            public override string PetsTooltip => PetUtil.LocVal("PetTooltips.Scuttlers.GenericScuttle")
                .Replace("<gem>", "Amber")
                .Replace("<color>", "Orange")
                .Replace("<def>", ambScuttle.defenseStat.ToString())
                .Replace("<dmg>", PetUtil.FloatToPercent(ambScuttle.weaponDmg))
                .Replace("<robeDef>", ambScuttle.robeDef.ToString())
                .Replace("<mana>", ambScuttle.robeMana.ToString())
                .Replace("<chance>", ambScuttle.scuttleGemMult.ToString() + "%");
            public override string SimpleTooltip => PetUtil.LocVal("SimplePetTooltips.Scuttlers.GenericScuttle")
                .Replace("<gem>", "Amber")
                .Replace("<color>", "Orange");
        }
    }
}

