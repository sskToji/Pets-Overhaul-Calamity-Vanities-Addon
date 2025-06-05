using CalValEX.Items.Pets;
using PetsOverhaul.Systems;
using POCalValAddon.Systems;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;

//Idea by @iamnamedmuffin

namespace POCalValAddon.PetEffects
{
    public sealed class Tara : PetEffect
    {
        public override int PetItemID => ModContent.ItemType<BambooStick>();
        public override PetClasses PetClassPrimary => PetClasses.Utility;

        public float radiusConfuse = 200f;
        public int taraCooldown = 600;
        public int debuffTime = 300;

        public override int PetAbilityCooldown => taraCooldown;

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (Pet.AbilityPressCheck() && PetIsEquipped() && Pet.timer <= 0)
            {
                foreach (NPC item in Main.ActiveNPCs)
                {
                    if (item.Distance(Player.Center) <= radiusConfuse)
                    {
                        item.AddBuff(BuffID.Confused, debuffTime);
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
            public override string PetsTooltip => PetUtil.LocVal("PetTooltips.Tara")
                .Replace("<keybind>", PetTextsColors.KeybindText(PetKeybinds.UsePetAbility))
                .Replace("<px>", redPanda.radiusConfuse.ToString())
                .Replace("<secs>", PetUtil.IntToTime(redPanda.debuffTime))
                .Replace("<cd>", PetUtil.IntToTime(redPanda.taraCooldown));
            public override string SimpleTooltip => PetUtil.LocVal("SimplePetTooltips.Tara").Replace("<keybind>", PetTextsColors.KeybindText(PetKeybinds.UsePetAbility));
        }
    }
}
