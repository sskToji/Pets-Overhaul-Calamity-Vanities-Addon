using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Items.Weapons.Summon;
using CalValEX.Items.Pets.ExoMechs;
using PetsOverhaul.Systems;
using POCalValAddon.Systems;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace POCalValAddon.PetEffects
{
    public sealed class ExoAres : PetEffect
    {
        public override int PetItemID => ModContent.ItemType<OminousCore>();
        public override PetClasses PetClassPrimary => PetClasses.Offensive;

        public float aresMeleeDmg = 0.2f;
        public float aresRangedHurtReduction = 0.1f;
        public float aresSummonDmg = 0.25f;
        public int aresNoUse = 0;

        public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
        {
            if (PetIsEquipped() && (item.type == ModContent.ItemType<PhotonRipper>()))
            {
                damage += aresMeleeDmg;
            }
            if (PetIsEquipped() && (item.type == ModContent.ItemType<AresExoskeleton>()))
            {
                damage += aresSummonDmg;
            }
            if (PetIsEquipped() && !CalValItemSets.AresWeapons[item.type]) //For nullifying damage from other weapons
            {
                damage *= aresNoUse;
            }
        }
        public override void ModifyHurt(ref Player.HurtModifiers modifiers)
        {
            if (PetIsEquipped() && Player.HasItem(ModContent.ItemType<TheJailor>()))
            {
                modifiers.FinalDamage *= 1f - aresRangedHurtReduction;
            }
        }

        public override void ModifyHitNPCWithItem(Item item, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (PetIsEquipped() && !CalValItemSets.AresWeapons[item.type])
            {
                target.takenDamageMultiplier = aresNoUse;
                if (target.lifeRegen < 0)
                {
                    target.lifeRegen = 0;
                }
            }
        }
        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (PetIsEquipped() && !(CalValItemSets.AresWeapons[proj.type] || proj.GetGlobalProjectile<AresProjectile>().aresProj == true))
            {
                target.takenDamageMultiplier = aresNoUse;
                if (target.lifeRegen < 0)
                {
                    target.lifeRegen = 0;
                }
            }
        }
        public sealed class AresProjectile : GlobalProjectile
        {
            public override bool InstancePerEntity => true;
            public bool aresProj = false;
            public override void OnSpawn(Projectile projectile, IEntitySource source)
            {
                if (source is EntitySource_ItemUse item && item.Item is not null && CalValItemSets.AresWeapons[item.Item.type])
                {
                    aresProj = true;
                }
                if (source is EntitySource_Parent parent && parent.Entity is Projectile proj && proj.GetGlobalProjectile<AresProjectile>().aresProj == true)
                {
                    aresProj = true;
                }
            }
        }
        public sealed class ExoAresPetItem : PetTooltip
        {
            public override PetEffect PetsEffect => aresBaby;
            public static ExoAres aresBaby
            {
                get
                {
                    if (Main.LocalPlayer.TryGetModPlayer(out ExoAres pet))
                        return pet;
                    else
                        return ModContent.GetInstance<ExoAres>();
                }
            }
            public override string PetsTooltip => PetUtil.LocVal("PetTooltips.ExoMechs.ExoAres")
                .Replace("<prDmg>", PetUtil.FloatToPercent(aresBaby.aresMeleeDmg))
                .Replace("<aeDmg>", PetUtil.FloatToPercent(aresBaby.aresSummonDmg))
                .Replace("<reduce>", PetUtil.FloatToPercent(aresBaby.aresRangedHurtReduction));
            public override string SimpleTooltip => PetUtil.LocVal("SimplePetTooltips.ExoMechs.ExoAres");
        }
    }
}
