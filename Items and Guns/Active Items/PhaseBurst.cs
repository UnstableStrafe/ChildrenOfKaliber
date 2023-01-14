using UnityEngine;
using Alexandria.ItemAPI;
using Gungeon;
using MonoMod;
using System;
using System.Collections;
using System.Collections.Generic;
using MonoMod.RuntimeDetour;
namespace Items
{
    class PhaseBurst : PlayerItem
    {
        public static void Init()
        {
            string itemName = "Phase Burst";

            string resourceName = "Items/Resources/ItemSprites/Actives/phase_burst.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<PhaseBurst>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Cha Cha Real Smooth";
            string longDesc = "Allows the user to temporarily slip between timelines, but at the cost of being unable to attack.\n\nThe Pursued is known for jumping in and out of timestreams in order to escape the Hegemony Police Force. Doing so is a delicate balance, but the risk is worth being able to evade capture.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cel");
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Damage, 400);

            item.quality = ItemQuality.C;
            item.sprite.IsPerpendicular = true;
        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);

        }
        public override void Update()
        {
            PlayerController player = this.LastOwner;
            bool a = player;
            bool flag2 = a;
            bool flag3 = flag2;
            if (flag3)
            {
                bool flag = this.SoShitDoesntNull == 0;
                if (flag)
                {
                    zoomy = player.gameObject.AddComponent<ImprovedAfterImage>();
                    zoomy.dashColor = new Color(33, 71, 237);
                    zoomy.spawnShadows = false;
                    zoomy.shadowTimeDelay = 0.05f;
                    zoomy.shadowTimeDelay = 0.05f;
                    zoomy.shadowLifetime = 0.3f;
                    zoomy.minTranslation = 0.05f;
                    zoomy.OverrideImageShader = ShaderCache.Acquire("Brave/Internal/DownwellAfterImage");
                    this.SoShitDoesntNull += 1f;
                }
            }
            base.Update();
        }
        private float SoShitDoesntNull = 0;

        
        public override void DoEffect(PlayerController user)
        {
            base.DoEffect(user);
            StartEffect(user);
            StartCoroutine(ItemBuilder.HandleDuration(this, 6, user, EndEffect));
        }
        private void StartEffect(PlayerController player)
        {
            CanBeDropped = false;
            CanBeSold = false;
            if (!zoomy.spawnShadows)
            {
                zoomy.spawnShadows = true;
            }
            if (!player.IsGunLocked)
            {
                player.IsGunLocked = true;
            }
            Rad = new RadialSlowInterface
            {
                RadialSlowHoldTime = 3f,
                RadialSlowOutTime = .6f,
                RadialSlowTimeModifier = 0.4f,
                DoesSepia = false,
                UpdatesForNewEnemies = true,
                audioEvent = "Play_OBJ_time_bell_01",
            };
            Rad.DoRadialSlow(player.CenterPosition, player.CurrentRoom);
            player.stats.RecalculateStats(player, false, false);
            player.sprite.usesOverrideMaterial = true;
            Material material = player.sprite.renderer.material;
            material.shader = ShaderCache.Acquire("Brave/Internal/HologramShader");
            material.SetFloat("_IsGreen", 0f);
        }
        private void EndEffect(PlayerController player)
        {
            CanBeDropped = true;
            CanBeSold = true;
            if (zoomy.spawnShadows)
            {
                zoomy.spawnShadows = false;
            }
            if (player.IsGunLocked)
            {
                player.IsGunLocked = false;
            }
            player.sprite.usesOverrideMaterial = false;
            player.ClearOverrideShader();
        }
        public static List<PlayerController> cursedPlayers = new List<PlayerController>();
        private ImprovedAfterImage zoomy;
        private RadialSlowInterface Rad;
    }
}
