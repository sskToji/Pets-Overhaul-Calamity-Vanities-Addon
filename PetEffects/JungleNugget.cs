using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Items.Weapons.Rogue;
using CalValEX.Items.Pets;
using PetsOverhaul.Systems;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace POCalValAddon.PetEffects
{
    public sealed class JungleNugget : PetEffect
    {
        public override int PetItemID => ModContent.ItemType<NuggetinaBiscuit>();
        public override PetClasses PetClassPrimary => PetClasses.Rogue;
        public override PetClasses PetClassSecondary => PetClasses.Melee;

        public float nuggetDmg = 0.2f;
        public int debuffTime = 300;
        public List<int> NuggetWeapons =
        [
            ModContent.ItemType<TheBurningSky>(),
            ModContent.ItemType<DragonRage>(),
            ModContent.ItemType<TheFinalDawn>(),
            ModContent.ItemType<Wrathwing>()
        ];

        public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
        {
            if (PetIsEquipped() && NuggetWeapons.Contains(item.type))
            {
                damage += nuggetDmg;
            }
            if (PetIsEquipped() && !NuggetWeapons.Contains(item.type) && (item.CountsAsClass<MeleeDamageClass>() || item.CountsAsClass<RogueDamageClass>()))
            {
                damage -= nuggetDmg;
            }
        }
        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (PetIsEquipped() && !NuggetWeapons.Contains(item.type) && item.CountsAsClass<MeleeDamageClass>())
            {
                target.AddBuff(ModContent.BuffType<Dragonfire>(), debuffTime);
            }
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (PetIsEquipped() && !NuggetWeapons.Contains(proj.type) && proj.CountsAsClass<RogueDamageClass>())
            {
                target.AddBuff(ModContent.BuffType<Dragonfire>(), debuffTime);
            }
        }
        public sealed class JungleNuggetPetItem : PetTooltip
        {
            public override PetEffect PetsEffect => jungleNugget;
            public static JungleNugget jungleNugget
            {
                get
                {
                    if (Main.LocalPlayer.TryGetModPlayer(out JungleNugget pet))
                        return pet;
                    else
                        return ModContent.GetInstance<JungleNugget>();
                }
            }
            public override string PetsTooltip => Language.GetTextValue("Mods.POCalValAddon.PetTooltips.JungleNugget");
        }
    }
}
