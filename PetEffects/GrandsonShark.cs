using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod.Items.Weapons.Summon;
using CalValEX.Items.Pets;
using PetsOverhaul.Systems;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace POCalValAddon.PetEffects
{
    public sealed class GrandsonShark : PetEffect
    {
        public override int PetItemID => ModContent.ItemType<DustyBadge>();
        public override PetClasses PetClassPrimary => PetClasses.Offensive;

        public float sharkDmg = 0.1f;
        public static List<int> GrandScaleWeapons =
        [
            ModContent.ItemType<DuststormInABottle>(),
            ModContent.ItemType<SandSharknadoStaff>(),
            ModContent.ItemType<Sandslasher>(),
            ModContent.ItemType<SandstormGun>(),
            ModContent.ItemType<ShiftingSands>(),
            ModContent.ItemType<Tumbleweed>(),
        ];

        public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
        {
            if (PetIsEquipped() && GrandScaleWeapons.Contains(item.type))
            {
                damage += sharkDmg;
            }
        }

        public sealed class GrandsonSharkPetItem : PetTooltip
        {
            public override PetEffect PetsEffect => grandsonShark;
            public static GrandsonShark grandsonShark
            {
                get
                {
                    if (Main.LocalPlayer.TryGetModPlayer(out GrandsonShark pet))
                        return pet;
                    else
                        return ModContent.GetInstance<GrandsonShark>();
                }
            }
            public override string PetsTooltip => Language.GetTextValue("Mods.POCalValAddon.PetTooltips.GrandsonShark");
        }
    }
}
