using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalValEX.Items.Pets;
using PetsOverhaul.Systems;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace POCalValAddon.PetEffects
{
    public sealed class MirrorOrb : PetEffect
    {
        public override int PetItemID => ModContent.ItemType<MirrorMatter>();
        public override PetClasses PetClassPrimary => PetClasses.Defensive;

        public int mirrorTimer = 0;
        public int mirrorTimerMax = 300;
        public int mirrorCooldown = 7200;
        public bool mirrorReducOn = false;
        public float mirrorReduction = 0.6f;

        public override int PetAbilityCooldown => mirrorCooldown;

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (Pet.AbilityPressCheck() && PetIsEquipped())
            {
                if (Pet.timer <= 0)
                {
                    mirrorReducOn = true;
                    Pet.timer = Pet.timerMax;
                }
            }
        }
        public override void ModifyHurt(ref Player.HurtModifiers modifiers)
        {
            if (PetIsEquipped() && mirrorReducOn == true)
            {
                modifiers.FinalDamage *= mirrorReduction; //45% Damage Reduction
                Player.thorns += 6;
                mirrorTimer++;
                if (mirrorTimer >= mirrorTimerMax)
                {
                    mirrorReducOn = false;
                    mirrorTimer = 0;
                }
            }
        }

        public sealed class MirrorOrbPetItem : PetTooltip
        {
            public override PetEffect PetsEffect => mirrorOrb;
            public static MirrorOrb mirrorOrb
            {
                get
                {
                    if (Main.LocalPlayer.TryGetModPlayer(out MirrorOrb pet))
                        return pet;
                    else
                        return ModContent.GetInstance<MirrorOrb>();
                }
            }
            public override string PetsTooltip => Language.GetTextValue("Mods.POCalValAddon.PetTooltips.MirrorOrb")
                .Replace("<keybind>", PetTextsColors.KeybindText(PetKeybinds.UsePetAbility));
        }
    }
}
