using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalValEX.Items.Pets;
using PetsOverhaul.Systems;
using Terraria.ModLoader;

namespace POCalValAddon.PetEffects
{
    public sealed class BuffMan : PetEffect
    {
        public override int PetItemID => ModContent.ItemType<ReaperoidPills>();
        public override PetClasses PetClassPrimary => PetClasses.Melee;

    }
}
