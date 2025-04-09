using System;
using System.Collections.Generic;
using PetsOverhaul;
using PetsOverhaul.Items;
using PetsOverhaul.Systems;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using CalValEX;
using CalValEX.Items.Pets.Scuttlers;
using System.Security.Cryptography.X509Certificates;
using POCalValAddon.Systems;
using PetsOverhaul.PetEffects;

namespace POCalValAddon.PetEffects
{
    public sealed class CrystalScuttle : PetEffect
    {
        public override int PetItemID => ModContent.ItemType<CrystalGeode>();
        public override PetClasses PetClassPrimary => PetClasses.Defensive;
        public override PetClasses PetClassSecondary => PetClasses.Offensive;

        public int defenseStat = 10;
        public float ammoBuff = 0.25f;
        public float staffBuff = 0.25f;

        public override void PostUpdateMiscEffects()
        {
            if (PetIsEquipped())
            {
                Player.statDefense += defenseStat;
            }
        }

        public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
        {
            if (PetIsEquipped() && (item.type == ItemID.CrystalVileShard || item.type == ItemID.CrystalSerpent))
            {
                damage += staffBuff;
            }
            if (PetIsEquipped() && (item.type == ItemID.CrystalBullet || item.type == ItemID.CrystalDart))
            {
                damage += ammoBuff;
            }
        }

        public sealed class CrystalScuttlePetItem : PetTooltip
        {
            public override PetEffect PetsEffect => cryScuttle;
            public static CrystalScuttle cryScuttle
            {
                get
                {
                    if (Main.LocalPlayer.TryGetModPlayer(out CrystalScuttle pet))
                        return pet;
                    else
                        return ModContent.GetInstance<CrystalScuttle>();
                }
            }
            public override string PetsTooltip => Language.GetTextValue("Mods.POCalValAddon.PetTooltips.Scuttlers.CrystalScuttle");
        }
    }
}
