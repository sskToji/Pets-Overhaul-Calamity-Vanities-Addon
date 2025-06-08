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
    public sealed class AmethystScuttle : PetEffect
    {
        public override int PetItemID => ModContent.ItemType<AmethystGeode>();
        public override PetClasses PetClassPrimary => PetClasses.Defensive;
        public override PetClasses PetClassSecondary => PetClasses.Mining;

        public int defenseStat = 5;
        public int robeDef = 2;
        public int robeMana = 20;
        public int scuttleGemMult = 50;
        public float weaponDmg = 0.25f;
        public int amethystMana = 40;

        public override void PostUpdateMiscEffects()
        {
            if (PetIsEquipped())
            {
                Player.statDefense += defenseStat;
            }
        }

        public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
        {
            if (PetIsEquipped() && (item.type == ItemID.AmethystStaff || item.type == ItemID.PurplePhaseblade))
            {
                damage += weaponDmg;
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
                AmethystScuttle ameScuttle = Main.LocalPlayer.GetModPlayer<AmethystScuttle>();
                if (ameScuttle.PetIsEquipped())
                {
                    AmethystScuttle ame = player.GetModPlayer<AmethystScuttle>();
                    if (item.type == ItemID.AmethystRobe)
                    {
                        player.statDefense += ame.robeDef;
                        player.statManaMax2 += ame.robeMana;
                    }
                }
            }
            public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
            {
                AmethystScuttle ame = Main.LocalPlayer.GetModPlayer<AmethystScuttle>();
                if (ame.PetIsEquipped())
                {
                    int indx = tooltips.FindLastIndex(x => x.Name == "Equipable") + 1;
                    int def = item.defense;
                    if (item.type == ItemID.AmethystRobe)
                    {
                        def = ame.robeDef + item.defense;
                    }
                    tooltips.Insert(indx, new(Mod, "PetToolTip0", $"{def}{Language.GetTextValue("LegacyTooltip.25")}"));

                    if (tooltips.Find(x => x.Name == "Tooltip0") != null)
                        tooltips.Find(x => x.Name == "Tooltip0").Text = Language.GetTextValue("ItemTooltip.BandofStarpower").Replace("20", ame.amethystMana.ToString());
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
            AmethystScuttle amegeode = player.GetModPlayer<AmethystScuttle>();
            if (PickerPet.PickupChecks(item, amegeode.PetItemID, out ItemPet itemChck) && itemChck.oreBoost && item.type == ItemID.Amethyst)
            {
                for (int i = 0; i < GlobalPet.Randomizer(amegeode.scuttleGemMult * item.stack, 100); i++)
                {
                    player.QuickSpawnItem(GlobalPet.GetSource_Pet(EntitySourcePetIDs.MiningItem), item.type, 1);
                }
            }
        }

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
            public override string PetsTooltip => PetUtil.LocVal("PetTooltips.Scuttlers.GenericScuttle")
                .Replace("<gem>", "Amethyst")
                .Replace("<color>", "Purple")
                .Replace("<def>", ameScuttle.defenseStat.ToString())
                .Replace("<dmg>", PetUtil.FloatToPercent(ameScuttle.weaponDmg))
                .Replace("<robeDef>", ameScuttle.robeDef.ToString())
                .Replace("<mana>", ameScuttle.robeMana.ToString())
                .Replace("<chance>", ameScuttle.scuttleGemMult.ToString() + "%");
            public override string SimpleTooltip => PetUtil.LocVal("SimplePetTooltips.Scuttlers.GenericScuttle")
                .Replace("<gem>", "Amethyst")
                .Replace("<color>", "Purple");
        }
    }
}
