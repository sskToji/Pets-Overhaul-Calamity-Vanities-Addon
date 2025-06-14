﻿using CalValEX.Items.Pets;
using PetsOverhaul.Systems;
using POCalValAddon.Systems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

//Idea by @iamnamedmuffin

namespace POCalValAddon.PetEffects
{
    public sealed class Shydolist : PetEffect
    {
        public override int PetItemID => ModContent.ItemType<SmolEldritchHoodie>();
        public override PetClasses PetClassPrimary => PetClasses.Magic;
        public override PetClasses PetClassSecondary => PetClasses.Utility;

        public float shyCost = 0.2f;
        public float shyUse = 0.2f;
        public int shyElectrifyTime = 300;

        public override void ModifyManaCost(Item item, ref float reduce, ref float mult)
        {
            if (PetIsEquipped())
            {
                mult = 1f - shyCost;
            }
        }

        public override void PostUpdateMiscEffects()
        {
            if (PetIsEquipped())
            {
                Player.GetAttackSpeed<MagicDamageClass>() += shyUse;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (PetIsEquipped())
            {
                target.AddBuff(BuffID.Electrified, shyElectrifyTime);
            }
        }

        public sealed class ShydolistPetItem : PetTooltip
        {
            public override PetEffect PetsEffect => shyDoll;
            public static Shydolist shyDoll
            {
                get
                {
                    if (Main.LocalPlayer.TryGetModPlayer(out Shydolist pet))
                        return pet;
                    else
                        return ModContent.GetInstance<Shydolist>();
                }
            }
            public override string PetsTooltip => PetUtil.LocVal("PetTooltips.Shydolist")
                .Replace("<time>", PetUtil.FloatToPercent(shyDoll.shyUse))
                .Replace("<mana>", PetUtil.FloatToPercent(shyDoll.shyCost))
                .Replace("<secs>", PetUtil.IntToTime(shyDoll.shyElectrifyTime));
            public override string SimpleTooltip => PetUtil.LocVal("SimplePetTooltips.Shydolist");
        }
    }
}
