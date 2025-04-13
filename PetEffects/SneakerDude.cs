using System;
using System.Collections.Generic;
using PetsOverhaul.Items;
using PetsOverhaul.Systems;
using PetsOverhaul.PetEffects;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using CalValEX;
using CalValEX.Items.Pets;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Items.Materials;
using Terraria.GameContent.ItemDropRules;

//Idea by @icecontrol33

namespace POCalValAddon.PetEffects
{
    public sealed class SneakerDude : PetEffect
    {
        public override int PetItemID => ModContent.ItemType<ProfanedChewToy>();
        public override PetClasses PetClassPrimary => PetClasses.Utility;

        public int sneakerChance = 500;
        public float lowChanceThreshold = 0.25f;
        public int chanceToRollDoubleItem = 50;
        public int denominatorMult = 100;
        public int numeratorMult = 200;

        public override void UpdateBadLifeRegen()
        {
            if (PetIsEquipped() && Player.HasBuff(ModContent.BuffType<BrimstoneFlames>()))
            {
                Player.lifeRegen += 6;
            }
        }

        public override void Load()
        {
            On_ItemDropResolver.ResolveRule += SneakerDudeExtraDrop;
        }

        private static ItemDropAttemptResult SneakerDudeExtraDrop(On_ItemDropResolver.orig_ResolveRule orig, ItemDropResolver self, IItemDropRule rule, DropAttemptInfo info)
        {
            ItemDropAttemptResult tempResult = new();
            if (rule is CommonDrop drop && drop.itemId == ModContent.ItemType<Bloodstone>() && info.player.TryGetModPlayer(out SneakerDude sneaker) && sneaker.PetIsEquipped())
            {
                if ((float)Math.Max(drop.chanceNumerator, 1) / Math.Max(drop.chanceDenominator, 1) <= sneaker.lowChanceThreshold)
                {
                    drop.chanceNumerator *= sneaker.numeratorMult;
                    drop.chanceDenominator *= sneaker.denominatorMult;
                    tempResult = orig(self, rule, info);
                    drop.chanceNumerator /= sneaker.numeratorMult; //If not reversed back, this is applied permanently until reloaded.
                    drop.chanceDenominator /= sneaker.denominatorMult;
                    return tempResult;
                }
                else if (Main.rand.NextBool(sneaker.chanceToRollDoubleItem, 100))
                {
                    drop.amountDroppedMaximum *= 2;
                    drop.amountDroppedMinimum *= 2;
                    tempResult = orig(self, rule, info);
                    drop.amountDroppedMaximum /= 2; //If not reversed back, this is applied permanently until reloaded.
                    drop.amountDroppedMinimum /= 2;
                    return tempResult;
                }
            }
            tempResult = orig(self, rule, info);

            return tempResult;
        }


        public sealed class SneakerDudePetItem : PetTooltip
        {
            public override PetEffect PetsEffect => sneaDude;
            public static SneakerDude sneaDude
            {
                get
                {
                    if (Main.LocalPlayer.TryGetModPlayer(out SneakerDude pet))
                        return pet;
                    else
                        return ModContent.GetInstance<SneakerDude>();
                }
            }
            public override string PetsTooltip => Language.GetTextValue("Mods.POCalValAddon.PetTooltips.SneakerDude");
        }
    }
}
