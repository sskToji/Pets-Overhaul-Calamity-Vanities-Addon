using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using CalamityMod.BiomeManagers;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Items.Weapons.DraedonsArsenal;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.World;
using CalValEX.Items.Pets;
using PetsOverhaul.Systems;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using static System.Net.WebRequestMethods;

namespace POCalValAddon.PetEffects
{
    public sealed class GeorgeMecha : PetEffect
    {
        public override int PetItemID => ModContent.ItemType<FROM>();
        public override PetClasses PetClassPrimary => PetClasses.Offensive;
        public override PetClasses PetClassSecondary => PetClasses.Utility;

        public float mechaPreHmDmg = 0.1f;
        public float mechaHmDmg = 0.15f;
        public float mechaPostMlDmg = 0.2f;
        public int mechaAggro = 400;
        public static List<int> MechaPreHmWeapons =
        [
            ModContent.ItemType<PulsePistol>(),
            ModContent.ItemType<StarSwallowerContainmentUnit>(),
            ModContent.ItemType<Taser>(),
            ModContent.ItemType<TrackingDisk>(),
            ModContent.ItemType<GaussDagger>(),
        ];
        public static List<int> MechaHmWeapons =
        [
            ModContent.ItemType<FrequencyManipulator>(),
            ModContent.ItemType<GatlingLaser>(),
            ModContent.ItemType<GaussPistol>(),
            ModContent.ItemType<GaussRifle>(),
            ModContent.ItemType<HydraulicVoltCrasher>(),
            ModContent.ItemType<MatterModulator>(),
            ModContent.ItemType<PulseTurretRemote>(),
        ];
        public static List<int> MechaPostMlWeapons =
        [
            ModContent.ItemType<Karasawa>(),
            ModContent.ItemType<Phaseslayer>(),
            ModContent.ItemType<PlasmaGrenade>(),
            ModContent.ItemType<PoleWarper>(),
            ModContent.ItemType<PulseRifle>(),
            ModContent.ItemType<TeslaCannon>(),
            ModContent.ItemType<TheAnomalysNanogun>(),
            ModContent.ItemType<FreedomStar>(),
            ModContent.ItemType<HeavyLaserRifle>(),
            ModContent.ItemType<PlasmaCaster>(),
            ModContent.ItemType<PulseDragon>(),
            ModContent.ItemType<SnakeEyes>(),
            ModContent.ItemType<WavePounder>(),
        ];

        public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
        {
            if (PetIsEquipped() && MechaPreHmWeapons.Contains(item.type))
            {
                damage += mechaPreHmDmg;
            }
            if (PetIsEquipped() && MechaHmWeapons.Contains(item.type))
            {
                damage += mechaHmDmg;
            }
            if (PetIsEquipped() && MechaPostMlWeapons.Contains(item.type))
            {
                damage += mechaPostMlDmg;
            }
        }
        public override void UpdateBadLifeRegen()
        {
            if (PetIsEquipped() && Player.HasBuff<CrushDepth>())
            {
                Player.lifeRegen += 5;
            }
        }
        public override void PostUpdateMiscEffects()
        {
            if (PetIsEquipped() && (Player.InModBiome<SulphurousSeaBiome>() || Player.InModBiome<AbyssLayer1Biome>() || Player.InModBiome<AbyssLayer2Biome>() || Player.InModBiome<AbyssLayer3Biome>()))
            {
                Player.aggro -= mechaAggro;
            }
        }

        public sealed class GeorgeMechaPetItem : PetTooltip
        {
            public override PetEffect PetsEffect => mechaGeorge;
            public static GeorgeMecha mechaGeorge
            {
                get
                {
                    if (Main.LocalPlayer.TryGetModPlayer(out GeorgeMecha pet))
                        return pet;
                    else
                        return ModContent.GetInstance<GeorgeMecha>();
                }
            }
            public override string PetsTooltip => Language.GetTextValue("Mods.POCalValAddon.PetTooltips.GeorgeMecha");
        }
    }
}
