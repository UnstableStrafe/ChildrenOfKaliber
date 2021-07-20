using System;
using System.Collections.Generic;
using ItemAPI;
using System.Collections;
using UnityEngine;
using Gungeon;
using Dungeonator;

namespace Items
{
    class CircleOfAntimatter : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Circle Of Antimatter";

            string resourceName = "Items/Resources/test_icon.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<CircleOfAntimatter>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "NullReferenceException";
            string longDesc = "Periodically causes chain explosions in the direction the player is aiming.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cel");
            //WORK ON BASIC STAT UP SPRITES FOR SWORDTRESS YOU FUCKIGN GOOONNNNN!!! DO IT COWARD!
            item.quality = ItemQuality.EXCLUDED;
            item.sprite.IsPerpendicular = true;
        }
        private IEnumerator HahaCrownExplosionsGoBrrrrrrr(PlayerController player)
        {
            yield return new WaitForSeconds(6f);
            Vector2 vector = Owner.CenterPosition;
            Vector2 normalized = (Owner.unadjustedAimPoint.XY() - vector).normalized;
            vector += normalized * this.startDistance;
            List<Vector2> targets = this.AcquireBarrageTargets(vector, normalized);
            Owner.StartCoroutine(this.HandleBarrage(targets));


            //Vector2 boomSpot = player.sprite.WorldCenter + UnityEngine.Random.insideUnitCircle * 8;
            //Exploder.Explode(boomSpot, funnyExplosionData, Vector2.zero, null, true, CoreDamageTypes.None, false);

            StartCoroutine(HahaCrownExplosionsGoBrrrrrrr(player));
            yield break;
        }
        private IEnumerator HandleBarrage(List<Vector2> targets)
        {
            ExplosionData funnyExplosionData = new ExplosionData() { };
            funnyExplosionData = GameManager.Instance.Dungeon.sharedSettingsPrefab.DefaultExplosionData;
            funnyExplosionData.damageToPlayer = 0;
            funnyExplosionData.preventPlayerForce = true;
            funnyExplosionData.damageRadius *= 1.2f;
            funnyExplosionData.force *= .5f;
            funnyExplosionData.pushRadius = funnyExplosionData.damageRadius *= .7f;
            while (targets.Count > 0)
            {
                Vector2 currentTarget = targets[0];
                targets.RemoveAt(0);
                Exploder.Explode(currentTarget, funnyExplosionData, Vector2.zero, null, false, CoreDamageTypes.None, false);
                yield return new WaitForSeconds(this.delayBetweenStrikes / (float)this.BarrageColumns);
            }
            yield return new WaitForSeconds(.25f);
            yield break;
        }
        protected List<Vector2> AcquireBarrageTargets(Vector2 startPoint, Vector2 direction)
        {
            List<Vector2> list = new List<Vector2>();
            float num = -this.barrageRadius / 2f;
            float z = BraveMathCollege.Atan2Degrees(direction);
            Quaternion rotation = Quaternion.Euler(0f, 0f, z);
            while (num < this.attackLength)
            {
                float t = Mathf.Clamp01(num / this.attackLength);
                float num2 = Mathf.Lerp(this.initialWidth, this.finalWidth, t);
                float x = Mathf.Clamp(num, 0f, this.attackLength);
                for (int i = 0; i < this.BarrageColumns; i++)
                {
                    float num3 = Mathf.Lerp(-num2, num2, ((float)i + 1f) / ((float)this.BarrageColumns + 1f));
                    float num4 = UnityEngine.Random.Range(-num2 / (4f * (float)this.BarrageColumns), num2 / (4f * (float)this.BarrageColumns));
                    Vector2 v = new Vector2(x, num3 + num4);
                    Vector2 b = (rotation * v).XY();
                    list.Add(startPoint + b);
                }
                num += this.barrageRadius;
            }
            return list;
        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            StartCoroutine(HahaCrownExplosionsGoBrrrrrrr(player));
        }
        public override DebrisObject Drop(PlayerController player)
        {
            StopCoroutine(HahaCrownExplosionsGoBrrrrrrr(player));
            return base.Drop(player);
        }
        private float initialWidth = 3;
        private float finalWidth = 3;
        private float startDistance = 5;
        private float attackLength = 10;     
        private int BarrageColumns = 1;
        private float barrageRadius = 2f;
        private float delayBetweenStrikes = 0.2f;
    }
}
