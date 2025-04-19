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

namespace POCalValAddon.PetEffects
{
    public sealed class StasisDrone : PetEffect
    {
        public override int PetItemID => ModContent.ItemType<ArmoredScrap>();
        public override PetClasses PetClassPrimary => PetClasses.Utility;

        public float droneMovespeed = 0.2f;
        public float droneAccelspeed = 0.2f;
        public float droneWingTimeStore = 0.5f;
        private float droneWingTimeBank = 0;

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
                if (droneWingTimeBank >= 1 && Player.wingTime < Player.wingTimeMax)
                {
                    Player.wingTime++;
                    droneWingTimeBank--;
                }
            }
        }
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (Player.wingTime > 0 && PetIsEquipped() && triggersSet.Jump && Player.dead == false && Player.equippedWings is not null && Player.ZoneRain is not false)
            {
                float total = Math.Abs(Player.velocity.Y) + Math.Abs(Player.velocity.X);
                float xRemain = Math.Abs(Player.velocity.X) / total;
                if (xRemain is float.NaN)
                {
                    xRemain = 0;
                }
                droneWingTimeBank += Math.Abs(xRemain * droneWingTimeStore);
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
