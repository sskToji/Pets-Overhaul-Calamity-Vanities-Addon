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
    public sealed class TopazScuttle : PetEffect
    {
        public override int PetItemID => ModContent.ItemType<TopazGeode>();
        public override PetClasses PetClassPrimary => PetClasses.Defensive;
        public override PetClasses PetClassSecondary => PetClasses.Mining;

        public int defenseStat = 5;
        public int robeDef = 3;
        public int robeMana = 20;
        public int scuttleGemMult = 50;
        public float weaponDmg = 0.25f;
        public int topazMana = 40;

        public override void PostUpdateMiscEffects()
        {
            if (PetIsEquipped())
            {
                Player.statDefense += defenseStat;
            }
        }

        public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
        {
            if (PetIsEquipped() && (item.type == ItemID.TopazStaff || item.type == ItemID.YellowPhaseblade))
            {
                damage += weaponDmg;
            }
        }
        public sealed class TopScuttleArmor : GlobalItem
        {
            public override bool AppliesToEntity(Item entity, bool lateInstantation)
            {
                return entity.type == ItemID.TopazRobe;
            }
            public override void UpdateEquip(Item item, Player player)
            {
                TopazScuttle topScuttle = Main.LocalPlayer.GetModPlayer<TopazScuttle>();
                if (topScuttle.PetIsEquipped())
                {
                    TopazScuttle top = player.GetModPlayer<TopazScuttle>();
                    if (item.type == ItemID.TopazRobe)
                    {
                        player.statDefense += top.robeDef;
                        player.statManaMax2 += top.robeMana;
                    }
                }
            }
            public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
            {
                TopazScuttle top = Main.LocalPlayer.GetModPlayer<TopazScuttle>();
                if (top.PetIsEquipped())
                {
                    int def = item.defense;
                    if (item.type == ItemID.TopazRobe)
                    {
                        def = top.robeDef + item.defense;
                    }
                    if (tooltips.Find(x => x.Name == "Defense") != null)
                        tooltips.Find(x => x.Name == "Defense").Text = def.ToString() + " defense";

                    if (tooltips.Find(x => x.Name == "Tooltip0") != null)
                        tooltips.Find(x => x.Name == "Tooltip0").Text = Language.GetTextValue("ItemTooltip.BandofStarpower").Replace("20", top.topazMana.ToString());
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
            TopazScuttle topgeode = player.GetModPlayer<TopazScuttle>();
            if (PickerPet.PickupChecks(item, topgeode.PetItemID, out ItemPet itemChck) && itemChck.oreBoost && item.type == ItemID.Topaz)
            {
                for (int i = 0; i < GlobalPet.Randomizer(topgeode.scuttleGemMult * item.stack, 100); i++)
                {
                    player.QuickSpawnItem(GlobalPet.GetSource_Pet(EntitySourcePetIDs.MiningItem), item.type, 1);
                }
            }
        }

        public sealed class TopazScuttlePetItem : PetTooltip
        {
            public override PetEffect PetsEffect => topScuttle;
            public static TopazScuttle topScuttle
            {
                get
                {
                    if (Main.LocalPlayer.TryGetModPlayer(out TopazScuttle pet))
                        return pet;
                    else
                        return ModContent.GetInstance<TopazScuttle>();
                }
            }
            public override string PetsTooltip => PetUtil.LocVal("PetTooltips.Scuttlers.GenericScuttle")
                .Replace("<gem>", "Topaz")
                .Replace("<color>", "Yellow")
                .Replace("<def>", topScuttle.defenseStat.ToString())
                .Replace("<dmg>", PetUtil.FloatToPercent(topScuttle.weaponDmg))
                .Replace("<robeDef>", topScuttle.robeDef.ToString())
                .Replace("<mana>", topScuttle.robeMana.ToString())
                .Replace("<chance>", topScuttle.scuttleGemMult.ToString() + "%");
            public override string SimpleTooltip => PetUtil.LocVal("SimplePetTooltips.Scuttlers.GenericScuttle")
                .Replace("<gem>", "Topaz")
                .Replace("<color>", "Yellow");
        }
    }
}