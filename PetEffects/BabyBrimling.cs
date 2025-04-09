using System.Text;
using System.Threading.Tasks;
using PetsOverhaul.Systems;
using PetsOverhaul;
using PetsOverhaul.Items;
using Terraria.ModLoader;
using CalValEX.Items.Pets.Elementals;
using Terraria;
using Terraria.Localization;
using CalamityMod.Dusts;
using Terraria.ID;

namespace POCalValAddon.PetEffects
{
    public sealed class BabyBrimling : PetEffect
    {
        public override int PetItemID => ModContent.ItemType<RareBrimtulip>();
        public override PetClasses PetClassPrimary => PetClasses.Offensive;

        public float radiusFire = 160f;
        public override void PostUpdateMiscEffects()
        {
            if (PetIsEquipped())
            {
                GlobalPet.CircularDustEffect(Player.Center, ModContent.DustType<BrimstoneFlame>(), (int)radiusFire, dustAmount: 64);
                GlobalPet.CircularDustEffect(Player.Center, DustID.Torch, (int)radiusFire, dustAmount: 32);
                foreach (NPC item in Main.ActiveNPCs)
                {
                    if (item.Distance(Player.Center) <= radiusFire)
                    {
                        item.AddBuff(BuffID.OnFire, 300);
                    }
                }
            }
        }
        public sealed class BabyBrimlingPetItem : PetTooltip //Tooltip
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
            public override string PetsTooltip => Language.GetTextValue("Mods.POCalValAddon.PetTooltips.Elementals.BabyBrimling");
        }
    }
}
