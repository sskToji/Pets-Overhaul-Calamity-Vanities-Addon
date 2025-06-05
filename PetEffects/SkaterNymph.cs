using CalamityMod.BiomeManagers;
using CalamityMod.Buffs.StatDebuffs;
using CalValEX.Items.Pets;
using PetsOverhaul.Systems;
using POCalValAddon.Systems;
using Terraria;
using Terraria.ModLoader;

namespace POCalValAddon.PetEffects
{
    public sealed class SkaterNymph : PetEffect
    {
        public override int PetItemID => ModContent.ItemType<AcidLamp>();
        public override PetClasses PetClassPrimary => PetClasses.Utility;

        public float nymphBreath = 0.15f;
        public float nymphHealth = 1f;
        public float nymphDef = 5f;

        public override void PostUpdateMiscEffects()
        {
            if (PetIsEquipped() && Player.InModBiome<SulphurousSeaBiome>())
            {
                Player.breathEffectiveness += nymphBreath;
            }
        }
        public override void UpdateBadLifeRegen()
        {
            if (PetIsEquipped() && Player.HasBuff(ModContent.BuffType<Irradiated>()))
            {
                Player.statDefense += (int)nymphDef;
                Player.lifeRegen += (int)nymphHealth;
            }
        }
        public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
        {
            if (PetIsEquipped() && Player.HasBuff(ModContent.BuffType<Irradiated>()))
            {
                damage += 0.05f;
            }
        }

        public sealed class SkaterNymphPetItem : PetTooltip
        {
            public override PetEffect PetsEffect => skaterNymph;
            public static SkaterNymph skaterNymph
            {
                get
                {
                    if (Main.LocalPlayer.TryGetModPlayer(out SkaterNymph pet))
                        return pet;
                    else
                        return ModContent.GetInstance<SkaterNymph>();
                }
            }
            public override string PetsTooltip => PetUtil.LocVal("PetTooltips.SkaterNymph")
                .Replace("<dmg>", skaterNymph.nymphHealth.ToString())
                .Replace("<def>", skaterNymph.nymphDef.ToString())
                .Replace("<breath>", PetUtil.FloatToPercent(skaterNymph.nymphBreath));
            public override string SimpleTooltip => PetUtil.LocVal("SimplePetTooltips.SkaterNymph");
        }
    }
}
