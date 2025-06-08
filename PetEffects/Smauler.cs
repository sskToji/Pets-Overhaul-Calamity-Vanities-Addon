using CalamityMod.Buffs.StatDebuffs;
using CalamityMod.Dusts;
using CalValEX.Items.Pets;
using PetsOverhaul.NPCs;
using PetsOverhaul.Systems;
using POCalValAddon.Systems;
using Terraria;
using Terraria.ModLoader;

namespace POCalValAddon.PetEffects
{
    public sealed class Smauler : PetEffect
    {
        public override int PetItemID => ModContent.ItemType<BubbledFin>();
        public override PetClasses PetClassPrimary => PetClasses.Utility;

        public float radiusSmauler = 300f;
        public int debuffTime = 300;

        public override void PostUpdateMiscEffects()
        {
            if (PetIsEquipped())
            {
                GlobalPet.CircularDustEffect(Player.Center, ModContent.DustType<AuricBarDust>(), (int)radiusSmauler, dustAmount: 64);
                foreach (NPC npc in Main.ActiveNPCs)
                {
                    if (npc.Distance(Player.Center) <= radiusSmauler && !(npc.friendly || npc.CountsAsACritter))
                    {
                        npc.AddBuff(ModContent.BuffType<Irradiated>(), debuffTime);
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
            public override string PetsTooltip => PetUtil.LocVal("PetTooltips.Smauler")
                .Replace("<px>", smauler.radiusSmauler.ToString())
                .Replace("<secs>", PetUtil.IntToTime(smauler.debuffTime));
            public override string SimpleTooltip => PetUtil.LocVal("SimplePetTooltips.Smauler");
        }
    }
}
