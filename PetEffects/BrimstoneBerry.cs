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
using CalamityMod.Items.Accessories;
using Terraria.GameInput;
using CalamityMod.Buffs.DamageOverTime;
using MonoMod.Core.Platforms;
using CalamityMod.Items.Weapons.Summon;

namespace POCalValAddon.PetEffects
{
    public sealed class BrimstoneBerry : PetEffect
    {
        public override int PetItemID => ModContent.ItemType<BrimberryItem>();
        public override PetClasses PetClassPrimary => PetClasses.Utility;

        public double brimDmg = 0.5;
        public bool IsStrawberry => Player.HasItem(ModContent.ItemType<DormantBrimseeker>());

        //Increasing wing time 
        public override void PostUpdateMiscEffects()
        {
            if (PetIsEquipped())
            {
                Player.wingTimeMax += (int)(Player.wingTimeMax * brimDmg);
                if (IsStrawberry == true)
                {
                    Player.buffImmune[ModContent.BuffType<BrimstoneFlames>()] = true;
                }
            }
        }

        //Decreasing or nullifying Brimstone Flames Effect
        public override void UpdateBadLifeRegen()
        {
            if (PetIsEquipped() && Player.HasBuff(ModContent.BuffType<BrimstoneFlames>()) && !IsStrawberry)
            {
                Player.lifeRegen += 8;
            }
        }
        public sealed class BrimstoneBerryPetItem : PetTooltip
        {
            public override PetEffect PetsEffect => brimBerry;
            public static BrimstoneBerry brimBerry
            {
                get
                {
                    if (Main.LocalPlayer.TryGetModPlayer(out BrimstoneBerry pet))
                        return pet;
                    else
                        return ModContent.GetInstance<BrimstoneBerry>();
                }
            }
            public override string PetsTooltip => Language.GetTextValue("Mods.POCalValAddon.PetTooltips.BrimstoneBerry");
        }
    }
}
