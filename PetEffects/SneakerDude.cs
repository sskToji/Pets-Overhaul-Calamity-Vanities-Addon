using System;
using System.Collections.Generic;
using PetsOverhaul.Items;
using PetsOverhaul.Systems;
using PetsOverhaul.PetEffects;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using CalValEX;
using CalValEX.Items.Pets;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Items.Materials;

//Idea by @icecontrol33

namespace POCalValAddon.PetEffects
{
    public sealed class SneakerDude : PetEffect
    {
        public override int PetItemID => ModContent.ItemType<ProfanedChewToy>();
        public override PetClasses PetClassPrimary => PetClasses.Utility;

        public int sneakerChance = 500;

        public override void UpdateBadLifeRegen()
        {
            if (PetIsEquipped() && Player.HasBuff(ModContent.BuffType<BrimstoneFlames>()))
            {
                Player.lifeRegen += 6;
            }
        }

        public override void Load() 
        {
            PetsOverhaul.PetsOverhaul.OnPickupActions += PreOnPickup;
        }
        public static void PreOnPickup(Item item, Player player)
        {
            GlobalPet PickerPet = player.GetModPlayer<GlobalPet>();
            SneakerDude sneaDude = player.GetModPlayer<SneakerDude>();
            if (PickerPet.PickupChecks(item, sneaDude.PetItemID, out ItemPet itemChck) && itemChck.globalDrop && item.type == ModContent.ItemType<Bloodstone>())
            {
                for (int i = 0; i < GlobalPet.Randomizer(sneaDude.sneakerChance * item.stack, 1000); i++)
                {
                    player.QuickSpawnItem(GlobalPet.GetSource_Pet(EntitySourcePetIDs.GlobalItem), item.type, 1);
                }
            }
        }
        
        public sealed class SneakerDudePetItem : PetTooltip
        {
            public override PetEffect PetsEffect => sneaDude;
            public static SneakerDude sneaDude
            {
                get
                {
                    if (Main.LocalPlayer.TryGetModPlayer(out SneakerDude pet))
                        return pet;
                    else
                        return ModContent.GetInstance<SneakerDude>();
                }
            }
            public override string PetsTooltip => Language.GetTextValue("Mods.POCalValAddon.PetTooltips.SneakerDude");
        }
    }
}
