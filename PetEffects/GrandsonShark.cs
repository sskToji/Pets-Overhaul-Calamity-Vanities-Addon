using CalValEX.Items.Pets;
using PetsOverhaul.Systems;
using POCalValAddon.Systems;
using Terraria;
using Terraria.ModLoader;

namespace POCalValAddon.PetEffects
{
    public sealed class GrandsonShark : PetEffect
    {
        public override int PetItemID => ModContent.ItemType<DustyBadge>();
        public override PetClasses PetClassPrimary => PetClasses.Offensive;

        public float sharkDmg = 0.1f;

        public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
        {
            if (PetIsEquipped() && CalValItemSets.GrandScaleWeapons[item.type])
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
            public override string PetsTooltip => PetUtil.LocVal("PetTooltips.GrandsonShark")
                .Replace("<dmg>", PetUtil.FloatToPercent(grandsonShark.sharkDmg));
            public override string SimpleTooltip => PetUtil.LocVal("SimplePetTooltips.GrandsonShark");
        }
    }
}
