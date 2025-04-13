using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalamityMod.Items.Weapons.Summon;
using CalValEX.Items.Pets;
using PetsOverhaul.Systems;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static System.Net.Mime.MediaTypeNames;

namespace POCalValAddon.PetEffects
{
    public sealed class EggBaby : PetEffect
    {
        public override int PetItemID => ModContent.ItemType<AstraEGGeldon>();
        public override PetClasses PetClassPrimary => PetClasses.Summoner;

        public int gelConsume = 0;
        public float summonDmg = 1.2f;
        public static List<int> SlimeWeapons =
        [
            ModContent.ItemType<AbandonedSlimeStaff>(),
            ModContent.ItemType<Perdition>(),
            ModContent.ItemType<Vigilance>(),
            ModContent.ItemType<EntropysVigil>()
        ];

        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (PetIsEquipped() && proj.GetGlobalProjectile<SlimeProjectile>().eggProj == true)
            {
                if (gelConsume > 0)
                {
                    modifiers.FinalDamage *= summonDmg;
                    gelConsume--;
                }
                else
                {
                    if (Player.ConsumeItem(ItemID.Gel))
                    {
                        gelConsume = 30;
                    }
                }
            }
        }
        public sealed class SlimeProjectile : GlobalProjectile
        {
            public override bool InstancePerEntity => true;
            public bool eggProj = false;
            public override void OnSpawn(Projectile projectile, IEntitySource source)
            {
                if (source is EntitySource_ItemUse item && item.Item is not null && EggBaby.SlimeWeapons.Contains(item.Item.type))
                {
                    eggProj = true;
                }
                if (source is EntitySource_Parent parent && parent.Entity is Projectile proj && proj.GetGlobalProjectile<SlimeProjectile>().eggProj == true)
                {
                    eggProj = true;
                }
            }
        }

        public sealed class EggBabyPetItem : PetTooltip
        {
            public override PetEffect PetsEffect => eggBaby;
            public static EggBaby eggBaby
            {
                get
                {
                    if (Main.LocalPlayer.TryGetModPlayer(out EggBaby pet))
                        return pet;
                    else
                        return ModContent.GetInstance<EggBaby>();
                }
            }
            public override string PetsTooltip => Language.GetTextValue("Mods.POCalValAddon.PetTooltips.EggBaby");
        }
    }
}
