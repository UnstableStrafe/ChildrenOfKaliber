using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gungeon;
using Alexandria.ItemAPI;
using UnityEngine;
using Dungeonator;
using System.Collections;
using System.Reflection;
using System.Collections.ObjectModel;

namespace Items
{
    class PsiFocus : PlayerItem
    {
        public static void Init()
        {
            string itemName = "Psi Focus";

            string resourceName = "Items/Resources/ItemSprites/Actives/psi_focus.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<PsiFocus>();
            obj.AddComponent<PreventOnActiveEffects>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Bet Ges Aln";
            string longDesc = "On use, converts 5 nearby enemy projectiles into friendly projectiles and sends them towards the nearest enemy. The amount of projectiles that can be converted scales based on ammo capacity and clip size. When in great stress, the capacity is doubled.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "ck");

            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Timed, 7f);

            item.consumable = false;
            item.quality = ItemQuality.EXCLUDED;
            item.sprite.IsPerpendicular = true;
          

        }
        public override void DoEffect(PlayerController user)
        {
            base.DoEffect(user);
            bool flag = user.CurrentRoom != null;
            if (flag)
            {
                ReadOnlyCollection<Projectile> allProjectiles = StaticReferenceManager.AllProjectiles;
                if(allProjectiles != null)
                {
                    if (indicator == null)
                    {

                        this.indicator = ((GameObject)UnityEngine.Object.Instantiate(ResourceCache.Acquire("Global VFX/HeatIndicator"), user.CenterPosition.ToVector3ZisY(), Quaternion.identity, user.sprite.transform)).GetComponent<HeatIndicatorController>();
                        this.indicator.CurrentRadius = range / 8;
                        this.indicator.transform.parent = user.sprite.transform;
                        indicator.IsFire = false;
                        indicator.CurrentColor = new Color(254, 126, 229, 30);
                    }
                    GameManager.Instance.StartCoroutine(FindNearbyEnemyProjectiles(user));
                    //StartCoroutine(ItemBuilder.HandleDurationWithoutEndEffect(this, .3f, user));
                }
                
            }
           
            
        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
        }

        private IEnumerator FindNearbyEnemyProjectiles(PlayerController player)
        {
            
            ReadOnlyCollection<Projectile> allProjectiles = StaticReferenceManager.AllProjectiles;
            List<Projectile> projectiles = new List<Projectile> { };
            int c = Mathf.CeilToInt((4 + Mathf.CeilToInt(1 * player.stats.GetStatModifier(PlayerStats.StatType.AdditionalClipCapacityMultiplier))) * player.stats.GetStatModifier(PlayerStats.StatType.AmmoCapacityMultiplier));
            if(player.healthHaver.GetCurrentHealth() == .5f)
            {
                c *= 2;
            }
            int i = 0;
            foreach(Projectile proj in allProjectiles)
            {
                if (proj)
                {
                    if (!(proj.Owner is PlayerController))
                    {
                        Vector3 v3 = player.sprite.WorldCenter;
                        Vector2 vector = (proj.transform.position - v3).XY();
                        if (proj.CanBeKilledByExplosions && vector.sqrMagnitude < range)
                        {
                            projectiles.Add(proj);
                            i++;
                            if(i == c)
                            {
                                break;
                            }
                        }
                    }
                }

            }
            if (projectiles.Any())
            {
                GameManager.Instance.StartCoroutine(RetargetEnemyProjectiles(player, projectiles));
            }
            yield return new WaitForSeconds(.3f);
            if (indicator != null)
            {
                Destroy(indicator.gameObject);
                indicator = null;
            }
            yield break;
        }
        private IEnumerator RetargetEnemyProjectiles(PlayerController player, List<Projectile> projectiles)
        {
            
            foreach (Projectile proj in projectiles)
            {
                if(proj != null)
                {
                    proj.RemoveBulletScriptControl();
                    if (proj.Owner && proj.Owner.specRigidbody)
                    {
                        proj.specRigidbody.DeregisterSpecificCollisionException(proj.Owner.specRigidbody);
                    }
                    proj.Speed = 0;
                    proj.Owner = player;
                    proj.SetNewShooter(player.specRigidbody);
                    proj.ResetDistance();
                    if(proj.baseData.damage != ProjectileData.FixedFallbackDamageToEnemies)
                    {
                        proj.baseData.damage = ProjectileData.FixedFallbackDamageToEnemies;
                    }
                    proj.baseData.damage = 7 * player.stats.GetStatModifier(PlayerStats.StatType.Damage);
                    proj.collidesWithPlayer = false;
                    proj.collidesWithEnemies = true;
                    proj.UpdateCollisionMask();
                    Color color = new Color32(254, 126, 229, 255);
                    proj.AdjustPlayerProjectileTint(color, 10000);
                    player.DoPostProcessProjectile(proj);
                }
            }
            yield return new WaitForSeconds(.35f);
            foreach(Projectile proj in projectiles)
            {
                proj.Speed = 25;
                Vector2 vector = UnityEngine.Random.insideUnitCircle;
                Func<AIActor, bool> isValid = (AIActor a) => a && a.HasBeenEngaged && a.healthHaver && a.healthHaver.IsVulnerable;
                AIActor closestToPosition = BraveUtility.GetClosestToPosition<AIActor>(player.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All), player.sprite.WorldCenter, isValid);
                if (closestToPosition)
                {
                    vector = closestToPosition.CenterPosition - proj.transform.position.XY();
                }
                proj.SendInDirection(vector, true);
            }
            yield break;
        }


        private HeatIndicatorController indicator = null;
        private float range = 30;

    }
    

}
