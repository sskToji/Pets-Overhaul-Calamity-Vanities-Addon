using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using PetsOverhaul.Systems;
using Terraria;
using Terraria.ModLoader;

namespace POCalValAddon.Systems
{
    public abstract class CVAPetEffect : PetEffect
    {
        /// <summary>
        /// Accesses the CVAGlobalPet Class, which can contain useful methods and fields
        /// </summary>
        public CVAGlobalPet CVPet => Player.GetModPlayer<CVAGlobalPet>();
    }
}
