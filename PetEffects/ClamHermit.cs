using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalamityMod.Items.Accessories;
using CalValEX.Items.Pets;
using PetsOverhaul.Systems;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace POCalValAddon.PetEffects
{
    public sealed class ClamHermit : PetEffect
    {
        public override int PetItemID => ModContent.ItemType<ClamHermitMedallion>();
        public override PetClasses PetClassPrimary => PetClasses.Utility;
        public override PetClasses PetClassSecondary => PetClasses.Offensive;

        public int clamBreath = 50; //200 is base, so 50 is a 25% increase
        public float clamDmg = 0.1f;

        public override void PostUpdateMiscEffects()
        {
            if (PetIsEquipped())
            {
                Player.breathMax += clamBreath;
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
            public override string PetsTooltip => Language.GetTextValue("Mods.POCalValAddon.PetTooltips.ClamHermit");
        }
    }
}
