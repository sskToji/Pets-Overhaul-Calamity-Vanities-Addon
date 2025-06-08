using CalamityMod.Dusts;
using CalValEX.Items.Pets.Elementals;
using PetsOverhaul.Systems;
using POCalValAddon.Systems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace POCalValAddon.PetEffects
{
    public sealed class BabyBrimling : PetEffect
    {
        public override int PetItemID => ModContent.ItemType<RareBrimtulip>();
        public override PetClasses PetClassPrimary => PetClasses.Offensive;

        public float radiusFire = 160f;
        public int debuffTime = 300;

        public override void PostUpdateMiscEffects()
        {
            if (PetIsEquipped())
            {
                GlobalPet.CircularDustEffect(Player.Center, ModContent.DustType<BrimstoneFlame>(), (int)radiusFire, dustAmount: 64);
                GlobalPet.CircularDustEffect(Player.Center, DustID.Torch, (int)radiusFire, dustAmount: 32);
                foreach (NPC item in Main.ActiveNPCs)
                {
                    if (item.Distance(Player.Center) <= radiusFire && !(item.friendly || item.CountsAsACritter || item.dontTakeDamage))
                    {
                        item.AddBuff(BuffID.OnFire, debuffTime);
                    }
                }
            }
        }
        public sealed class BabyBrimlingPetItem : PetTooltip
        {
            public override PetEffect PetsEffect => babyBrimling;
            public static BabyBrimling babyBrimling
            {
                get
                {
                    if (Main.LocalPlayer.TryGetModPlayer(out BabyBrimling pet))
                        return pet;
                    else
                        return ModContent.GetInstance<BabyBrimling>();
                }
            }
            public override string PetsTooltip => PetUtil.LocVal("PetTooltips.Elementals.BabyBrimling")
                .Replace("<px>", babyBrimling.radiusFire.ToString())
                .Replace("<secs>", PetUtil.IntToTime(babyBrimling.debuffTime));
            public override string SimpleTooltip => PetUtil.LocVal("SimplePetTooltips.Elementals.BabyBrimling");
        }
    }
}
