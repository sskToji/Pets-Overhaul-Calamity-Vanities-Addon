using CalamityMod.Buffs.StatBuffs;
using CalValEX.Items.Pets;
using PetsOverhaul.Systems;
using POCalValAddon.Systems;
using Terraria;
using Terraria.ModLoader;

//make supportive pet. players in radius get mushy buff, dont forget tooltips when this is done

namespace POCalValAddon.PetEffects
{
    public sealed class SmolCrab : PetEffect
    {
        public override int PetItemID => ModContent.ItemType<ClawShroom>();
        public override PetClasses PetClassPrimary => PetClasses.Supportive;

        public int crabRegen = 5;
        public int buffDuration = 300;
        public int mushyRadius = 450;

        public override void PostUpdateMiscEffects()
        {
            if (PetIsEquipped())
            {
                Player.lifeRegen += crabRegen;
            }
        }

        public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo)
        {
            if (PetIsEquipped())
            {
                Player.AddBuff(ModContent.BuffType<Mushy>(), buffDuration);
            }
        }
        public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo)
        {
            if (PetIsEquipped())
            {
                Player.AddBuff(ModContent.BuffType<Mushy>(), buffDuration);
            }
        }

        public sealed class SmolCrabPetItem : PetTooltip
        {
            public override PetEffect PetsEffect => smolCrab;
            public static SmolCrab smolCrab
            {
                get
                {
                    if (Main.LocalPlayer.TryGetModPlayer(out SmolCrab pet))
                        return pet;
                    else
                        return ModContent.GetInstance<SmolCrab>();
                }
            }
            public override string PetsTooltip => PetUtil.LocVal("PetTooltips.SmolCrab")
                .Replace("<regen>", smolCrab.crabRegen.ToString())
                .Replace("<secs>", PetUtil.IntToTime(smolCrab.buffDuration));
            public override string SimpleTooltip => PetUtil.LocVal("SimplePetTooltips.SmolCrab");
        }
    }
}
