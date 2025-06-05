using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Buffs.StatDebuffs;
using CalValEX.Items.Pets;
using PetsOverhaul.Systems;
using POCalValAddon.Systems;
using Terraria;
using Terraria.ModLoader;

namespace POCalValAddon.PetEffects
{
    public sealed class SharkHeart : PetEffect
    {
        public override int PetItemID => ModContent.ItemType<HeartoftheSharks>();
        public override PetClasses PetClassPrimary => PetClasses.Melee;

        internal int currentStacks = 0;
        private int timer = 0;
        public int dmgBuffDuration = 600;
        public int dmgStacksMax;
        public int dmgStacksMaxSandstorm = 15;
        public int dmgStacksMaxNormal = 10;
        public float dmgIncrease = 0.02f;
        public int debuffDuration = 300;

        public override int PetStackCurrent => currentStacks;
        public override int PetStackMax => dmgStacksMax;
        public override string PetStackText => "damage stacks";

        public override void ExtraPreUpdate()
        {
            timer--;
            if (timer <= 0)
            {
                currentStacks = 0;
                timer = 0;
            }
            if (Player.ZoneSandstorm is true)
            {
                dmgStacksMax = dmgStacksMaxSandstorm;
            }
            else
            {
                dmgStacksMax = dmgStacksMaxNormal;
            }
            if (currentStacks > dmgStacksMax)
            {
                currentStacks = dmgStacksMax;
            }
        }
        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (PetIsEquipped() && item.CountsAsClass<MeleeDamageClass>())
            {
                target.AddBuff(ModContent.BuffType<Irradiated>(), debuffDuration);
                target.AddBuff(ModContent.BuffType<CrushDepth>(), debuffDuration);
                if (currentStacks < dmgStacksMax)
                {
                    currentStacks++;
                }
                timer = dmgBuffDuration;
            }
        }

        public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
        {
            if (PetIsEquipped() && item.CountsAsClass<MeleeDamageClass>())
            {
                if (currentStacks > 0)
                {
                    damage += dmgIncrease * currentStacks;
                }
            }
        }

        public override void ModifyHurt(ref Player.HurtModifiers modifiers)
        {
            if (PetIsEquipped() && currentStacks > 0)
            {
                currentStacks = 0;
                timer = 0;
            }
        }

        public sealed class SharkHeartPetItem : PetTooltip
        {
            public override PetEffect PetsEffect => sharkHeart;
            public static SharkHeart sharkHeart
            {
                get
                {
                    if (Main.LocalPlayer.TryGetModPlayer(out SharkHeart pet))
                        return pet;
                    else
                        return ModContent.GetInstance<SharkHeart>();
                }
            }
            public override string PetsTooltip => PetUtil.LocVal("PetTooltips.SharkHeart")
                .Replace("<dmg>", PetUtil.FloatToPercent(sharkHeart.dmgIncrease))
                .Replace("<stacks>", sharkHeart.currentStacks.ToString())
                .Replace("<maxStack>", sharkHeart.dmgStacksMaxNormal.ToString())
                .Replace("<sandstorm>", sharkHeart.dmgStacksMaxSandstorm.ToString())
                .Replace("<lose>", PetUtil.IntToTime(sharkHeart.dmgBuffDuration))
                .Replace("<secs>", PetUtil.IntToTime(sharkHeart.debuffDuration));
            public override string SimpleTooltip => PetUtil.LocVal("SimplePetTooltips.SharkHeart");
        }
    }
}
