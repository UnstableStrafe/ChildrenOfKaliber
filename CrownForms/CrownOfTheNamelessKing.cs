using System;
using System.Collections.Generic;
using UnityEngine;
using ItemAPI;
using Dungeonator;
using System.Collections;


namespace Items
{
    class CrownOfTheNamelessKing : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Crown Of The Nameless King";

            string resourceName = "Items/Resources/test_icon.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<CrownOfTheNamelessKing>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Formless And Eternal";
            string longDesc = "A powerful crown for a king of a nameless land.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cel");
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.ProjectileSpeed, .6f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            item.quality = ItemQuality.EXCLUDED;
            item.sprite.IsPerpendicular = true;
        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            HandleRadialIndicator(7, player.sprite);
            player.PostProcessProjectile += PostProcessProjectile;
            
        }
        private IEnumerator RiskOfRain(Projectile projectile)
        {
            //ETGModConsole.Log("Burn baby burn!");
            if (projectile != null)
            {
                yield return new WaitForSeconds(0.05f);
                {
                    //AkSoundEngine.PostEvent("Play_BOSS_doormimic_flame_01", base.gameObject);
                    PlayerController player = base.Owner as PlayerController;
                    float num = 0f;
                    num = (player.stats.GetStatValue(PlayerStats.StatType.PlayerBulletScale));
                    List<AIActor> activeEnemies = player.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
                    Vector2 centerPosition = projectile.sprite.WorldCenter;
                    foreach (AIActor aiactor in activeEnemies)
                    {
                        bool flag = Vector2.Distance(aiactor.CenterPosition, centerPosition) < 2f * num && aiactor.healthHaver.GetMaxHealth() > 0f && aiactor != null && aiactor.specRigidbody != null && player != null;
                        if (flag)
                        {
                            aiactor.healthHaver.ApplyDamage(.5f, Vector2.zero, "fuckigjmnkbjnbbnjbnjnjbnjbnjbnjbjn", CoreDamageTypes.Electric, DamageCategory.Normal, false, null, false);
                        }
                    }
                }
                projectile.StartCoroutine(RiskOfRain(projectile));
                yield break;
            }
            yield break;
        }
        private void HandleRadialIndicator(float Radius, tk2dBaseSprite Psprite)
        {
            Psprite = Owner.sprite;
            if (!this.indicator)
            {
                Vector3 position = Psprite.WorldCenter.ToVector3ZisY(0f);
                indicator = ((GameObject)UnityEngine.Object.Instantiate(ResourceCache.Acquire("Global VFX/HeatIndicator"), position, Quaternion.identity, Owner.sprite.transform)).GetComponent<HeatIndicatorController>();
                indicator.CurrentRadius = 7;
                indicator.IsFire = false;
                indicator.CurrentColor = new Color(78 / 25f, 5 / 50f, 120 / 25f);
            }
        }

        private void PostProcessProjectile(Projectile sourceProjectile, float effectChanceScalar)
        {
            try
            {
                sourceProjectile.StartCoroutine(RiskOfRain(sourceProjectile));
                this.ShockRing(sourceProjectile);
                PierceProjModifier modifier = sourceProjectile.gameObject.GetOrAddComponent<PierceProjModifier>();
                modifier.penetration += 4;
            }
            catch (Exception ex)
            {
                ETGModConsole.Log(ex.Message, false);
            }
        }
        private void ShockRing(Projectile projectile)
        {
            PlayerController player = projectile.Owner as PlayerController;
            float num = 0f;
            num = (player.stats.GetStatValue(PlayerStats.StatType.PlayerBulletScale));
            this.m_radialIndicator = ((GameObject)UnityEngine.Object.Instantiate(ResourceCache.Acquire("Global VFX/HeatIndicator"), projectile.sprite.WorldCenter, Quaternion.identity, projectile.transform)).GetComponent<HeatIndicatorController>();
            this.m_radialIndicator.CurrentColor = new Color(78 / 25f, 5 / 50f, 120 / 25f);
            this.m_radialIndicator.IsFire = false;
            this.m_radialIndicator.CurrentRadius = 1.75f * num;

        }
        protected override void Update()
        {
            base.Update();
            RoomHandler r = Owner.sprite.transform.position.GetAbsoluteRoom();
            Vector3 tableCenter = Owner.sprite.WorldCenter.ToVector3ZisY(0f);
            Action<AIActor, float> AuraAction = delegate (AIActor actor, float dist)
            {
                actor.healthHaver.ApplyDamage(.7f, Vector2.zero, "bow unto his power");
            };
            r.ApplyActionToNearbyEnemies(tableCenter.XY(), 8, AuraAction);
        }
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            debrisObject.GetComponent<CrownOfTheNamelessKing>().m_pickedUpThisRun = true;
            player.PostProcessProjectile -= PostProcessProjectile;
            UnhandleRadialIndicator();
            return debrisObject;
        }
        private void UnhandleRadialIndicator()
        {
            if (indicator)
            {
                indicator.EndEffect();
                indicator = null;
            }
        }
        protected override void OnDestroy()
        {
            base.Owner.PostProcessProjectile -= this.PostProcessProjectile;
            UnhandleRadialIndicator();
            base.OnDestroy();
        }
        private HeatIndicatorController m_radialIndicator;
        private HeatIndicatorController indicator;
    }
}
