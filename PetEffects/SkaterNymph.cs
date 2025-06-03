using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalamityMod.BiomeManagers;
using CalamityMod.Buffs.StatDebuffs;
using CalValEX.Items.Pets;
using PetsOverhaul.Systems;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace POCalValAddon.PetEffects
{
    public sealed class SkaterNymph : PetEffect
    {
        public override int PetItemID => ModContent.ItemType<AcidLamp>();
        public override PetClasses PetClassPrimary => PetClasses.Utility;

        public int nymphBreath = 75;

        public override void PostUpdateMiscEffects()
        {
            if (PetIsEquipped() && Player.InModBiome<SulphurousSeaBiome>())
            {
                Player.breathMax += nymphBreath;
            }
        }
        public override void UpdateBadLifeRegen()
        {
            if (PetIsEquipped() && Player.HasBuff(ModContent.BuffType<Irradiated>()))
            {
                Player.statDefense += 5;
                Player.lifeRegen += 1;
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
            public override string PetsTooltip => Language.GetTextValue("Mods.POCalValAddon.PetTooltips.SkaterNymph");
        }
    }
}
