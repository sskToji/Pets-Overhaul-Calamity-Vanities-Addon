using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalValEX.Items.Pets;
using PetsOverhaul.Systems;
using Terraria;
using Terraria.GameInput;
using Terraria.Localization;
using Terraria.ModLoader;

//change wing to speed, not time

namespace POCalValAddon.PetEffects
{
    public sealed class StasisDrone : PetEffect
    {
        public override int PetItemID => ModContent.ItemType<ArmoredScrap>();
        public override PetClasses PetClassPrimary => PetClasses.Mobility;

        public float droneMovespeed = 0.2f;
        public float droneAccelspeed = 0.2f;
        public float droneWingSpeed = 0.2f;

        public override void PostUpdateMiscEffects()
        {
            if (PetIsEquipped() && Player.ZoneRain is not false)
            {
                Player.moveSpeed += droneMovespeed;
            }
        }
        public override void PostUpdateRunSpeeds()
        {
            if (PetIsEquipped() && Player.ZoneRain is not false)
            {
                Player.runAcceleration *= droneAccelspeed + 1f;
                
            }
        }
        public sealed class StasisDroneWing : GlobalItem
        {
            public override bool InstancePerEntity => true;

            public override void HorizontalWingSpeeds(Item item, Player player, ref float speed, ref float acceleration)
            {
                if (player.TryGetModPlayer(out StasisDrone stasisDrone) && player.GetModPlayer<GlobalPet>().PetInUseWithSwapCd(ModContent.ItemType<ArmoredScrap>()) && player.ZoneRain is not false)
                {
                    speed += stasisDrone.droneWingSpeed;
                }
            }
        }

        public sealed class StasisDronePetItem : PetTooltip
        {
            public override PetEffect PetsEffect => stasisDrone;
            public static StasisDrone stasisDrone
            {
                get
                {
                    if (Main.LocalPlayer.TryGetModPlayer(out StasisDrone pet))
                        return pet;
                    else
                        return ModContent.GetInstance<StasisDrone>();
                }
            }
            public override string PetsTooltip => Language.GetTextValue("Mods.POCalValAddon.PetTooltips.StasisDrone");
        }
    }
}
