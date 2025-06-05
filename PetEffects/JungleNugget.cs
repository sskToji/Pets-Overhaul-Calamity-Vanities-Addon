using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using CalValEX.Items.Pets;
using PetsOverhaul.Systems;
using POCalValAddon.Systems;
using Terraria;
using Terraria.ModLoader;

namespace POCalValAddon.PetEffects
{
    public sealed class JungleNugget : PetEffect
    {
        public override int PetItemID => ModContent.ItemType<NuggetinaBiscuit>();
        public override PetClasses PetClassPrimary => PetClasses.Rogue;
        public override PetClasses PetClassSecondary => PetClasses.Melee;

        public float nuggetDmg = 0.2f;
        public int debuffTime = 300;

        public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
        {
            if (PetIsEquipped() && CalValItemSets.NuggetWeapons[item.type])
            {
                damage += nuggetDmg;
            }
            if (PetIsEquipped() && !CalValItemSets.NuggetWeapons[item.type] && (item.CountsAsClass<MeleeDamageClass>() || item.CountsAsClass<RogueDamageClass>()))
            {
                damage -= nuggetDmg;
            }
        }
        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (PetIsEquipped() && !CalValItemSets.NuggetWeapons[item.type] && item.CountsAsClass<MeleeDamageClass>())
            {
                target.AddBuff(ModContent.BuffType<Dragonfire>(), debuffTime);
            }
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (PetIsEquipped() && !CalValItemSets.NuggetWeapons[proj.type] && proj.CountsAsClass<RogueDamageClass>())
            {
                target.AddBuff(ModContent.BuffType<Dragonfire>(), debuffTime);
            }
        }
        public sealed class JungleNuggetPetItem : PetTooltip
        {
            public override PetEffect PetsEffect => jungleNugget;
            public static JungleNugget jungleNugget
            {
                get
                {
                    if (Main.LocalPlayer.TryGetModPlayer(out JungleNugget pet))
                        return pet;
                    else
                        return ModContent.GetInstance<JungleNugget>();
                }
            }
            public override string PetsTooltip => PetUtil.LocVal("PetTooltips.JungleNugget")
                .Replace("<secs>", PetUtil.IntToTime(jungleNugget.debuffTime))
                .Replace("<minusDmg>", PetUtil.FloatToPercent(jungleNugget.nuggetDmg))
                .Replace("<plusDmg>", PetUtil.FloatToPercent(jungleNugget.nuggetDmg));
            public override string SimpleTooltip => PetUtil.LocVal("SimplePetTooltips.JungleNugget");
        }
    }
}
