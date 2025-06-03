using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using CalValEX.Items.Pets;
using PetsOverhaul.Systems;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace POCalValAddon.PetEffects
{
    public sealed class WormTumor : PetEffect
    {
        public override int PetItemID => ModContent.ItemType<MeatyWormTumor>();
        public override PetClasses PetClassPrimary => PetClasses.Melee;
        public override PetClasses PetClassSecondary => PetClasses.Rogue;

        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (PetIsEquipped() && item.CountsAsClass<MeleeDamageClass>())
            {
                target.AddBuff(ModContent.BuffType<BurningBlood>(), 300);
                target.AddBuff(BuffID.Ichor, 300);
            }
        }
        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (PetIsEquipped() && (proj.CountsAsClass<RogueDamageClass>() || proj.CountsAsClass<MeleeDamageClass>()))
            {
                target.AddBuff(ModContent.BuffType<BurningBlood>(), 150);
                target.AddBuff(BuffID.Ichor, 150);
            }
        }

        public sealed class WormTumorPetItem : PetTooltip
        {
            public override PetEffect PetsEffect => wormTumor;
            public static WormTumor wormTumor
            {
                get
                {
                    if (Main.LocalPlayer.TryGetModPlayer(out WormTumor pet))
                        return pet;
                    else
                        return ModContent.GetInstance<WormTumor>();
                }
            }
            public override string PetsTooltip => Language.GetTextValue("Mods.POCalValAddon.PetTooltips.WormTumor");
        }
    }
}
