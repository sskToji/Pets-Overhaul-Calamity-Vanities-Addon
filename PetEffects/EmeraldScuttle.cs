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
    public sealed class EmeraldScuttle : PetEffect
    {
        public override int PetItemID => ModContent.ItemType<EmeraldGeode>();
        public override PetClasses PetClassPrimary => PetClasses.Defensive;
        public override PetClasses PetClassSecondary => PetClasses.Mining;

        public int defenseStat = 5;
        public int robeDef = 4;
        public int robeMana = 10;
        public int scuttleGemMult = 50;
        public float weaponDmg = 0.25f;
        public int emeraldMana = 50;

        public override void PostUpdateMiscEffects()
        {
            if (PetIsEquipped())
            {
                Player.statDefense += defenseStat;
            }
        }

        public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
        {
            if (PetIsEquipped() && (item.type == ItemID.EmeraldStaff || item.type == ItemID.GreenPhaseblade))
            {
                damage += weaponDmg;
            }
        }
        public sealed class EmeScuttleArmor : GlobalItem
        {
            public override bool AppliesToEntity(Item entity, bool lateInstantation)
            {
                return entity.type == ItemID.EmeraldRobe;
            }
            public override void UpdateEquip(Item item, Player player)
            {
                EmeraldScuttle emeScuttle = Main.LocalPlayer.GetModPlayer<EmeraldScuttle>();
                if (emeScuttle.PetIsEquipped())
                {
                    EmeraldScuttle eme = player.GetModPlayer<EmeraldScuttle>();
                    if (item.type == ItemID.EmeraldRobe)
                    {
                        player.statDefense += eme.robeDef;
                        player.statManaMax2 += eme.robeMana;
                    }
                }
            }
            public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
            {
                EmeraldScuttle eme = Main.LocalPlayer.GetModPlayer<EmeraldScuttle>();
                if (eme.PetIsEquipped())
                {
                    int def = item.defense;
                    if (item.type == ItemID.EmeraldRobe)
                    {
                        def = eme.robeDef + item.defense;
                    }
                    if (tooltips.Find(x => x.Name == "Defense") != null)
                        tooltips.Find(x => x.Name == "Defense").Text = def.ToString() + " defense";

                    if (tooltips.Find(x => x.Name == "Tooltip0") != null)
                        tooltips.Find(x => x.Name == "Tooltip0").Text = Language.GetTextValue("ItemTooltip.BandofStarpower").Replace("20", eme.emeraldMana.ToString());
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
            EmeraldScuttle emegeode = player.GetModPlayer<EmeraldScuttle>();
            if (PickerPet.PickupChecks(item, emegeode.PetItemID, out ItemPet itemChck) && itemChck.oreBoost && item.type == ItemID.Emerald)
            {
                for (int i = 0; i < GlobalPet.Randomizer(emegeode.scuttleGemMult * item.stack, 100); i++)
                {
                    player.QuickSpawnItem(GlobalPet.GetSource_Pet(EntitySourcePetIDs.MiningItem), item.type, 1);
                }
            }
        }

        public sealed class EmeraldScuttlePetItem : PetTooltip
        {
            public override PetEffect PetsEffect => emeScuttle;
            public static EmeraldScuttle emeScuttle
            {
                get
                {
                    if (Main.LocalPlayer.TryGetModPlayer(out EmeraldScuttle pet))
                        return pet;
                    else
                        return ModContent.GetInstance<EmeraldScuttle>();
                }
            }
            public override string PetsTooltip => PetUtil.LocVal("PetTooltips.Scuttlers.GenericScuttle")
                .Replace("<gem>", "Emerald")
                .Replace("<color>", "Green")
                .Replace("<def>", emeScuttle.defenseStat.ToString())
                .Replace("<dmg>", PetUtil.FloatToPercent(emeScuttle.weaponDmg))
                .Replace("<robeDef>", emeScuttle.robeDef.ToString())
                .Replace("<mana>", emeScuttle.robeMana.ToString())
                .Replace("<chance>", emeScuttle.scuttleGemMult.ToString() + "%");
            public override string SimpleTooltip => PetUtil.LocVal("SimplePetTooltips.Scuttlers.GenericScuttle")
                .Replace("<gem>", "Emerald")
                .Replace("<color>", "Green");
        }
    }
}