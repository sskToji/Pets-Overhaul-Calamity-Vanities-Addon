using System;
using Microsoft.Xna.Framework;
using Steamworks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using PetsOverhaul;
using PetsOverhaul.Items;
using PetsOverhaul.Systems;
using CalValEX.Items.Pets;
using System.Security.Cryptography.X509Certificates;
using Terraria.DataStructures;

namespace POCalValAddon.PetEffects
{
    public sealed class DriedPest : PetEffect
    {
        public override int PetItemID => ModContent.ItemType<DriedLocket>();
        public override PetClasses PetClassPrimary => PetClasses.Defensive;
        public override PetClasses PetClassSecondary => PetClasses.Mobility;

        public float desertMovement = 0.20f;
    }
}
