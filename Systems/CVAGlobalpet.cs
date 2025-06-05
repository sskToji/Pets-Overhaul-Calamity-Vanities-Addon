using System;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod.Items.Weapons.Summon;
using CalamityMod.Projectiles.Ranged;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace POCalValAddon.Systems
{
    public class PetUtil
    {
        public static string LocVal(string val)
        {
            return Language.GetTextValue("Mods.POCalValAddon." + val);
        }

        public static string FloatToPercent(float val)
        {
            return Math.Round(val * 100, 2).ToString() + "%";
        }

        public static string IntToTime(int val)
        {
            if (val / 3600 >= 1)
            {
                return Math.Round(val / 3600f, 2).ToString() + " " + LocVal("Misc.Mins");
            }
            if (val / 60 <= 1)
            {
                return Math.Round(val / 3600f, 2).ToString() + " " + LocVal("Misc.Sec");
            }
            return Math.Round(val / 60f, 2).ToString() + " " + LocVal("Misc.Secs");
        }
    }
    [ReinitializeDuringResizeArrays]
    public static class CalValItemSets
    {
        public static bool[] NuggetWeapons = ItemID.Sets.Factory.CreateNamedSet("NuggetSet").Description("Weapons in this set have their Damage boosted by Jungle Nugget (Nugget in a Biscuit)").RegisterBoolSet(false,
            ModContent.ItemType<TheBurningSky>(),
            ModContent.ItemType<DragonRage>(),
            ModContent.ItemType<TheFinalDawn>(),
            ModContent.ItemType<Wrathwing>()
        );

        public static bool[] GrandScaleWeapons = ItemID.Sets.Factory.CreateNamedSet("GrandScaleSet").Description("Weapons in this set have their Damage boosted by Grandson Shark (Dusty Badge)").RegisterBoolSet(false,
            ModContent.ItemType<DuststormInABottle>(),
            ModContent.ItemType<SandSharknadoStaff>(),
            ModContent.ItemType<Sandslasher>(),
            ModContent.ItemType<SandstormGun>(),
            ModContent.ItemType<ShiftingSands>(),
            ModContent.ItemType<Tumbleweed>()
        );

        public static bool[] GemWeapons = ItemID.Sets.Factory.CreateNamedSet("GemWeaponSet").Description("Weapons in this set have their Damage boosted by Spike Scuttle (Bejeweled Spike)").RegisterBoolSet(false,
            ItemID.BluePhaseblade, ItemID.BluePhasesaber, ItemID.SapphireStaff,         //Sapphire Weapons
            ItemID.RedPhaseblade, ItemID.RedPhasesaber, ItemID.RubyStaff,               //Ruby Weapons
            ItemID.PurplePhaseblade, ItemID.PurplePhasesaber, ItemID.AmethystStaff,     //Amethyst Weapons
            ItemID.GreenPhaseblade, ItemID.GreenPhasesaber, ItemID.EmeraldStaff,        //Emerald Weapons
            ItemID.WhitePhaseblade, ItemID.WhitePhasesaber, ItemID.DiamondStaff,        //Diamond Weapons
            ItemID.YellowPhaseblade, ItemID.YellowPhasesaber, ItemID.TopazStaff,        //Topaz Weapons
            ItemID.OrangePhaseblade, ItemID.OrangePhasesaber, ItemID.AmberStaff,        //Amber Weapons
            ItemID.CrystalVileShard, ItemID.CrystalSerpent                              //Crystal Weapons
        );

        public static bool[] GemTypes = ItemID.Sets.Factory.CreateNamedSet("GemTypeSet").Description("Gems in this set have their DropChance boosted by Spike Scuttle (Bejeweled Spike)").RegisterBoolSet(false,
            ItemID.Sapphire,
            ItemID.Ruby,
            ItemID.Amethyst,
            ItemID.Emerald,
            ItemID.Diamond,
            ItemID.Topaz,
            ItemID.Amber
        );

        public static bool[] GemRobes = ItemID.Sets.Factory.CreateNamedSet("GemRobeSet").Description("Robes in this set have their stats increased by Spike Scuttle (Bejeweled Spike)").RegisterBoolSet(false,
            ItemID.SapphireRobe,
            ItemID.RubyRobe,
            ItemID.AmethystRobe,
            ItemID.EmeraldRobe,
            ItemID.DiamondRobe,
            ItemID.TopazRobe,
            ItemID.AmberRobe
        );

        public static bool[] AresWeapons = ItemID.Sets.Factory.CreateNamedSet("AresWeaponSet").Description("Weapons in this set have their stats increased by Toy XF-Ares (Ominous Core)").RegisterBoolSet(false,
            ModContent.ItemType<AresExoskeleton>(),
            ModContent.ItemType<PhotonRipper>(),
            ModContent.ItemType<TheJailor>(),
            ModContent.ProjectileType<PrismMine>()
        );

        public static bool[] EyesWeapons = ItemID.Sets.Factory.CreateNamedSet("EyesWeaponSet").Description("Weapons in this set have their stats increased by Toy XS-Apollo and Artemis (Gemini Mark Implants)").RegisterBoolSet(false,
            ModContent.ItemType<TheAtomSplitter>(),
            ModContent.ItemType<SurgeDriver>(),
            ModContent.ProjectileType<PrismExplosionLarge>(),
            ModContent.ProjectileType<PrismExplosionSmall>(),
            ModContent.ProjectileType<PrismComet>()
        );

        public static bool[] WormWeapons = ItemID.Sets.Factory.CreateNamedSet("WormWeaponSet").Description("Weapons in this set have their stats increased by Toy XM-Thanatos (Dark Gunmetal Remote)").RegisterBoolSet(false,
            ModContent.ItemType<SpineOfThanatos>(),
            ModContent.ItemType<AtlasMunitionsBeacon>(),
            ModContent.ItemType<RefractionRotor>()
        );

        public static bool[] GemMelee = ItemID.Sets.Factory.CreateNamedSet("GemMeleeSet").Description("Weapons in this set have their stats increased by Exo Gemstone").RegisterBoolSet(false,
            ModContent.ItemType<PhotonRipper>(),
            ModContent.ItemType<SpineOfThanatos>()
        );
        public static bool[] GemRanged = ItemID.Sets.Factory.CreateNamedSet("GemRangedSet").Description("Weapons in this set have their stats increased by Exo Gemstone").RegisterBoolSet(false,
            ModContent.ItemType<SurgeDriver>(),
            ModContent.ItemType<TheJailor>()
        );
        public static bool[] GemSummon = ItemID.Sets.Factory.CreateNamedSet("GemSummonSet").Description("Weapons in this set have their stats increased by Exo Gemstone").RegisterBoolSet(false,
            ModContent.ItemType<AresExoskeleton>(),
            ModContent.ItemType<AtlasMunitionsBeacon>()
        );
        public static bool[] GemRogue = ItemID.Sets.Factory.CreateNamedSet("GemRogueSet").Description("Weapons in this set have their stats increased by Exo Gemstone").RegisterBoolSet(false,
            ModContent.ItemType<TheAtomSplitter>(),
            ModContent.ItemType<RefractionRotor>()
        );
    }
}
