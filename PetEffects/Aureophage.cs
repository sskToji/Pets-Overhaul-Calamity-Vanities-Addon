using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalamityMod.Buffs.DamageOverTime;
using CalValEX.Items.Pets;
using PetsOverhaul.Systems;
using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;
using POCalValAddon.Systems;
using CalamityMod;
using CalamityMod.Dusts;

namespace POCalValAddon.PetEffects
{
    public sealed class Aureophage : CVAPetEffect
    {
        public override int PetItemID => ModContent.ItemType<AstralInfectedIcosahedron>();
        public override PetClasses PetClassPrimary => PetClasses.Offensive;
        public override int PetAbilityCooldown => astralTimer;

        public int astralTimer = 1800;
        public float astralRadius = 450f;
        public float astralDebuffTimer = 300f;

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (Pet.AbilityPressCheck() && PetIsEquipped())
            {
                if (Pet.timer <= 0)
                {
                    foreach (NPC item in Main.ActiveNPCs)
                    {
                        if (item.Distance(Player.Center) <= astralRadius && !(item.friendly || item.CountsAsACritter || item.dontTakeDamage))
                        {
                            item.AddBuff(ModContent.BuffType<AstralInfectionDebuff>(), (int)astralDebuffTimer);
                        }
                    }
                    Pet.timer = Pet.timerMax;
                }
            }
        }
        public void EnemyHitEnemy(NPC npc)
        {
            if (PetIsEquipped() && npc.TryGetGlobalNPC(out EnemyTouchedEnemy touch) && touch.isTouching > -1 && npc.Calamity().astralInfection > 0)
            {
                Main.npc[touch.isTouching].AddBuff(ModContent.BuffType<AstralInfectionDebuff>(), (int)astralDebuffTimer);
                // touch.isTouching = -1; (for testing with bugra purposes, comment if statement in postupdatemisceffects)
            }
        }
        public override void PostUpdateMiscEffects()
        {
            GlobalPet.CircularDustEffect(Player.Center, ModContent.DustType<AstralBasic>(), (int)astralRadius, dustAmount: 32);
            foreach (NPC item in Main.ActiveNPCs)
            {
                EnemyHitEnemy(item);
                if (item.TryGetGlobalNPC(out EnemyTouchedEnemy touch) && touch.isTouching > -1)
                {
                    touch.isTouching = -1;
                }
            }
        }

        public sealed class AureophagePetItem : PetTooltip
        {
            public override PetEffect PetsEffect => icosahedron;
            public static Aureophage icosahedron
            {
                get
                {
                    if (Main.LocalPlayer.TryGetModPlayer(out Aureophage pet))
                        return pet;
                    else
                        return ModContent.GetInstance<Aureophage>();
                }
            }
            public override string PetsTooltip => PetUtil.LocVal("PetTooltips.Aureophage")
                .Replace("<keybind>", PetTextsColors.KeybindText(PetKeybinds.UsePetAbility))
                .Replace("<secs>", PetUtil.IntToTime((int)icosahedron.astralDebuffTimer));
            public override string SimpleTooltip => PetUtil.LocVal("SimplePetTooltips.Aureophage")
                .Replace("<keybind>", PetTextsColors.KeybindText(PetKeybinds.UsePetAbility));
        }
    }
}
