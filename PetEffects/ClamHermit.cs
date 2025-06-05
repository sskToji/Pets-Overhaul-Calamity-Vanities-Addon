using CalValEX.Items.Pets;
using PetsOverhaul.Systems;
using POCalValAddon.Systems;
using Terraria;
using Terraria.ModLoader;

namespace POCalValAddon.PetEffects
{
    public sealed class ClamHermit : PetEffect
    {
        public override int PetItemID => ModContent.ItemType<ClamHermitMedallion>();
        public override PetClasses PetClassPrimary => PetClasses.Utility;
        public override PetClasses PetClassSecondary => PetClasses.Offensive;

        public float clamBreath = 0.25f;
        public float clamDmg = 0.1f;

        public override void PostUpdateMiscEffects()
        {
            if (PetIsEquipped())
            {
                Player.breathEffectiveness += clamBreath;
            }
        }

        public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
        {
            if (PetIsEquipped() && (Player.wet == true) && (Player.lavaWet == false) && (Player.honeyWet == false) && (Player.shimmerWet == false))
            {
                damage += clamDmg;
            }
        }

        public sealed class ClamHermitPetItem : PetTooltip
        {
            public override PetEffect PetsEffect => clamHermit;
            public static ClamHermit clamHermit
            {
                get
                {
                    if (Main.LocalPlayer.TryGetModPlayer(out ClamHermit pet))
                        return pet;
                    else
                        return ModContent.GetInstance<ClamHermit>();
                }
            }
            public override string PetsTooltip => PetUtil.LocVal("PetTooltips.ClamHermit")
                .Replace("<breath>", PetUtil.FloatToPercent(clamHermit.clamBreath))
                .Replace("<dmg>", PetUtil.FloatToPercent(clamHermit.clamDmg));
            public override string SimpleTooltip => PetUtil.LocVal("SimplePetTooltips.ClamHermit");
        }
    }
}
