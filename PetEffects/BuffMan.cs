using CalamityMod.Buffs.DamageOverTime;
using CalValEX.Items.Pets;
using PetsOverhaul.Systems;
using POCalValAddon.Systems;
using Terraria;
using Terraria.ModLoader;

namespace POCalValAddon.PetEffects
{
    public sealed class BuffMan : PetEffect
    {
        public override int PetItemID => ModContent.ItemType<ReaperoidPills>();
        public override PetClasses PetClassPrimary => PetClasses.Melee;
        public override PetClasses PetClassSecondary => PetClasses.Utility;
        public override int PetStackCurrent => currentStacks;
        public override int PetStackMax => dmgStacksMax;
        public override string PetStackText => "stacks";

        internal int currentStacks = 0;
        private int timer = 0;
        public int dmgBuffDuration = 600;
        public int dmgStacksMax = 10;
        public int dmgStacksMaxSandstorm = 15;
        public float dmgIncrease = 0.02f;
        public int crushDepthDuration = 300;

        public override void ExtraPreUpdate()
        {
            if (PetIsEquipped())
            {
                timer--;
                if (timer <= 0)
                {
                    currentStacks = 0;
                    timer = 0;
                }
            }
        }

        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (PetIsEquipped() && item.CountsAsClass<MeleeDamageClass>())
            {
                target.AddBuff(ModContent.BuffType<CrushDepth>(), crushDepthDuration);
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

        public sealed class BuffManPetItem : PetTooltip
        {
            public override PetEffect PetsEffect => buffMan;
            public static BuffMan buffMan
            {
                get
                {
                    if (Main.LocalPlayer.TryGetModPlayer(out BuffMan pet))
                        return pet;
                    else
                        return ModContent.GetInstance<BuffMan>();
                }
            }
            public override string PetsTooltip => PetUtil.LocVal("PetTooltips.BuffMan")
                .Replace("<current>", buffMan.currentStacks.ToString())
                .Replace("<stack>", buffMan.dmgStacksMax.ToString())
                .Replace("<secs>", PetUtil.IntToTime(buffMan.dmgBuffDuration))
                .Replace("<debuff>", PetUtil.IntToTime(buffMan.crushDepthDuration));
            public override string SimpleTooltip => PetUtil.LocVal("SimplePetTooltips.BuffMan");
        }
    }
}
