using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using CalamityMod;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod.Items.Weapons.Summon;
using CalValEX.Items.Pets.ExoMechs;
using PetsOverhaul.Systems;
using Steamworks;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace POCalValAddon.PetEffects
{
    public sealed class ExoGem : PetEffect
    {
        public override int PetItemID => ModContent.ItemType<ExoGemstone>();
        public override PetClasses PetClassPrimary => PetClasses.Offensive;

        public float gemMeleeDmg = 0.3f;
        public float gemMeleeCrit = 0.25f;
        public float gemRogueDmg = 0.3f;
        public float gemRogueCrit = 0.25f;
        public float gemRangedDmg = 0.3f;
        public float gemRangedUse = 0.4f;
        public float gemSummonDmg = 0.3f;
        public float gemWeaponDmg = 0.15f;
        public float gemWeaponCrit = 0.15f;

        public static List<int> GemMeleeWeapons =
        [
            ModContent.ItemType<PhotonRipper>(),
            ModContent.ItemType<SpineOfThanatos>(),
        ];
        public static List<int> GemRangedWeapons =
        [
            ModContent.ItemType<SurgeDriver>(),
            ModContent.ItemType<TheJailor>(),
        ];
        public static List<int> GemRogueWeapons =
        [
            ModContent.ItemType<TheAtomSplitter>(),
            ModContent.ItemType<RefractionRotor>(),
        ];
        public static List<int> GemSummonWeapons =
        [
            ModContent.ItemType<AresExoskeleton>(),
            ModContent.ItemType<AtlasMunitionsBeacon>(),
        ];

        public override void ModifyWeaponCrit(Item item, ref float crit)
        {
            if (PetIsEquipped() && GemMeleeWeapons.Contains(item.type))
            {
                crit += gemMeleeCrit;
            }
            if (PetIsEquipped() && GemRogueWeapons.Contains(item.type))
            {
                crit += gemRogueCrit;
            }
            if (PetIsEquipped() && (item.CountsAsClass<MeleeDamageClass>() || item.CountsAsClass<RogueDamageClass>()) && !(GemRogueWeapons.Contains(item.type) || GemMeleeWeapons.Contains(item.type)))
            {
                crit += gemWeaponCrit;
            }
        }
        public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
        {
            if (PetIsEquipped() && GemMeleeWeapons.Contains(item.type))
            {
                damage += gemMeleeDmg;
            }
            if (PetIsEquipped() && GemRogueWeapons.Contains(item.type))
            {
                damage += gemRogueDmg;
            }
            if (PetIsEquipped() && GemRangedWeapons.Contains(item.type))
            {
                damage += gemRangedDmg;
            }
            if (PetIsEquipped() && GemSummonWeapons.Contains(item.type))
            {
                damage += gemSummonDmg;
            }
            if (PetIsEquipped() && (item.CountsAsClass<MeleeDamageClass>() || item.CountsAsClass<RogueDamageClass>()) && !(GemRogueWeapons.Contains(item.type) || GemMeleeWeapons.Contains(item.type)))
            {
                damage += gemWeaponDmg;
            }
        }
        public override void PostUpdateMiscEffects()
        {
            if (PetIsEquipped())
            {
                Player.GetAttackSpeed<RangedDamageClass>() += gemRangedUse;
            }
        }
        public sealed class ExoGemPetItem : PetTooltip
        {
            public override PetEffect PetsEffect => gemBabies;
            public static ExoGem gemBabies
            {
                get
                {
                    if (Main.LocalPlayer.TryGetModPlayer(out ExoGem pet))
                        return pet;
                    else
                        return ModContent.GetInstance<ExoGem>();
                }
            }
            public override string PetsTooltip => Language.GetTextValue("Mods.POCalValAddon.PetTooltips.ExoMechs.ExoGemstone");
        }
    }
}
