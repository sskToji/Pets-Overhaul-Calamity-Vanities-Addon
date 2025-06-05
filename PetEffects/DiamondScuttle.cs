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
    public sealed class DiamondScuttle : PetEffect
    {
        public override int PetItemID => ModContent.ItemType<DiamondGeode>();
        public override PetClasses PetClassPrimary => PetClasses.Defensive;
        public override PetClasses PetClassSecondary => PetClasses.Mining;

        public int defenseStat = 5;
        public int robeDef = 5;
        public int robeMana = 20;
        public int scuttleGemMult = 50;
        public float weaponDmg = 0.25f;
        public int diamondMana = 80;

        public override void PostUpdateMiscEffects() //Defense increase from Scuttle
        {
            if (PetIsEquipped())
            {
                Player.statDefense += defenseStat;
            }
        }

        public override void ModifyWeaponDamage(Item item, ref StatModifier damage) //Buffs to equipment and changing tooltips of the items
        {
            if (PetIsEquipped() && (item.type == ItemID.DiamondStaff || item.type == ItemID.WhitePhaseblade))
            {
                damage += weaponDmg;
            }
        }
        public sealed class DiaScuttleArmor : GlobalItem
        {
            public override bool AppliesToEntity(Item entity, bool lateInstantation)
            {
                return entity.type == ItemID.DiamondRobe;
            }
            public override void UpdateEquip(Item item, Player player)
            {
                DiamondScuttle diaScuttle = Main.LocalPlayer.GetModPlayer<DiamondScuttle>();
                if (diaScuttle.PetIsEquipped())
                {
                    DiamondScuttle dia = player.GetModPlayer<DiamondScuttle>();
                    if (item.type == ItemID.DiamondRobe)
                    {
                        player.statDefense += dia.robeDef;
                        player.statManaMax2 += dia.robeMana;
                    }
                }
            }
            public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
            {
                DiamondScuttle dia = Main.LocalPlayer.GetModPlayer<DiamondScuttle>();
                if (dia.PetIsEquipped())
                {
                    int def = item.defense;
                    if (item.type == ItemID.DiamondRobe)
                    {
                        def = dia.robeDef + item.defense;
                    }
                    if (tooltips.Find(x => x.Name == "Defense") != null)
                        tooltips.Find(x => x.Name == "Defense").Text = def.ToString() + " defense";

                    if (tooltips.Find(x => x.Name == "Tooltip0") != null)
                        tooltips.Find(x => x.Name == "Tooltip0").Text = Language.GetTextValue("ItemTooltip.BandofStarpower").Replace("20", dia.diamondMana.ToString());
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
            DiamondScuttle diageode = player.GetModPlayer<DiamondScuttle>();
            if (PickerPet.PickupChecks(item, diageode.PetItemID, out ItemPet itemChck) && itemChck.oreBoost && item.type == ItemID.Diamond)
            {
                for (int i = 0; i < GlobalPet.Randomizer(diageode.scuttleGemMult * item.stack, 100); i++)
                {
                    player.QuickSpawnItem(GlobalPet.GetSource_Pet(EntitySourcePetIDs.MiningItem), item.type, 1);
                }
            }
        }

        public sealed class DiamondScuttlePetItem : PetTooltip //Tooltip
        {
            public override PetEffect PetsEffect => diaScuttle;
            public static DiamondScuttle diaScuttle
            {
                get
                {
                    if (Main.LocalPlayer.TryGetModPlayer(out DiamondScuttle pet))
                        return pet;
                    else
                        return ModContent.GetInstance<DiamondScuttle>();
                }
            }
            public override string PetsTooltip => PetUtil.LocVal("PetTooltips.Scuttlers.GenericScuttle")
                .Replace("<gem>", "Diamond")
                .Replace("<color>", "White")
                .Replace("<def>", diaScuttle.defenseStat.ToString())
                .Replace("<dmg>", PetUtil.FloatToPercent(diaScuttle.weaponDmg))
                .Replace("<robeDef>", diaScuttle.robeDef.ToString())
                .Replace("<mana>", diaScuttle.robeMana.ToString())
                .Replace("<chance>", diaScuttle.scuttleGemMult.ToString() + "%");
            public override string SimpleTooltip => PetUtil.LocVal("SimplePetTooltips.Scuttlers.GenericScuttle")
                .Replace("<gem>", "Diamond")
                .Replace("<color>", "White");
        }
    }
}
