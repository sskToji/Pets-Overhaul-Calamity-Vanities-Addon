﻿using CalValEX.Items.Pets;
using PetsOverhaul.Systems;
using POCalValAddon.Systems;
using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;

namespace POCalValAddon.PetEffects
{
    public sealed class BabySignus : PetEffect
    {
        public override int PetItemID => ModContent.ItemType<ShadowCloth>();
        public override PetClasses PetClassPrimary => PetClasses.Offensive;
        public override PetClasses PetClassSecondary => PetClasses.Summoner;

        public int signusActiveDmg = 300;
        public int signusCooldown = 600;
        public float signusSummonDmg = 0.15f;

        public override int PetAbilityCooldown => signusCooldown;

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (Pet.AbilityPressCheck() && PetIsEquipped())
            {
                if (Pet.timer <= 0)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        Projectile petProjectile = Projectile.NewProjectileDirect(GlobalPet.GetSource_Pet(EntitySourcePetIDs.PetProjectile), Player.Center + Main.rand.NextVector2CircularEdge(Player.width, Player.height), Main.rand.NextVector2CircularEdge(10, 10), ModContent.ProjectileType<CalamityMod.Projectiles.Boss.SignusScythe>(), Pet.PetDamage(signusActiveDmg, DamageClass.Summon), 0, Player.whoAmI);
                        petProjectile.DamageType = DamageClass.Summon;
                        petProjectile.CritChance = (int)Player.GetTotalCritChance<GenericDamageClass>();
                        petProjectile.friendly = true;
                        petProjectile.hostile = false;
                    }
                    Pet.timer = Pet.timerMax;
                }
            }
        }
        public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
        {
            if (PetIsEquipped() && item.CountsAsClass<SummonDamageClass>())
            {
                damage += signusSummonDmg;
            }
        }

        public sealed class BabySignusPetItem : PetTooltip
        {
            public override PetEffect PetsEffect => babySignus;
            public static BabySignus babySignus
            {
                get
                {
                    if (Main.LocalPlayer.TryGetModPlayer(out BabySignus pet))
                        return pet;
                    else
                        return ModContent.GetInstance<BabySignus>();
                }
            }
            public override string PetsTooltip => PetUtil.LocVal("PetTooltips.BabySignus")
                .Replace("<keybind>", PetTextsColors.KeybindText(PetKeybinds.UsePetAbility))
                .Replace("<summon>", PetUtil.FloatToPercent(babySignus.signusSummonDmg))
                .Replace("<cd>", PetUtil.IntToTime(babySignus.signusCooldown))
                .Replace("<summon>", babySignus.signusActiveDmg.ToString());
            public override string SimpleTooltip => PetUtil.LocVal("SimplePetTooltips.BabySignus").Replace("<keybind>", PetTextsColors.KeybindText(PetKeybinds.UsePetAbility));
        }
    }
}
