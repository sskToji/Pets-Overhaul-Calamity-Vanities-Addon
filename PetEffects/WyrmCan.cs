using System;
using CalValEX.Items.Pets;
using Microsoft.Xna.Framework;
using PetsOverhaul.Systems;
using POCalValAddon.Systems;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Achievements;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace POCalValAddon.PetEffects
{
    public sealed class WyrmCan : PetEffect
    {
        public override int PetItemID => ModContent.ItemType<CanofWyrms>();
        public override PetClasses PetClassPrimary => PetClasses.Fishing;

        public float baitMod = 0.1f;

        public override void Load()
        {
            On_Projectile.FishingCheck += WyrmPond;
            On_Player.GetFishingConditions += WyrmFish;
        }

        private static PlayerFishingConditions WyrmFish(On_Player.orig_GetFishingConditions orig, Player self)
        {
            var wyrm = new WyrmCan();
            PlayerFishingConditions condition = orig(self);
            if (self.miscEquips[0].type == ModContent.ItemType<CanofWyrms>())
            {
                condition.FinalFishingLevel += condition.BaitPower * (int)wyrm.baitMod;
            }
            return condition;
        }

        private static void GetFishingPondState(int x, int y, out bool lava, out bool honey, out int numWaters, out int chumCount)
        {
            //IL_0094: Unknown result type (might be due to invalid IL or missing references)
            lava = false;
            honey = false;
            numWaters = 0;
            chumCount = 0;
            Point tileCoords = default(Point);
            tileCoords = new Point(0, 0);
            GetFishingPondWidth(x, y, out var minX, out var maxX);
            for (int i = minX; i <= maxX; i++)
            {
                int num = y;
                while (Main.tile[i, num] != null && Main.tile[i, num].LiquidAmount > 0 && !WorldGen.SolidTile(i, num) && num < Main.maxTilesY - 10)
                {
                    numWaters++;
                    num++;
                    if (Main.tile[i, num].LiquidType == LiquidID.Lava)
                    {
                        lava = true;
                    }
                    else if (Main.tile[i, num].LiquidType == LiquidID.Honey)
                    {
                        honey = true;
                    }
                    tileCoords.X = i;
                    tileCoords.Y = num;
                    chumCount += Main.instance.ChumBucketProjectileHelper.GetChumsInLocation(tileCoords);
                }
            }
            if (honey)
            {
                numWaters = (int)((double)numWaters * 1.5);
            }
            numWaters = 500;
        }

        private static void GetFishingPondWidth(int x, int y, out int minX, out int maxX)
        {
            minX = x;
            maxX = x;
            while (minX > 10 && Main.tile[minX, y] != null && Main.tile[minX, y].LiquidAmount > 0 && !WorldGen.SolidTile(minX, y))
            {
                minX--;
            }
            while (maxX < Main.maxTilesX - 10 && Main.tile[maxX, y] != null && Main.tile[maxX, y].LiquidAmount > 0 && !WorldGen.SolidTile(maxX, y))
            {
                maxX++;
            }
        }

        private static void FishingCheck_RollDropLevels(Projectile proj, int fishingLevel, out bool common, out bool uncommon, out bool rare, out bool veryrare, out bool legendary, out bool crate)
        {
            int num = 150 / fishingLevel;
            int num2 = 300 / fishingLevel;
            int num3 = 1050 / fishingLevel;
            int num4 = 2250 / fishingLevel;
            int num5 = 4500 / fishingLevel;
            int num6 = 10;
            if (Main.player[proj.owner].cratePotion)
            {
                num6 += 15;
            }
            if (num < 2)
            {
                num = 2;
            }
            if (num2 < 3)
            {
                num2 = 3;
            }
            if (num3 < 4)
            {
                num3 = 4;
            }
            if (num4 < 5)
            {
                num4 = 5;
            }
            if (num5 < 6)
            {
                num5 = 6;
            }
            common = false;
            uncommon = false;
            rare = false;
            veryrare = false;
            legendary = false;
            crate = false;
            if (Main.rand.Next(num) == 0)
            {
                common = true;
            }
            if (Main.rand.Next(num2) == 0)
            {
                uncommon = true;
            }
            if (Main.rand.Next(num3) == 0)
            {
                rare = true;
            }
            if (Main.rand.Next(num4) == 0)
            {
                veryrare = true;
            }
            if (Main.rand.Next(num5) == 0)
            {
                legendary = true;
            }
            if (Main.rand.Next(100) < num6)
            {
                crate = true;
            }
        }

        private static void FishingCheck_ProbeForQuestFish(Projectile proj, ref FishingAttempt fisher)
        {
            fisher.questFish = Main.anglerQuestItemNetIDs[Main.anglerQuest];
            if (Main.player[proj.owner].HasItem(fisher.questFish))
            {
                fisher.questFish = -1;
            }
            if (!NPC.AnyNPCs(369))
            {
                fisher.questFish = -1;
            }
            if (Main.anglerQuestFinished)
            {
                fisher.questFish = -1;
            }
        }

        private static void FishingCheck_RollEnemySpawns(ref FishingAttempt fisher)
        {
            if (fisher.inLava || fisher.inHoney || !Main.bloodMoon || Main.dayTime)
            {
                return;
            }
            int maxValue = 6;
            if (fisher.playerFishingConditions.PoleItemType == 4325)
            {
                maxValue = 3;
            }
            if (Main.rand.Next(maxValue) != 0)
            {
                return;
            }
            if (!NPC.unlockedSlimeRedSpawn && Main.rand.Next(5) == 0)
            {
                fisher.rolledEnemySpawn = 682;
            }
            else if (Main.hardMode)
            {
                fisher.rolledEnemySpawn = Utils.SelectRandom(Main.rand, new short[4] { 620, 621, 586, 587 });
                if (Main.rand.Next(10) == 0)
                {
                    fisher.rolledEnemySpawn = 618;
                }
            }
            else
            {
                fisher.rolledEnemySpawn = Utils.SelectRandom(Main.rand, new short[2] { 586, 587 });
            }
        }

        private static void FishingCheck_RollItemDrop(Projectile proj, ref FishingAttempt fisher)
        {
            bool flag = Main.player[proj.owner].ZoneCorrupt;
            bool flag2 = Main.player[proj.owner].ZoneCrimson;
            bool flag3 = Main.player[proj.owner].ZoneJungle;
            bool flag4 = Main.player[proj.owner].ZoneSnow;
            bool flag5 = Main.player[proj.owner].ZoneDungeon;
            if (!NPC.downedBoss3)
            {
                flag5 = false;
            }
            if (Main.notTheBeesWorld && !Main.remixWorld && Main.rand.Next(2) == 0)
            {
                flag3 = false;
            }
            if (Main.remixWorld && fisher.heightLevel == 0)
            {
                flag = false;
                flag2 = false;
            }
            else if (flag && flag2)
            {
                if (Main.rand.Next(2) == 0)
                {
                    flag2 = false;
                }
                else
                {
                    flag = false;
                }
            }
            if (fisher.rolledEnemySpawn > 0)
            {
                return;
            }
            if (fisher.inLava)
            {
                if (fisher.CanFishInLava)
                {
                    if (fisher.crate && Main.rand.Next(6) == 0)
                    {
                        fisher.rolledItemDrop = (Main.hardMode ? 4878 : 4877);
                    }
                    else if (fisher.legendary && Main.hardMode && Main.rand.Next(3) == 0)
                    {
                        fisher.rolledItemDrop = Main.rand.NextFromList(new short[4] { 4819, 4820, 4872, 2331 });
                    }
                    else if (fisher.legendary && !Main.hardMode && Main.rand.Next(3) == 0)
                    {
                        fisher.rolledItemDrop = Main.rand.NextFromList(new short[3] { 4819, 4820, 4872 });
                    }
                    else if (fisher.veryrare)
                    {
                        fisher.rolledItemDrop = 2312;
                    }
                    else if (fisher.rare)
                    {
                        fisher.rolledItemDrop = 2315;
                    }
                }
                return;
            }
            if (fisher.inHoney)
            {
                if (fisher.rare || (fisher.uncommon && Main.rand.Next(2) == 0))
                {
                    fisher.rolledItemDrop = 2314;
                }
                else if (fisher.uncommon && fisher.questFish == 2451)
                {
                    fisher.rolledItemDrop = 2451;
                }
                return;
            }
            if (Main.rand.Next(50) > fisher.fishingLevel && Main.rand.Next(50) > fisher.fishingLevel && fisher.waterTilesCount < fisher.waterNeededToFish)
            {
                fisher.rolledItemDrop = Main.rand.Next(2337, 2340);
                if (Main.rand.Next(8) == 0)
                {
                    fisher.rolledItemDrop = 5275;
                }
                return;
            }
            if (fisher.crate)
            {
                bool hardMode = Main.hardMode;
                if (fisher.rare && flag5)
                {
                    fisher.rolledItemDrop = (hardMode ? 3984 : 3205);
                }
                else if (fisher.rare && (Main.player[proj.owner].ZoneBeach || (Main.remixWorld && fisher.heightLevel == 1 && (double)fisher.Y >= Main.rockLayer && Main.rand.Next(2) == 0)))
                {
                    fisher.rolledItemDrop = (hardMode ? 5003 : 5002);
                }
                else if (fisher.rare && flag)
                {
                    fisher.rolledItemDrop = (hardMode ? 3982 : 3203);
                }
                else if (fisher.rare && flag2)
                {
                    fisher.rolledItemDrop = (hardMode ? 3983 : 3204);
                }
                else if (fisher.rare && Main.player[proj.owner].ZoneHallow)
                {
                    fisher.rolledItemDrop = (hardMode ? 3986 : 3207);
                }
                else if (fisher.rare && flag3)
                {
                    fisher.rolledItemDrop = (hardMode ? 3987 : 3208);
                }
                else if (fisher.rare && Main.player[proj.owner].ZoneSnow)
                {
                    fisher.rolledItemDrop = (hardMode ? 4406 : 4405);
                }
                else if (fisher.rare && Main.player[proj.owner].ZoneDesert)
                {
                    fisher.rolledItemDrop = (hardMode ? 4408 : 4407);
                }
                else if (fisher.rare && fisher.heightLevel == 0)
                {
                    fisher.rolledItemDrop = (hardMode ? 3985 : 3206);
                }
                else if (fisher.veryrare || fisher.legendary)
                {
                    fisher.rolledItemDrop = (hardMode ? 3981 : 2336);
                }
                else if (fisher.uncommon)
                {
                    fisher.rolledItemDrop = (hardMode ? 3980 : 2335);
                }
                else
                {
                    fisher.rolledItemDrop = (hardMode ? 3979 : 2334);
                }
                return;
            }
            if (!NPC.combatBookWasUsed && Main.bloodMoon && fisher.legendary && Main.rand.Next(3) == 0)
            {
                fisher.rolledItemDrop = 4382;
                return;
            }
            if (Main.bloodMoon && fisher.legendary && Main.rand.Next(2) == 0)
            {
                fisher.rolledItemDrop = 5240;
                return;
            }
            if (fisher.legendary && Main.rand.Next(5) == 0)
            {
                fisher.rolledItemDrop = 2423;
                return;
            }
            if (fisher.legendary && Main.rand.Next(5) == 0)
            {
                fisher.rolledItemDrop = 3225;
                return;
            }
            if (fisher.legendary && Main.rand.Next(10) == 0)
            {
                fisher.rolledItemDrop = 2420;
                return;
            }
            if (!fisher.legendary && !fisher.veryrare && fisher.uncommon && Main.rand.Next(5) == 0)
            {
                fisher.rolledItemDrop = 3196;
                return;
            }
            bool flag6 = Main.player[proj.owner].ZoneDesert;
            if (flag5)
            {
                flag6 = false;
                if (fisher.rolledItemDrop == 0 && fisher.veryrare && Main.rand.Next(7) == 0)
                {
                    fisher.rolledItemDrop = 3000;
                }
            }
            else
            {
                if (flag)
                {
                    if (fisher.legendary && Main.hardMode && Main.player[proj.owner].ZoneSnow && fisher.heightLevel == 3 && Main.rand.Next(3) != 0)
                    {
                        fisher.rolledItemDrop = 2429;
                    }
                    else if (fisher.legendary && Main.hardMode && Main.rand.Next(2) == 0)
                    {
                        fisher.rolledItemDrop = 3210;
                    }
                    else if (fisher.rare)
                    {
                        fisher.rolledItemDrop = 2330;
                    }
                    else if (fisher.uncommon && fisher.questFish == 2454)
                    {
                        fisher.rolledItemDrop = 2454;
                    }
                    else if (fisher.uncommon && fisher.questFish == 2485)
                    {
                        fisher.rolledItemDrop = 2485;
                    }
                    else if (fisher.uncommon && fisher.questFish == 2457)
                    {
                        fisher.rolledItemDrop = 2457;
                    }
                    else if (fisher.uncommon)
                    {
                        fisher.rolledItemDrop = 2318;
                    }
                }
                else if (flag2)
                {
                    if (fisher.legendary && Main.hardMode && Main.player[proj.owner].ZoneSnow && fisher.heightLevel == 3 && Main.rand.Next(3) != 0)
                    {
                        fisher.rolledItemDrop = 2429;
                    }
                    else if (fisher.legendary && Main.hardMode && Main.rand.Next(2) == 0)
                    {
                        fisher.rolledItemDrop = 3211;
                    }
                    else if (fisher.uncommon && fisher.questFish == 2477)
                    {
                        fisher.rolledItemDrop = 2477;
                    }
                    else if (fisher.uncommon && fisher.questFish == 2463)
                    {
                        fisher.rolledItemDrop = 2463;
                    }
                    else if (fisher.uncommon)
                    {
                        fisher.rolledItemDrop = 2319;
                    }
                    else if (fisher.common)
                    {
                        fisher.rolledItemDrop = 2305;
                    }
                }
                else if (Main.player[proj.owner].ZoneHallow)
                {
                    if (flag6 && Main.rand.Next(2) == 0)
                    {
                        if (fisher.uncommon && fisher.questFish == 4393)
                        {
                            fisher.rolledItemDrop = 4393;
                        }
                        else if (fisher.uncommon && fisher.questFish == 4394)
                        {
                            fisher.rolledItemDrop = 4394;
                        }
                        else if (fisher.uncommon)
                        {
                            fisher.rolledItemDrop = 4410;
                        }
                        else if (Main.rand.Next(3) == 0)
                        {
                            fisher.rolledItemDrop = 4402;
                        }
                        else
                        {
                            fisher.rolledItemDrop = 4401;
                        }
                    }
                    else if (fisher.legendary && Main.hardMode && Main.player[proj.owner].ZoneSnow && fisher.heightLevel == 3 && Main.rand.Next(3) != 0)
                    {
                        fisher.rolledItemDrop = 2429;
                    }
                    else if (fisher.legendary && Main.hardMode && Main.rand.Next(2) == 0)
                    {
                        fisher.rolledItemDrop = 3209;
                    }
                    else if (fisher.legendary && Main.hardMode && Main.rand.Next(3) != 0)
                    {
                        fisher.rolledItemDrop = 5274;
                    }
                    else if (fisher.heightLevel > 1 && fisher.veryrare)
                    {
                        fisher.rolledItemDrop = 2317;
                    }
                    else if (fisher.heightLevel > 1 && fisher.uncommon && fisher.questFish == 2465)
                    {
                        fisher.rolledItemDrop = 2465;
                    }
                    else if (fisher.heightLevel < 2 && fisher.uncommon && fisher.questFish == 2468)
                    {
                        fisher.rolledItemDrop = 2468;
                    }
                    else if (fisher.rare)
                    {
                        fisher.rolledItemDrop = 2310;
                    }
                    else if (fisher.uncommon && fisher.questFish == 2471)
                    {
                        fisher.rolledItemDrop = 2471;
                    }
                    else if (fisher.uncommon)
                    {
                        fisher.rolledItemDrop = 2307;
                    }
                }
                if (fisher.rolledItemDrop == 0 && Main.player[proj.owner].ZoneGlowshroom && fisher.uncommon && fisher.questFish == 2475)
                {
                    fisher.rolledItemDrop = 2475;
                }
                if (flag4 && flag3 && Main.rand.Next(2) == 0)
                {
                    flag4 = false;
                }
                if (fisher.rolledItemDrop == 0 && flag4)
                {
                    if (fisher.heightLevel < 2 && fisher.uncommon && fisher.questFish == 2467)
                    {
                        fisher.rolledItemDrop = 2467;
                    }
                    else if (fisher.heightLevel == 1 && fisher.uncommon && fisher.questFish == 2470)
                    {
                        fisher.rolledItemDrop = 2470;
                    }
                    else if (fisher.heightLevel >= 2 && fisher.uncommon && fisher.questFish == 2484)
                    {
                        fisher.rolledItemDrop = 2484;
                    }
                    else if (fisher.heightLevel > 1 && fisher.uncommon && fisher.questFish == 2466)
                    {
                        fisher.rolledItemDrop = 2466;
                    }
                    else if ((fisher.common && Main.rand.Next(12) == 0) || (fisher.uncommon && Main.rand.Next(6) == 0))
                    {
                        fisher.rolledItemDrop = 3197;
                    }
                    else if (fisher.uncommon)
                    {
                        fisher.rolledItemDrop = 2306;
                    }
                    else if (fisher.common)
                    {
                        fisher.rolledItemDrop = 2299;
                    }
                    else if (fisher.heightLevel > 1 && Main.rand.Next(3) == 0)
                    {
                        fisher.rolledItemDrop = 2309;
                    }
                }
                if (fisher.rolledItemDrop == 0 && flag3)
                {
                    if (fisher.heightLevel == 1 && fisher.uncommon && fisher.questFish == 2452)
                    {
                        fisher.rolledItemDrop = 2452;
                    }
                    else if (fisher.heightLevel == 1 && fisher.uncommon && fisher.questFish == 2483)
                    {
                        fisher.rolledItemDrop = 2483;
                    }
                    else if (fisher.heightLevel == 1 && fisher.uncommon && fisher.questFish == 2488)
                    {
                        fisher.rolledItemDrop = 2488;
                    }
                    else if (fisher.heightLevel >= 1 && fisher.uncommon && fisher.questFish == 2486)
                    {
                        fisher.rolledItemDrop = 2486;
                    }
                    else if (fisher.heightLevel > 1 && fisher.uncommon)
                    {
                        fisher.rolledItemDrop = 2311;
                    }
                    else if (fisher.uncommon)
                    {
                        fisher.rolledItemDrop = 2313;
                    }
                    else if (fisher.common)
                    {
                        fisher.rolledItemDrop = 2302;
                    }
                }
            }
            if (fisher.rolledItemDrop == 0)
            {
                if ((Main.remixWorld && fisher.heightLevel == 1 && (double)fisher.Y >= Main.rockLayer && Main.rand.Next(3) == 0) || (fisher.heightLevel <= 1 && (fisher.X < 380 || fisher.X > Main.maxTilesX - 380) && fisher.waterTilesCount > 1000))
                {
                    if (fisher.veryrare && Main.rand.Next(2) == 0)
                    {
                        fisher.rolledItemDrop = 2341;
                    }
                    else if (fisher.veryrare)
                    {
                        fisher.rolledItemDrop = 2342;
                    }
                    else if (fisher.rare && Main.rand.Next(5) == 0)
                    {
                        fisher.rolledItemDrop = 2438;
                    }
                    else if (fisher.rare && Main.rand.Next(3) == 0)
                    {
                        fisher.rolledItemDrop = 2332;
                    }
                    else if (fisher.uncommon && fisher.questFish == 2480)
                    {
                        fisher.rolledItemDrop = 2480;
                    }
                    else if (fisher.uncommon && fisher.questFish == 2481)
                    {
                        fisher.rolledItemDrop = 2481;
                    }
                    else if (fisher.uncommon)
                    {
                        fisher.rolledItemDrop = 2316;
                    }
                    else if (fisher.common && Main.rand.Next(2) == 0)
                    {
                        fisher.rolledItemDrop = 2301;
                    }
                    else if (fisher.common)
                    {
                        fisher.rolledItemDrop = 2300;
                    }
                    else
                    {
                        fisher.rolledItemDrop = 2297;
                    }
                }
                else if (flag6)
                {
                    if (fisher.uncommon && fisher.questFish == 4393)
                    {
                        fisher.rolledItemDrop = 4393;
                    }
                    else if (fisher.uncommon && fisher.questFish == 4394)
                    {
                        fisher.rolledItemDrop = 4394;
                    }
                    else if (fisher.uncommon)
                    {
                        fisher.rolledItemDrop = 4410;
                    }
                    else if (Main.rand.Next(3) == 0)
                    {
                        fisher.rolledItemDrop = 4402;
                    }
                    else
                    {
                        fisher.rolledItemDrop = 4401;
                    }
                }
            }
            if (fisher.rolledItemDrop != 0)
            {
                return;
            }
            if (fisher.heightLevel < 2 && fisher.uncommon && fisher.questFish == 2461)
            {
                fisher.rolledItemDrop = 2461;
            }
            else if (fisher.heightLevel == 0 && fisher.uncommon && fisher.questFish == 2453)
            {
                fisher.rolledItemDrop = 2453;
            }
            else if (fisher.heightLevel == 0 && fisher.uncommon && fisher.questFish == 2473)
            {
                fisher.rolledItemDrop = 2473;
            }
            else if (fisher.heightLevel == 0 && fisher.uncommon && fisher.questFish == 2476)
            {
                fisher.rolledItemDrop = 2476;
            }
            else if (fisher.heightLevel < 2 && fisher.uncommon && fisher.questFish == 2458)
            {
                fisher.rolledItemDrop = 2458;
            }
            else if (fisher.heightLevel < 2 && fisher.uncommon && fisher.questFish == 2459)
            {
                fisher.rolledItemDrop = 2459;
            }
            else if (fisher.heightLevel == 0 && fisher.uncommon)
            {
                fisher.rolledItemDrop = 2304;
            }
            else if (fisher.heightLevel > 0 && fisher.heightLevel < 3 && fisher.uncommon && fisher.questFish == 2455)
            {
                fisher.rolledItemDrop = 2455;
            }
            else if (fisher.heightLevel == 1 && fisher.uncommon && fisher.questFish == 2479)
            {
                fisher.rolledItemDrop = 2479;
            }
            else if (fisher.heightLevel == 1 && fisher.uncommon && fisher.questFish == 2456)
            {
                fisher.rolledItemDrop = 2456;
            }
            else if (fisher.heightLevel == 1 && fisher.uncommon && fisher.questFish == 2474)
            {
                fisher.rolledItemDrop = 2474;
            }
            else if (fisher.heightLevel > 1 && fisher.rare && Main.rand.Next(5) == 0)
            {
                if (Main.hardMode && Main.rand.Next(2) == 0)
                {
                    fisher.rolledItemDrop = 2437;
                }
                else
                {
                    fisher.rolledItemDrop = 2436;
                }
            }
            else if (fisher.heightLevel > 1 && fisher.legendary && Main.rand.Next(3) != 0)
            {
                fisher.rolledItemDrop = 2308;
            }
            else if (fisher.heightLevel > 1 && fisher.veryrare && Main.rand.Next(2) == 0)
            {
                fisher.rolledItemDrop = 2320;
            }
            else if (fisher.heightLevel > 1 && fisher.rare)
            {
                fisher.rolledItemDrop = 2321;
            }
            else if (fisher.heightLevel > 1 && fisher.uncommon && fisher.questFish == 2478)
            {
                fisher.rolledItemDrop = 2478;
            }
            else if (fisher.heightLevel > 1 && fisher.uncommon && fisher.questFish == 2450)
            {
                fisher.rolledItemDrop = 2450;
            }
            else if (fisher.heightLevel > 1 && fisher.uncommon && fisher.questFish == 2464)
            {
                fisher.rolledItemDrop = 2464;
            }
            else if (fisher.heightLevel > 1 && fisher.uncommon && fisher.questFish == 2469)
            {
                fisher.rolledItemDrop = 2469;
            }
            else if (fisher.heightLevel > 2 && fisher.uncommon && fisher.questFish == 2462)
            {
                fisher.rolledItemDrop = 2462;
            }
            else if (fisher.heightLevel > 2 && fisher.uncommon && fisher.questFish == 2482)
            {
                fisher.rolledItemDrop = 2482;
            }
            else if (fisher.heightLevel > 2 && fisher.uncommon && fisher.questFish == 2472)
            {
                fisher.rolledItemDrop = 2472;
            }
            else if (fisher.heightLevel > 2 && fisher.uncommon && fisher.questFish == 2460)
            {
                fisher.rolledItemDrop = 2460;
            }
            else if (fisher.heightLevel > 1 && fisher.uncommon && Main.rand.Next(4) != 0)
            {
                fisher.rolledItemDrop = 2303;
            }
            else if (fisher.heightLevel > 1 && (fisher.uncommon || fisher.common || Main.rand.Next(4) == 0))
            {
                if (Main.rand.Next(4) == 0)
                {
                    fisher.rolledItemDrop = 2303;
                }
                else
                {
                    fisher.rolledItemDrop = 2309;
                }
            }
            else if (fisher.uncommon && fisher.questFish == 2487)
            {
                fisher.rolledItemDrop = 2487;
            }
            else if (fisher.waterTilesCount > 1000 && fisher.common)
            {
                fisher.rolledItemDrop = 2298;
            }
            else
            {
                fisher.rolledItemDrop = 2290;
            }
        }

        private static void WyrmPond(On_Projectile.orig_FishingCheck orig, Projectile self)
        {
            if (Main.player[self.owner].miscEquips[0].type == ModContent.ItemType<CanofWyrms>())
            {
                if (Main.player[self.owner].wet && !(self.Center.Y >= Main.player[self.owner].RotatedRelativePoint(Main.player[self.owner].MountedCenter).Y))
                {
                    return;
                }
                FishingAttempt fisher = default(FishingAttempt);
                fisher.X = (int)(self.Center.X / 16f);
                fisher.Y = (int)(self.Center.Y / 16f);
                fisher.bobberType = self.type;

                GetFishingPondState(fisher.X, fisher.Y, out fisher.inLava, out fisher.inHoney, out fisher.waterTilesCount, out fisher.chumsInWater);

                if (Main.notTheBeesWorld && Main.rand.Next(2) == 0)
                {
                    fisher.inHoney = false;
                }
                if (fisher.waterTilesCount < 75)
                {
                    Main.player[self.owner].displayedFishingInfo = Language.GetTextValue("GameUI.NotEnoughWater");
                    return;
                }
                fisher.playerFishingConditions = Main.player[self.owner].GetFishingConditions();
                if (fisher.playerFishingConditions.BaitItemType == 2673)
                {
                    Main.player[self.owner].displayedFishingInfo = Language.GetTextValue("GameUI.FishingWarning");
                    if ((fisher.X < 380 || fisher.X > Main.maxTilesX - 380) && fisher.waterTilesCount > 1000 && !NPC.AnyNPCs(370))
                    {
                        self.ai[1] = Main.rand.Next(-180, -60) - 100;
                        self.localAI[1] = 1f;
                        self.netUpdate = true;
                    }
                    return;
                }
                fisher.fishingLevel = fisher.playerFishingConditions.FinalFishingLevel;
                if (fisher.fishingLevel == 0)
                {
                    return;
                }
                fisher.CanFishInLava = ItemID.Sets.CanFishInLava[fisher.playerFishingConditions.PoleItemType] || ItemID.Sets.IsLavaBait[fisher.playerFishingConditions.BaitItemType] || Main.player[self.owner].accLavaFishing;
                if (fisher.chumsInWater > 0)
                {
                    fisher.fishingLevel += 11;
                }
                if (fisher.chumsInWater > 1)
                {
                    fisher.fishingLevel += 6;
                }
                if (fisher.chumsInWater > 2)
                {
                    fisher.fishingLevel += 3;
                }
                Main.player[self.owner].displayedFishingInfo = Language.GetTextValue("GameUI.FishingPower", fisher.fishingLevel);
                fisher.waterNeededToFish = 300;
                float num = (float)Main.maxTilesX / 4200f;
                num *= num;
                fisher.atmo = (float)((double)(self.position.Y / 16f - (60f + 10f * num)) / (Main.worldSurface / 6.0));
                if ((double)fisher.atmo < 0.25)
                {
                    fisher.atmo = 0.25f;
                }
                if (fisher.atmo > 1f)
                {
                    fisher.atmo = 1f;
                }
                fisher.waterNeededToFish = (int)((float)fisher.waterNeededToFish * fisher.atmo);
                fisher.waterQuality = (float)fisher.waterTilesCount / (float)fisher.waterNeededToFish;
                if (fisher.waterQuality < 1f)
                {
                    fisher.fishingLevel = (int)((float)fisher.fishingLevel * fisher.waterQuality);
                }
                fisher.waterQuality = 1f - fisher.waterQuality;
                if (fisher.waterTilesCount < fisher.waterNeededToFish)
                {
                    Main.player[self.owner].displayedFishingInfo = Language.GetTextValue("GameUI.FullFishingPower", fisher.fishingLevel, 0.0 - Math.Round(fisher.waterQuality * 100f));
                }
                if (Main.player[self.owner].luck < 0f)
                {
                    if (Main.rand.NextFloat() < 0f - Main.player[self.owner].luck)
                    {
                        fisher.fishingLevel = (int)((double)fisher.fishingLevel * (0.9 - (double)Main.rand.NextFloat() * 0.3));
                    }
                }
                else if (Main.rand.NextFloat() < Main.player[self.owner].luck)
                {
                    fisher.fishingLevel = (int)((double)fisher.fishingLevel * (1.1 + (double)Main.rand.NextFloat() * 0.3));
                }
                int num2 = (fisher.fishingLevel + 75) / 2;
                if (Main.rand.Next(100) > num2)
                {
                    return;
                }
                fisher.heightLevel = 0;
                if (Main.remixWorld)
                {
                    if ((double)fisher.Y < Main.worldSurface * 0.5)
                    {
                        fisher.heightLevel = 0;
                    }
                    else if ((double)fisher.Y < Main.worldSurface)
                    {
                        fisher.heightLevel = 1;
                    }
                    else if ((double)fisher.Y < Main.rockLayer)
                    {
                        fisher.heightLevel = 3;
                    }
                    else if (fisher.Y < Main.maxTilesY - 300)
                    {
                        fisher.heightLevel = 2;
                    }
                    else
                    {
                        fisher.heightLevel = 4;
                    }
                    if (fisher.heightLevel == 2 && Main.rand.Next(2) == 0)
                    {
                        fisher.heightLevel = 1;
                    }
                }
                else if ((double)fisher.Y < Main.worldSurface * 0.5)
                {
                    fisher.heightLevel = 0;
                }
                else if ((double)fisher.Y < Main.worldSurface)
                {
                    fisher.heightLevel = 1;
                }
                else if ((double)fisher.Y < Main.rockLayer)
                {
                    fisher.heightLevel = 2;
                }
                else if (fisher.Y < Main.maxTilesY - 300)
                {
                    fisher.heightLevel = 3;
                }
                else
                {
                    fisher.heightLevel = 4;
                }
                FishingCheck_RollDropLevels(self, fisher.fishingLevel, out fisher.common, out fisher.uncommon, out fisher.rare, out fisher.veryrare, out fisher.legendary, out fisher.crate);
                PlayerLoader.ModifyFishingAttempt(Main.player[self.owner], ref fisher);
                FishingCheck_ProbeForQuestFish(self, ref fisher);
                FishingCheck_RollEnemySpawns(ref fisher);
                FishingCheck_RollItemDrop(self, ref fisher);
                bool flag = false;
                AdvancedPopupRequest sonar = default(AdvancedPopupRequest);
                Vector2 sonarPosition = default(Vector2);
                sonarPosition = new Vector2(self.position.X, self.position.Y);
                PlayerLoader.CatchFish(Main.player[self.owner], fisher, ref fisher.rolledItemDrop, ref fisher.rolledEnemySpawn, ref sonar, ref sonarPosition);
                if (sonar.Text != null && Main.player[self.owner].sonarPotion)
                {
                    PopupText.AssignAsSonarText(PopupText.NewText(sonar, sonarPosition));
                }
                if (fisher.rolledItemDrop > 0)
                {
                    if (sonar.Text == null && Main.player[self.owner].sonarPotion)
                    {
                        Item item = new Item();
                        item.SetDefaults(fisher.rolledItemDrop);
                        item.position = self.position;
                        int sonarPopupText = PopupText.NewText(PopupTextContext.SonarAlert, item, 1, noStack: true);
                        if (sonarPopupText >= 0)
                        {
                            Main.popupText[sonarPopupText].NoStack = true;
                        }
                        PopupText.AssignAsSonarText(sonarPopupText);
                        self.localAI[2] = sonarPopupText + 1;
                    }
                    float num3 = fisher.fishingLevel;
                    self.ai[1] = (float)Main.rand.Next(-240, -90) - num3;
                    self.localAI[1] = fisher.rolledItemDrop;
                    self.netUpdate = true;
                    flag = true;
                }
                if (fisher.rolledEnemySpawn > 0)
                {
                    if (sonar.Text == null && Main.player[self.owner].sonarPotion)
                    {
                        PopupText.AssignAsSonarText(PopupText.NewText(PopupTextContext.SonarAlert, fisher.rolledEnemySpawn, self.Center, stay5TimesLonger: false));
                    }
                    float num4 = fisher.fishingLevel;
                    self.ai[1] = (float)Main.rand.Next(-240, -90) - num4;
                    self.localAI[1] = -fisher.rolledEnemySpawn;
                    self.netUpdate = true;
                    flag = true;
                }
                if (!flag && fisher.inLava)
                {
                    int num5 = 0;
                    if (ItemID.Sets.IsLavaBait[fisher.playerFishingConditions.BaitItemType])
                    {
                        num5++;
                    }
                    if (ItemID.Sets.CanFishInLava[fisher.playerFishingConditions.PoleItemType])
                    {
                        num5++;
                    }
                    if (Main.player[self.owner].accLavaFishing)
                    {
                        num5++;
                    }
                    if (num5 >= 2)
                    {
                        self.localAI[1] += 240f;
                    }
                }
                if (fisher.CanFishInLava && fisher.inLava)
                {
                    AchievementsHelper.HandleSpecialEvent(Main.player[self.owner], 19);
                }
            }
            else
            {
                orig(self);
            }
        }

        public sealed class WyrmCanPetItem : PetTooltip
        {
            public override PetEffect PetsEffect => wyrmCan;
            public static WyrmCan wyrmCan
            {
                get
                {
                    if (Main.LocalPlayer.TryGetModPlayer(out WyrmCan pet))
                        return pet;
                    else
                        return ModContent.GetInstance<WyrmCan>();
                }
            }
            public override string PetsTooltip => PetUtil.LocVal("PetTooltips.WyrmCan")
                .Replace("<bait>", PetUtil.FloatToPercent(wyrmCan.baitMod));
            public override string SimpleTooltip => PetUtil.LocVal("SimplePetTooltips.WyrmCan");
        }
    }
}
