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
}
