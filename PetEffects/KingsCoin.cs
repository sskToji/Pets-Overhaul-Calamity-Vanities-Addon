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
using Terraria.DataStructures;
using CalamityMod;
using Terraria.GameInput;
using CalamityMod.Projectiles.Melee;

namespace POCalValAddon.PetEffects
{
    public sealed class KingsCoin : PetEffect
    {
        public override int PetItemID => ModContent.ItemType<TheSeaKingsCoin>();
        public override PetClasses PetClassPrimary => PetClasses.Mobility;
        public override PetClasses PetClassSecondary => PetClasses.Utility;

        public float kingMovement = 0.25f;
        public int kingCooldown = 300;
        public float hpTreshold = 0.2f;
        public float baseHpShield = 0.1f;
        public int driedCooldown = 300;
        public int driedShieldDuration = 900;
        private int lifeguardMultTimer = 0;
        public float moistRadius = 20f;
        public int trigger = -1;
        public int moistDmg = 100;
        public override int PetAbilityCooldown => kingCooldown;

        public override void PostUpdateMiscEffects()
        {
            if (PetIsEquipped() && (Player.ZoneBeach || Player.ZoneDesert))
            {
                Player.moveSpeed += kingMovement;
            }
            if (PetIsEquipped() && trigger >= 0)
            {
                if (trigger % 20 == 0)
                {
                    for (int i = 0; i < 25; i++)
                    {
                        Projectile petProjectile = Projectile.NewProjectileDirect(GlobalPet.GetSource_Pet(EntitySourcePetIDs.PetProjectile), Player.Center + Main.rand.NextVector2CircularEdge(Player.width, Player.height), Main.rand.NextVector2CircularEdge(10, 10), ModContent.ProjectileType<CalamityMod.Projectiles.Boss.WaterSpear>(), Pet.PetDamage(moistDmg, DamageClass.Melee), 0, Player.whoAmI);
                        petProjectile.DamageType = DamageClass.Melee;
                        petProjectile.CritChance = (int)Player.GetTotalCritChance<GenericDamageClass>();
                        petProjectile.friendly = true;
                        petProjectile.hostile = false;
                    }
                }
                trigger--;
            }
        }

        public override void ModifyHurt(ref Player.HurtModifiers modifiers)
        {
            if (PetIsEquipped() && Pet.timer <= 0)
            {
                int shieldAmount = (int)((Player.statLifeMax * baseHpShield));

                modifiers.ModifyHurtInfo += (ref Player.HurtInfo info) =>
                {
                    if (info.Damage >= (Player.statLife - Player.statLifeMax2 * hpTreshold))
                    {
                        int belowHpDamage = (int)(Player.statLifeMax2 * hpTreshold - (Player.statLife - info.Damage));
                        int reduce = info.Damage - 1;

                        reduce = Math.Min(reduce, belowHpDamage);
                        reduce = Math.Min(reduce, shieldAmount);

                        if (reduce > 0)
                        {
                            CombatText.NewText(Player.getRect(), Color.DarkGreen, -reduce);
                        }
                        info.Damage -= reduce;
                        Pet.AddShield(shieldAmount - reduce, driedShieldDuration, false);

                        trigger = 100;
                        Pet.timer = Pet.timerMax;
                        lifeguardMultTimer = driedShieldDuration;
                    }
                };
            }
        }
        public sealed class KingsCoinPetItem : PetTooltip
        {
            public override PetEffect PetsEffect => kingCoin;
            public static KingsCoin kingCoin
            {
                get
                {
                    if (Main.LocalPlayer.TryGetModPlayer(out KingsCoin pet))
                        return pet;
                    else
                        return ModContent.GetInstance<KingsCoin>();
                }
            }
            public override string PetsTooltip => Language.GetTextValue("Mods.POCalValAddon.PetTooltips.KingsCoin")
                .Replace("<keybind>", PetTextsColors.KeybindText(PetKeybinds.PetAbilitySwitch))
                .Replace("<ability>", PetTextsColors.KeybindText(PetKeybinds.UsePetAbility));
        }
    }
}
