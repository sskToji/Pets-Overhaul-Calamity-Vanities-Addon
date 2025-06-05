using System.Collections.Generic;
using CalValEX.Items.Pets.Scuttlers;
using PetsOverhaul.Items;
using PetsOverhaul.Systems;
using POCalValAddon.Systems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace POCalValAddon.PetEffects
{
    public sealed class SpikeScuttle : PetEffect
    {
        public override int PetItemID => ModContent.ItemType<BejeweledSpike>();
        public override PetClasses PetClassPrimary => PetClasses.Defensive;
        public override PetClasses PetClassSecondary => PetClasses.Utility;

        //For lists used in this Pet, reference to CalValItemSets in CVAGlobalPet.

        public int defenseStat = 20;
        public float weaponBuff = 0.4f;
        public int robeDef = 10;
        public int robeMana = 40;
        public int scuttleGemMult = 50;
        public float ammoBuff = 0.30f;

        public override void PostUpdateMiscEffects()
        {
            if (PetIsEquipped())
            {
                Player.statDefense += defenseStat;
            }
        }

        public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
        {
            if (PetIsEquipped() && CalValItemSets.GemWeapons[item.type])
            {
                damage += weaponBuff;
            }
            if (PetIsEquipped() && (item.type == ItemID.CrystalBullet || item.type == ItemID.CrystalDart))
            {
                damage += ammoBuff;
            }
        }

        public sealed class ScuttleArmor : GlobalItem
        {
            public override bool AppliesToEntity(Item entity, bool lateInstantiation)
            {
                return CalValItemSets.GemRobes[entity.type];
            }
            public override void UpdateEquip(Item item, Player player)
            {
                SpikeScuttle spikeScuttle = Main.LocalPlayer.GetModPlayer<SpikeScuttle>();
                if (spikeScuttle.PetIsEquipped())
                {
                    player.statDefense += spikeScuttle.robeDef;
                    player.statManaMax2 += spikeScuttle.robeMana;
                }
            }
            public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
            {
                SpikeScuttle spike = Main.LocalPlayer.GetModPlayer<SpikeScuttle>();
                if (spike.PetIsEquipped())
                {
                    int def = item.defense;
                    def = spike.robeDef + item.defense;
                    int mana;
                    switch (item.type)
                    {
                        case ItemID.SapphireRobe:
                            mana = 50;
                            break;
                        case ItemID.TopazRobe:
                            mana = 40;
                            break;
                        case ItemID.RubyRobe:
                            mana = 70;
                            break;
                        case ItemID.EmeraldRobe:
                            mana = 50;
                            break;
                        case ItemID.AmethystRobe:
                            mana = 40;
                            break;
                        case ItemID.AmberRobe:
                            mana = 70;
                            break;
                        case ItemID.DiamondRobe:
                            mana = 80;
                            break;
                        default:
                            mana = 0;
                            break;
                    }
                    if (tooltips.Find(x => x.Name == "Defense") != null)
                        tooltips.Find(x => x.Name == "Defense").Text = def.ToString() + " defense";

                    if (tooltips.Find(x => x.Name == "Tooltip0") != null)
                        tooltips.Find(x => x.Name == "Tooltip0").Text = PetUtil.LocVal("ItemTooltips.ManaRobe").Replace("<mana>", mana.ToString());
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
            SpikeScuttle spikeJewel = player.GetModPlayer<SpikeScuttle>();
            if (PickerPet.PickupChecks(item, spikeJewel.PetItemID, out ItemPet itemChck) && itemChck.oreBoost)
            {
                if (CalValItemSets.GemTypes[item.type])
                {
                    for (int i = 0; i < GlobalPet.Randomizer(spikeJewel.scuttleGemMult * item.stack, 100); i++)
                    {
                        player.QuickSpawnItem(GlobalPet.GetSource_Pet(EntitySourcePetIDs.MiningItem), item.type, 1);
                    }
                }
            }
        }
        public sealed class BejeweledSpikePetItem : PetTooltip
        {
            public override PetEffect PetsEffect => bejSpike;
            public static SpikeScuttle bejSpike
            {
                get
                {
                    if (Main.LocalPlayer.TryGetModPlayer(out SpikeScuttle pet))
                        return pet;
                    else
                        return ModContent.GetInstance<SpikeScuttle>();
                }
            }
            public override string PetsTooltip => PetUtil.LocVal("PetTooltips.Scuttlers.SpikeScuttle")
                .Replace("<dmg>", PetUtil.FloatToPercent(bejSpike.weaponBuff))
                .Replace("<ammoDmg>", PetUtil.FloatToPercent(bejSpike.ammoBuff))
                .Replace("<robeDef>", bejSpike.robeDef.ToString())
                .Replace("<mana>", bejSpike.robeMana.ToString())
                .Replace("<chance>", bejSpike.scuttleGemMult.ToString() + "%");
            public override string SimpleTooltip => PetUtil.LocVal("SimplePetTooltips.Scuttlers.SpikeScuttle");
        }
    }
}
