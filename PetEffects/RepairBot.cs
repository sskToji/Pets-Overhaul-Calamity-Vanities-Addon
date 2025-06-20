﻿using System;
using CalValEX.Items.Pets;
using PetsOverhaul.Config;
using PetsOverhaul.Systems;
using POCalValAddon.Systems;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

//Heartsteel based pet item.
//TODO : Ask bugra about being able to show current hp and def increase on pet tooltip, not sure if this is possible

namespace POCalValAddon.PetEffects
{
    public sealed class RepairBot : PetEffect
    {
        public override int PetItemID => ModContent.ItemType<RepurposedMonitor>();
        public override PetClasses PetClassPrimary => PetClasses.Defensive;
        public override int PetAbilityCooldown => heartsteelCooldown;
        public override int PetStackCurrent => heartsteelStacks;
        public override int PetStackMax => heartsteelStacksMax;
        public override string PetStackText => "stacks";

        public int heartsteelHealthIncrease = 20;
        public int heartsteelDefIncrease = 3;
        public int heartsteelCooldown = 1200;
        public int heartsteelStackLose = 100;
        public int heartsteelNoHitCooldown = 600;
        public int heartsteelDamageDone;

        public int heartsteelStacks = 0;
        public int heartsteelStacksMax
        {
            get
            {
                if (NPC.downedMoonlord == true)
                {
                    return 1000000;
                }
                if (NPC.downedPlantBoss == true)
                {
                    return 1000;
                }
                if (Main.hardMode == true)
                {
                    return 500;
                }
                if (NPC.downedBoss1 == true)
                {
                    return 250;
                }
                return 100;
            }
        }
        public float heartsteelRadius = 160f;


        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (Pet.AbilityPressCheck() && PetIsEquipped())
            {
                if (Pet.timer <= 0)
                {
                    bool hit = false;
                    foreach (NPC item in Main.ActiveNPCs)
                    {
                        if (item.Distance(Player.Center) <= heartsteelRadius && !(item.friendly || item.dontTakeDamage || item.CountsAsACritter))
                        {
                            DoHeartSteelDamage(item);
                            heartsteelStacks++;
                            hit = true;
                        }
                    }
                    if (hit == true)
                    {
                        Pet.timer = Pet.timerMax;
                        if (ModContent.GetInstance<PetPersonalization>().AbilitySoundEnabled)
                        {
                            SoundEngine.PlaySound(SoundID.DeerclopsIceAttack with { Pitch = 0.5f, Volume = 0.8f, PitchVariance = 0, MaxInstances = 0 }, Player.Center);
                        }
                    }
                    else
                    {
                        Pet.timer = Pet.timerMax / 2;
                    }
                }
            }
        }

        public void DoHeartSteelDamage(NPC npc)
        {
            float damageDealt = npc.lifeMax * 0.05f;
            npc.SimpleStrikeNPC(Pet.PetDamage(damageDealt, DamageClass.Generic), Player.direction, false, 0, DamageClass.Generic, true, Player.luck);
        }

        public override void PostUpdateMiscEffects()
        {
            if (PetIsEquipped())
            {
                int cappedStack = Math.Clamp(heartsteelStacks, 0, heartsteelStacksMax);
                GlobalPet.CircularDustEffect(Player.Center, DustID.Torch, (int)heartsteelRadius, dustAmount: 64);
                if (heartsteelStacks > 0)
                {
                    Player.statDefense += (int)(Math.Log10(cappedStack) * heartsteelDefIncrease);
                    Player.statLifeMax2 += (int)(Math.Log10(cappedStack) * heartsteelHealthIncrease);
                }
            }
        }

        public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
        {
            if (PetIsEquipped())
            {
                heartsteelStacks -= heartsteelStackLose;
            }
        }

        public override void SaveData(TagCompound tag)
        {
            tag.Add("heartsteelStacks", heartsteelStacks);
        }

        public override void LoadData(TagCompound tag)
        {
            if (tag.TryGet("heartsteelStacks", out int stack))
            {
                heartsteelStacks = stack;
            }
        }

        public override void ExtraPreUpdateNoCheck()
        {
            if (heartsteelStacks < 0)
            {
                heartsteelStacks = 0;
            }
        }

        public sealed class RepairBotPetItem : PetTooltip
        {
            public override PetEffect PetsEffect => repairBot;
            public static RepairBot repairBot
            {
                get
                {
                    if (Main.LocalPlayer.TryGetModPlayer(out RepairBot pet))
                        return pet;
                    else
                        return ModContent.GetInstance<RepairBot>();
                }
            }
            public override string PetsTooltip => PetUtil.LocVal("PetTooltips.RepairBot")
                .Replace("<keybind>", PetTextsColors.KeybindText(PetKeybinds.UsePetAbility))
                .Replace("<stacks>", repairBot.heartsteelStacks.ToString())
                .Replace("<hp>", repairBot.heartsteelHealthIncrease.ToString())
                .Replace("<def>", repairBot.heartsteelDefIncrease.ToString())
                .Replace("<lose>", repairBot.heartsteelStackLose.ToString())
                .Replace("<cd>", PetUtil.IntToTime(repairBot.heartsteelCooldown))
                .Replace("<halfCd>", PetUtil.IntToTime(repairBot.heartsteelNoHitCooldown));
            public override string SimpleTooltip => PetUtil.LocVal("SimplePetTooltips.RepairBot").Replace("<keybind>", PetTextsColors.KeybindText(PetKeybinds.UsePetAbility));
        }
    }
}
