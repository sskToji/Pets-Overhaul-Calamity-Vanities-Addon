using CalamityMod.Items.Weapons.Summon;
using CalValEX;
using CalValEX.Items.Pets;
using PetsOverhaul.Systems;
using POCalValAddon.Systems;
using Terraria;
using Terraria.ModLoader;

namespace POCalValAddon.PetEffects
{
    public sealed class EggBaby : PetEffect
    {
        public override int PetItemID => ModContent.ItemType<AstraEGGeldon>();
        public override PetClasses PetClassPrimary => PetClasses.Summoner;

        public float summonDmg = 0.2f;


        public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
        {
            if (PetIsEquipped() && Player.HasItem(ModContent.ItemType<AbandonedSlimeStaff>()))
            {
                damage += summonDmg;
            }
        }

        public override void ExtraPreUpdate()
        {
            if (PetIsEquipped() && Player.HasItem(ModContent.ItemType<AbandonedSlimeStaff>()))
            {
                Player.GetModPlayer<CalValEXPlayer>().CalamityBABYBool = false;
            }
        }
        public sealed class EggBabyPetItem : PetTooltip
        {
            public override PetEffect PetsEffect => eggBaby;
            public static EggBaby eggBaby
            {
                get
                {
                    if (Main.LocalPlayer.TryGetModPlayer(out EggBaby pet))
                        return pet;
                    else
                        return ModContent.GetInstance<EggBaby>();
                }
            }
            public override string PetsTooltip => PetUtil.LocVal("PetTooltips.EggBaby")
                .Replace("<dmg>", PetUtil.FloatToPercent(eggBaby.summonDmg));
            public override string SimpleTooltip => PetUtil.LocVal("SimplePetTooltips.EggBaby");
        }
    }
}
