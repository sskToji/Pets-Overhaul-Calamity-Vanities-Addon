using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalValEX.Items.Pets;
using PetsOverhaul.Systems;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using static POCalValAddon.PetEffects.EggBaby;
using Terraria.DataStructures;
using System.Reflection.Metadata.Ecma335;
using Terraria.Localization;

namespace POCalValAddon.PetEffects
{
    public sealed class JellyCrystal : PetEffect
    {
        public override int PetItemID => ModContent.ItemType<IonizedJellyCrystal>();
        public override PetClasses PetClassPrimary => PetClasses.Summoner;

        public int gelConsume = 0;
        public float summonDmg = 1.2f;

        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (PetIsEquipped() && proj.GetGlobalProjectile<JellyProjectile>().jellyProj == true && proj.CountsAsClass<SummonDamageClass>())
            {
                if (gelConsume > 0)
                {
                    modifiers.FinalDamage *= summonDmg;
                    gelConsume--;
                }
                else if (gelConsume == 0)
                {
                    Player.ConsumeItem(ItemID.Gel);
                    gelConsume = 30;
                }
            }
        }
        public sealed class JellyProjectile : GlobalProjectile
        {
            public override bool InstancePerEntity => true;
            public bool jellyProj = false;
            public override void OnSpawn(Projectile projectile, IEntitySource source)
            {
                if (source is EntitySource_ItemUse item && item.Item is not null)
                {
                    jellyProj = true;
                }
                if (source is EntitySource_Parent parent && parent.Entity is Projectile proj && proj.GetGlobalProjectile<JellyProjectile>().jellyProj == true)
                {
                    jellyProj = true;
                }
            }
        }

        public sealed class JellyCrystalPetItem : PetTooltip
        {
            public override PetEffect PetsEffect => jellyCrystal;
            public static JellyCrystal jellyCrystal
            {
                get
                {
                    if (Main.LocalPlayer.TryGetModPlayer(out JellyCrystal pet))
                        return pet;
                    else
                        return ModContent.GetInstance<JellyCrystal>();
                }
            }
            public override string PetsTooltip => Language.GetTextValue("Mods.POCalValAddon.PetTooltips.JellyCrystal");
        }
    }
}
