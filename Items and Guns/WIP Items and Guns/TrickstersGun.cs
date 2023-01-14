using System.Collections.Generic;
using Dungeonator;
using UnityEngine;
using System.Collections;
using Alexandria.ItemAPI;

namespace Items
{
    class TrickstersGun : PlayerItem
    {
        public static void Init()
        {
            string itemName = "Trickster's Gun";

            string resourceName = "Items/Resources/tricksters_gun.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<TrickstersGun>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "A Memento";
            string longDesc = "Use on a boss with less than 1/3 health remaining for a devastating finale!\n\nA gun once wielded by a powerful Trickster who sought defiance against those who abused power. A potent energy thrums from within it." +
                "\n\nIt's time for an all out attack!";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cel");
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Curse, 2, StatModifier.ModifyMethod.ADDITIVE);
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Damage, 1000);
            ItemBuilder.AddToSubShop(item, ItemBuilder.ShopType.Cursula);

            item.quality = ItemQuality.EXCLUDED;
            item.sprite.IsPerpendicular = true;


        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
        }
        public override void DoEffect(PlayerController user)
        {
            EnemyListing(user);
        }
        private void EnemyListing(PlayerController user)
        {
            RoomHandler absoluteRoom = base.transform.position.GetAbsoluteRoom();
            List<AIActor> activeEnemies = absoluteRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
            bool flag = activeEnemies != null;
            if (flag)
            {
                for (int i = 0; i < activeEnemies.Count; i++)
                {
                    this.AffectEnemy(activeEnemies[i], user);
                }
            }
        }
        protected void AffectEnemy(AIActor target, PlayerController user)
        {
            bool flag = target.healthHaver.GetCurrentHealthPercentage() < .3f;
            if (flag)
            {
                target.StartCoroutine(HandleClockhair(target, user));
            }
        }
        public IEnumerator HandleClockhair(AIActor targetEnemy, PlayerController user)
        {
            RadialSlowInterface Rad;
            Rad = new RadialSlowInterface
            {
                RadialSlowHoldTime = 6f,
                RadialSlowOutTime = 2f,
                RadialSlowTimeModifier = 0f,
                DoesSepia = false,
                UpdatesForNewEnemies = true,
                audioEvent = "Play_OBJ_time_bell_01",
            };
            Rad.DoRadialSlow(user.CenterPosition, user.CurrentRoom);
            user.healthHaver.IsVulnerable = false;
            user.SetInputOverride("tiddy");
            Pixelator.Instance.DoFinalNonFadedLayer = false;
            Transform clockhairTransform = ((GameObject)Instantiate(BraveResources.Load("Clockhair", ".prefab"))).transform;
            ClockhairController clockhair = clockhairTransform.GetComponent<ClockhairController>();
           // clockhair.gameObject.SetLayerRecursively(LayerMask.NameToLayer("Unpixelated"));
            float elapsed = 0f;
            float duration = clockhair.ClockhairInDuration;
            Vector3 clockhairTargetPosition = targetEnemy.CenterPosition;
            Vector3 clockhairStartPosition = clockhairTargetPosition + new Vector3(-20f, 5f, 0f);
            clockhair.renderer.enabled = true;
            clockhair.spriteAnimator.alwaysUpdateOffscreen = true;
            clockhair.spriteAnimator.Play("clockhair_intro");
            clockhair.hourAnimator.Play("hour_hand_intro");
            clockhair.minuteAnimator.Play("minute_hand_intro");
            clockhair.secondAnimator.Play("second_hand_intro");
           
            while (elapsed < duration)
            {
                if (GameManager.INVARIANT_DELTA_TIME == 0f)
                {
                    elapsed += 0.05f;
                }
                elapsed += GameManager.INVARIANT_DELTA_TIME;
                float t = elapsed / duration;
                float smoothT = Mathf.SmoothStep(0f, 1f, t);
                clockhairTargetPosition = targetEnemy.CenterPosition;
                Vector3 currentPosition = Vector3.Slerp(clockhairStartPosition, clockhairTargetPosition, smoothT);
                clockhairTransform.position = currentPosition.WithZ(0f);
                if (t > 0.5f)
                {
                    clockhair.renderer.enabled = true;
                }
                if (t > 0.75f)
                {
                    clockhair.hourAnimator.GetComponent<Renderer>().enabled = true;
                    clockhair.minuteAnimator.GetComponent<Renderer>().enabled = true;
                    clockhair.secondAnimator.GetComponent<Renderer>().enabled = true;
                }
                clockhair.sprite.UpdateZDepth();
                yield return null;
            }
            clockhair.SetMotionType(1f);
            float dur = 2;
            elapsed = 0;
            clockhair.BeginSpinningWildly();
            while (elapsed < dur)
            {
                if (GameManager.INVARIANT_DELTA_TIME == 0f)
                {
                    elapsed += 0.05f;
                }
                elapsed += GameManager.INVARIANT_DELTA_TIME;
                clockhairTransform.position = targetEnemy.CenterPosition;
                yield return null;
            }
            AkSoundEngine.PostEvent("Play_ENV_time_shatter_01", gameObject);
            targetEnemy.healthHaver.ApplyDamage(10000, Vector2.zero, "die", CoreDamageTypes.None, DamageCategory.Normal, false, null, true);
            yield return new WaitForSeconds(.2f);
            Destroy(clockhairTransform.gameObject);
            user.ClearInputOverride("tiddy");
            user.healthHaver.IsVulnerable = true;
            yield break;
        }

        public DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            debrisObject.GetComponent<TrickstersGun>().m_pickedUpThisRun = true;
            return debrisObject;
        }
        
    }
}
