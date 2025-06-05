using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Items.Weapons.Summon;
using CalValEX.Items.Pets;
using PetsOverhaul.Systems;
using POCalValAddon.Systems;
using Terraria;
using Terraria.ModLoader;

namespace POCalValAddon.PetEffects
{
    public sealed class BrimstoneBerry : PetEffect
    {
        public override int PetItemID => ModContent.ItemType<BrimberryItem>();
        public override PetClasses PetClassPrimary => PetClasses.Utility;

        public float brimFlight = 0.5f;
        public float brimHealth = 8f;
        public bool IsStrawberry => Player.HasItem(ModContent.ItemType<DormantBrimseeker>());

        public override void PostUpdateMiscEffects()
        {
            if (PetIsEquipped())
            {
                Player.wingTimeMax += (int)(Player.wingTimeMax * brimFlight);
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
                Player.lifeRegen += (int)brimHealth;
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
            public override string PetsTooltip => PetUtil.LocVal("PetTooltips.BrimstoneBerry")
                .Replace("<wing>", PetUtil.FloatToPercent(brimBerry.brimFlight))
                .Replace("<dmg>", brimBerry.brimHealth.ToString());
            public override string SimpleTooltip => PetUtil.LocVal("SimplePetTooltips.BrimstoneBerry");
        }
    }
}
