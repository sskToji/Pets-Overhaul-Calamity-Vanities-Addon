using CalValEX.Items.Pets;
using PetsOverhaul.Systems;
using POCalValAddon.Systems;
using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;

namespace POCalValAddon.PetEffects
{
    public sealed class MoistPest : PetEffect
    {
        public override int PetItemID => ModContent.ItemType<MoistLocket>();
        public override PetClasses PetClassPrimary => PetClasses.Offensive;
        public override PetClasses PetClassSecondary => PetClasses.Mobility;

        public int moistCooldown = 300;
        public float moistRadius = 20f;
        public int moistDmg = 100;
        public float beachMovement = 0.20f;
        public override int PetAbilityCooldown => moistCooldown;

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (Pet.AbilityPressCheck() && PetIsEquipped())
            {
                if (Pet.timer <= 0)
                {
                    for (int i = 0; i < 25; i++)
                    {
                        Projectile petProjectile = Projectile.NewProjectileDirect(GlobalPet.GetSource_Pet(EntitySourcePetIDs.PetProjectile), Player.Center + Main.rand.NextVector2CircularEdge(Player.width, Player.height), Main.rand.NextVector2CircularEdge(10, 10), ModContent.ProjectileType<CalamityMod.Projectiles.Boss.WaterSpear>(), Pet.PetDamage(moistDmg, DamageClass.Melee), 0, Player.whoAmI);
                        petProjectile.DamageType = DamageClass.Melee;
                        petProjectile.CritChance = (int)Player.GetTotalCritChance<GenericDamageClass>();
                        petProjectile.friendly = true;
                        petProjectile.hostile = false;
                    }
                    Pet.timer = Pet.timerMax;
                }
            }
        }
        public override void PostUpdateMiscEffects()
        {
            if (PetIsEquipped() && Player.ZoneBeach)
            {
                Player.moveSpeed += beachMovement;
            }
        }

        public sealed class MoistPestPetItem : PetTooltip
        {
            public override PetEffect PetsEffect => babyMoist;
            public static MoistPest babyMoist
            {
                get
                {
                    if (Main.LocalPlayer.TryGetModPlayer(out MoistPest pet))
                        return pet;
                    else
                        return ModContent.GetInstance<MoistPest>();
                }
            }
            public override string PetsTooltip => PetUtil.LocVal("PetTooltips.MoistPest")
                .Replace("<keybind>", PetTextsColors.KeybindText(PetKeybinds.UsePetAbility))
                .Replace("<dmg>", babyMoist.moistDmg.ToString())
                .Replace("<cd>", PetUtil.IntToTime(babyMoist.moistCooldown))
                .Replace("<speed>", PetUtil.FloatToPercent(babyMoist.beachMovement));
            public override string SimpleTooltip => PetUtil.LocVal("SimplePetTooltips.MoistPest").Replace("<keybind>", PetTextsColors.KeybindText(PetKeybinds.UsePetAbility));
        }
    }
}
