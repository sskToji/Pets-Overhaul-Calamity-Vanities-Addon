using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalamityMod.Buffs.StatDebuffs;
using CalamityMod.Dusts;
using CalValEX.Items.Pets;
using PetsOverhaul.Systems;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace POCalValAddon.PetEffects
{
    public sealed class Smauler : PetEffect
    {
        public override int PetItemID => ModContent.ItemType<BubbledFin>();
        public override PetClasses PetClassPrimary => PetClasses.Utility;

        public float radiusSmauler = 300f;

        public override void PostUpdateMiscEffects()
        {
            if (PetIsEquipped())
            {
                GlobalPet.CircularDustEffect(Player.Center, ModContent.DustType<AuricBarDust>(), (int)radiusSmauler, dustAmount: 64);
                foreach(NPC item in Main.ActiveNPCs)
                {
                    if (item.Distance(Player.Center) <= radiusSmauler)
                    {
                        item.AddBuff(ModContent.BuffType<Irradiated>(), 300);
                    }
                }
            }
        }

        public sealed class SmaulerPetItem : PetTooltip
        {
            public override PetEffect PetsEffect => smauler;
            public static Smauler smauler
            {
                get
                {
                    if (Main.LocalPlayer.TryGetModPlayer(out Smauler pet))
                        return pet;
                    else
                        return ModContent.GetInstance<Smauler>();
                }
            }
            public override string PetsTooltip => Language.GetTextValue("Mods.POCalValAddon.PetTooltips.Smauler");
        }
    }
}
