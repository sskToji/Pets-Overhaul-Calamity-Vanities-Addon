using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using CalamityMod.Buffs.StatBuffs;
using CalamityMod.CalPlayer;
using CalValEX.Items.Pets;
using PetsOverhaul.Systems;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

//make supportive pet. players in radius get mushy buff

namespace POCalValAddon.PetEffects
{
    public sealed class SmolCrab : PetEffect
    {
        public override int PetItemID => ModContent.ItemType<ClawShroom>();
        public override PetClasses PetClassPrimary => PetClasses.Defensive;

        public int crabRegen = 5;

        public override void PostUpdateMiscEffects()
        {
            if (PetIsEquipped())
            {
                Player.lifeRegen += crabRegen;
            }
        }

        public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo)
        {
            if (PetIsEquipped())
            {
                Player.AddBuff(ModContent.BuffType<Mushy>(), 300);
            }
        }
        public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo)
        {
            if (PetIsEquipped())
            {
                Player.AddBuff(ModContent.BuffType<Mushy>(), 300);
            }
        }

        public sealed class SmolCrabPetItem : PetTooltip
        {
            public override PetEffect PetsEffect => smolCrab;
            public static SmolCrab smolCrab
            {
                get
                {
                    if (Main.LocalPlayer.TryGetModPlayer(out SmolCrab pet))
                        return pet;
                    else 
                        return ModContent.GetInstance<SmolCrab>();
                }
            }
            public override string PetsTooltip => Language.GetTextValue("Mods.POCalValAddon.PetTooltips.SmolCrab");
        }
    }
}
