using System;
using Microsoft.Xna.Framework;
using Steamworks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using PetsOverhaul;
using PetsOverhaul.Items;
using PetsOverhaul.Systems;
using CalValEX.Items.Pets;
using System.Security.Cryptography.X509Certificates;

namespace POCalValAddon.PetEffects
{
    public sealed class BabyRavager : PetEffect
    {
        public override int PetItemID => ModContent.ItemType<SkullCluster>();
        public override PetClasses PetClassPrimary => PetClasses.Melee;

        public float ravaDmg = 0.1f;
        public float ravaCrit = 0.05f;
        public float ravaHurt = 1.2f;
        public float ravaMult = 0.01f;
        public float ravaDmgTotal;

        public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
        {
            ravaDmgTotal = (Player.statLife + Player.statDefense) * ravaDmg;
            if (PetIsEquipped() && item.noMelee == false)
            {
                damage += ravaDmgTotal * ravaMult;
            }
        }
        public override void ModifyWeaponCrit(Item item, ref float crit)
        {
            if (PetIsEquipped() && item.noMelee == false)
            {
                crit += (Player.statLifeMax2 + Player.statDefense) * ravaCrit;
            }
        }

        public override void ModifyHurt(ref Player.HurtModifiers modifiers)
        {
            if (PetIsEquipped())
            {
                modifiers.FinalDamage *= ravaHurt;
            }
        }

       public sealed class BabyRavagerPetItem : PetTooltip
        {
            public override PetEffect PetsEffect => babyRava;
            public static BabyRavager babyRava
            {
                get
                {
                    if (Main.LocalPlayer.TryGetModPlayer(out BabyRavager pet))
                        return pet;
                    else
                        return ModContent.GetInstance<BabyRavager>();
                }
            }
            public override string PetsTooltip => Language.GetTextValue("Mods.POCalValAddon.PetTooltips.BabyRavager");
        }
    }
}
