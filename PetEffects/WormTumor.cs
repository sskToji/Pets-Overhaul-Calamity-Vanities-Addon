using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using CalValEX.Items.Pets;
using PetsOverhaul.Systems;
using POCalValAddon.Systems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace POCalValAddon.PetEffects
{
    public sealed class WormTumor : PetEffect
    {
        public override int PetItemID => ModContent.ItemType<MeatyWormTumor>();
        public override PetClasses PetClassPrimary => PetClasses.Melee;
        public override PetClasses PetClassSecondary => PetClasses.Rogue;

        public int meleeHitTime = 300;
        public int otherHitTime = 150;

        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (PetIsEquipped() && item.CountsAsClass<MeleeDamageClass>())
            {
                target.AddBuff(ModContent.BuffType<BurningBlood>(), meleeHitTime);
                target.AddBuff(BuffID.Ichor, meleeHitTime);
            }
        }
        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (PetIsEquipped() && (proj.CountsAsClass<RogueDamageClass>() || proj.CountsAsClass<MeleeDamageClass>()))
            {
                target.AddBuff(ModContent.BuffType<BurningBlood>(), otherHitTime);
                target.AddBuff(BuffID.Ichor, otherHitTime);
            }
        }

        public sealed class WormTumorPetItem : PetTooltip
        {
            public override PetEffect PetsEffect => wormTumor;
            public static WormTumor wormTumor
            {
                get
                {
                    if (Main.LocalPlayer.TryGetModPlayer(out WormTumor pet))
                        return pet;
                    else
                        return ModContent.GetInstance<WormTumor>();
                }
            }
            public override string PetsTooltip => PetUtil.LocVal("PetTooltips.WormTumor")
                .Replace("<hit>", PetUtil.IntToTime(wormTumor.otherHitTime))
                .Replace("<meleeHit>", PetUtil.IntToTime(wormTumor.meleeHitTime));
            public override string SimpleTooltip => PetUtil.LocVal("SimplePetTooltips.WormTumor");
        }
    }
}
