using System;
using System.Security.Cryptography.X509Certificates;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod.Items.Weapons.Summon;
using CalamityMod.Projectiles.Ranged;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using PetsOverhaul.Systems;
using Terraria;
using Terraria.DataStructures;
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
    public sealed class CVAGlobalPet : ModPlayer
    {
        //Empty for now, implemented for future use
    }

    public sealed class EnemyTouchedEnemy : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public int isTouching = -1;

        public override void AI(NPC npc)
        {
            if (!npc.friendly && !npc.CountsAsACritter)
            foreach (NPC npc1 in Main.ActiveNPCs)
            {
                if (npc1.friendly || npc1.CountsAsACritter)
                    continue;

                if (npc1.getRect().Intersects(npc.Hitbox))
                {
                    isTouching = npc1.whoAmI;
                }
            }
        }
    }

    public sealed class SourceProjectile : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        public bool myProj = false;
        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if (source is EntitySource_ItemUse item && item.Item is not null)
            {
                myProj = true;
            }
            if (source is EntitySource_Parent parent && parent.Entity is Projectile proj && proj.GetGlobalProjectile<SourceProjectile>().myProj == true)
            {
                myProj = true;
            }
        }
    }
}
