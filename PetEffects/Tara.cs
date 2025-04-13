using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetsOverhaul.Systems;
using Terraria.ModLoader;
using CalValEX.Items.Pets;
using Terraria.ID;
using Terraria;
using Terraria.Localization;
using Terraria.GameInput;

//Idea by @iamnamedmuffin

namespace POCalValAddon.PetEffects
{
    public sealed class Tara : PetEffect
    {
        public override int PetItemID => ModContent.ItemType<BambooStick>();
        public override PetClasses PetClassPrimary => PetClasses.Utility;

        public float radiusConfuse = 200f;
        public int taraCooldown = 600;

        public override int PetAbilityCooldown => taraCooldown;

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (Pet.AbilityPressCheck() && PetIsEquipped() && Pet.timer <= 0)
            {
                foreach (NPC item in Main.ActiveNPCs)
                {
                    if (item.Distance(Player.Center) <= radiusConfuse)
                    {
                        item.AddBuff(BuffID.Confused, 300);
                        Pet.timer = Pet.timerMax;
                    }
                }
            }
        }
        public override void PostUpdateMiscEffects()
        {
            if (PetIsEquipped())
            {
                GlobalPet.CircularDustEffect(Player.Center, DustID.OrangeTorch, (int)radiusConfuse, dustAmount: 128);
            }
        }

        public sealed class TaraPetItem : PetTooltip
        {
            public override PetEffect PetsEffect => redPanda;
            public static Tara redPanda
            {
                get
                {
                    if (Main.LocalPlayer.TryGetModPlayer(out Tara pet))
                        return pet;
                    else
                        return ModContent.GetInstance<Tara>();
                }
            }
            public override string PetsTooltip => Language.GetTextValue("Mods.POCalValAddon.PetTooltips.Tara")
                .Replace("<keybind>", PetTextsColors.KeybindText(PetKeybinds.UsePetAbility));
        }
    }
}
