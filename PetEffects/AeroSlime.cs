using CalValEX.Items.Pets;
using PetsOverhaul.Systems;
using POCalValAddon.Systems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

//Idea by @iamnamedmuffin

namespace POCalValAddon.PetEffects
{
    public sealed class AeroSlime : PetEffect
    {
        public override int PetItemID => ModContent.ItemType<AerialiteBubble>();
        public override PetClasses PetClassPrimary => PetClasses.Utility;

        public float WingtimeIncrease = 0.1f;

        public override void PostUpdateMiscEffects()
        {
            if (PetIsEquipped())
            {
                Player.wingTimeMax += (int)(Player.wingTimeMax * WingtimeIncrease);
                Player.AddBuff(BuffID.Featherfall, 1);
            }
        }

        public sealed class AeroSlimePetItem : PetTooltip
        {
            public override PetEffect PetsEffect => aeroSlime;
            public static AeroSlime aeroSlime
            {
                get
                {
                    if (Main.LocalPlayer.TryGetModPlayer(out AeroSlime pet))
                        return pet;
                    else
                        return ModContent.GetInstance<AeroSlime>();
                }
            }
            public override string PetsTooltip => PetUtil.LocVal("PetTooltips.AeroSlime")
                    .Replace("<wing>", PetUtil.FloatToPercent(aeroSlime.WingtimeIncrease));
            public override string SimpleTooltip => PetUtil.LocVal("SimplePetTooltips.AeroSlime");
        }
    }
}
