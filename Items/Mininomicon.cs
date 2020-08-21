using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using ItemAPI;
using MonoMod.RuntimeDetour;
using System.Reflection;
using UnityEngine;
using Random = UnityEngine.Random;


namespace Items
{
    class Mininomicon : PassiveItem
    {
        public static void Init()
        {

            string itemName = "Mininomocon";
            string resourceName = "Items/Resources/mininomicon.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<Mininomicon>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Full of Secrets";
            string longDesc = "Adds a chance to fire an additional projectile from ANY non-beam gun with each shot.\n\nA mini version of the Ammonomicon, used as a keychain decoration by gungeoneers.";



            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cel");
            item.quality = PickupObject.ItemQuality.B;
            item.sprite.IsPerpendicular = true;
            item.AddToSubShop(ItemBuilder.ShopType.Cursula);
        }
        private Projectile ReplacementProjectile1;
        private Projectile HandlePreFireProjectileModification(Gun sourceGun, Projectile sourceProjectile)
        {
            
            if (sourceGun.PickupObjectId == 31 || sourceGun.PickupObjectId == 690)
            {
                ReplacementProjectile1 = Library.RandomProjectile();
                ReplacementProjectile1.Owner = Owner;
                return this.ReplacementProjectile1;
            }
            else
            {
                return sourceProjectile;
            }       
        }

        private void PostProcessProjectile(Projectile projectile, float effectChanceScalar)
        {
            effectChanceScalar = 1f;
            if (Owner.HasPickupID(521))
            {
                chanceActivate = .3f;
            }
            if (!Owner.CurrentGun.InfiniteAmmo)
            {
                if (Random.value < (chanceActivate * effectChanceScalar))
                {
                    Projectile projectile2 = Library.RandomProjectile();
                    GameObject gameObject = SpawnManager.SpawnProjectile(projectile2.gameObject, base.Owner.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (base.Owner.CurrentGun == null) ? 0f : base.Owner.CurrentGun.CurrentAngle), true);
                    Projectile component = gameObject.GetComponent<Projectile>();

                    if (component != null)
                    {
                        component.Owner = base.Owner;
                        component.Shooter = base.Owner.specRigidbody;
                    }
                }
            }

        }
        private void PostProcessBeamChanceTick(BeamController beamController)
        {
            if (Owner.HasPickupID(521))
            {
                chanceActivate = .3f;
            }
            if (Owner.inventory.CurrentGun.InfiniteAmmo == false)
            {
                if (UnityEngine.Random.value < this.chanceActivate)
                {
                    Gun gun;
                    int gunID;
                    do
                    {

                        gun = PickupObjectDatabase.GetRandomGun();
                        gunID = gun.PickupObjectId;
                    }
                    while (gun.HasShootStyle(ProjectileModule.ShootStyle.Beam));
                    Projectile projectile2 = ((Gun)ETGMod.Databases.Items[gunID]).DefaultModule.projectiles[0];
                    GameObject gameObject = SpawnManager.SpawnProjectile(projectile2.gameObject, base.Owner.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (base.Owner.CurrentGun == null) ? 0f : base.Owner.CurrentGun.CurrentAngle), true);
                    Projectile component = gameObject.GetComponent<Projectile>();

                    if (component != null)
                    {
                        component.Owner = base.Owner;
                        component.Shooter = base.Owner.specRigidbody;
                        component.baseData.speed = 25f;
                        component.baseData.damage = 7f;

                    }
                }
            }
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.PostProcessProjectile += this.PostProcessProjectile;
            player.PostProcessBeamChanceTick += this.PostProcessBeamChanceTick;
            player.OnPreFireProjectileModifier = (Func<Gun, Projectile, Projectile>)Delegate.Combine(player.OnPreFireProjectileModifier, new Func<Gun, Projectile, Projectile>(this.HandlePreFireProjectileModification));
        }

        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            debrisObject.GetComponent<Mininomicon>().m_pickedUpThisRun = true;
            player.PostProcessProjectile -= this.PostProcessProjectile;
            player.PostProcessBeamChanceTick -= this.PostProcessBeamChanceTick;
            player.OnPreFireProjectileModifier = (Func<Gun, Projectile, Projectile>)Delegate.Remove(player.OnPreFireProjectileModifier, new Func<Gun, Projectile, Projectile>(this.HandlePreFireProjectileModification));
            return debrisObject;
        }
        
        private float chanceActivate = .15f;
        
    }
}
