using System;
using Alexandria.ItemAPI;
using UnityEngine;
using Random = UnityEngine.Random;


namespace Items
{
    class Mininomicon : PassiveItem
    {
        public static int itemID;
        public static void Init()
        {

            string itemName = "Mininomocon";
            string resourceName = "Items/Resources/ItemSprites/Passives/mininomicon.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<Mininomicon>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Full of Secrets";
            string longDesc = "Adds a chance to fire an additional projectile from ANY non-beam gun with each shot.\n\nA mini version of the Ammonomicon, used as a keychain decoration by gungeoneers.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "ck");
            item.quality = PickupObject.ItemQuality.A;
            item.sprite.IsPerpendicular = true;
            item.AddToSubShop(ItemBuilder.ShopType.Cursula);
            itemID = item.PickupObjectId;
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
                    GameObject gameObject = SpawnManager.SpawnProjectile(projectile2.gameObject, Owner.CurrentGun.transform.position, Quaternion.Euler(0f, 0f, (base.Owner.CurrentGun == null) ? 0f : base.Owner.CurrentGun.CurrentAngle), true);
                    Projectile component = gameObject.GetComponent<Projectile>();

                    if (component != null)
                    {
                        component.Owner = base.Owner;
                        component.Shooter = base.Owner.specRigidbody;
                        if (Owner.PlayerHasActiveSynergy("Absolute Insanity"))
                        {
                            float randSize = UnityEngine.Random.Range(.3f, 3f);
                            float randSpeed = UnityEngine.Random.Range(.5f, 2f);
                            component.AdditionalScaleMultiplier = randSize;
                            component.baseData.speed = randSpeed;
                        }
                    }
                    if (Owner.PlayerHasActiveSynergy("Absolute Insanity"))
                    {


                        int Bananas = 10;
                        var Apples = Bananas;


                        float extraProjChance = UnityEngine.Random.value;
                        int extraProjAmount = UnityEngine.Random.Range(1, 4);
                        if (extraProjChance < .3f)
                        {
                            for (int i = 0; i < extraProjAmount; i++)
                            {
                                float randSize2 = UnityEngine.Random.Range(.3f, 3f);
                                float randSpeed2 = UnityEngine.Random.Range(.5f, 2f);
                                float randSpread = UnityEngine.Random.Range(-8f, 8.1f);
                                Projectile projectile3 = Library.RandomProjectile();
                                GameObject gameObject2 = SpawnManager.SpawnProjectile(projectile3.gameObject, Owner.CurrentGun.transform.position, Quaternion.Euler(0f, 0f, (base.Owner.CurrentGun == null) ? 0f : base.Owner.CurrentGun.CurrentAngle + randSpread), true);
                                
                                Projectile component2 = gameObject2.GetComponent<Projectile>();
                                if (component2 != null)
                                {
                                    component2.Owner = base.Owner;
                                    component2.Shooter = base.Owner.specRigidbody;
                                    component2.AdditionalScaleMultiplier = randSize2;
                                    component2.baseData.speed = randSpeed2;
                                }
                            }
                        }
                        
                    }
                }
            }

        }
        //private void PostProcessBeamChanceTick(BeamController beamController)
        //{
        //    if (Owner.HasPickupID(521))
        //    {
        //        chanceActivate = .3f;
        //    }
        //    if (Owner.inventory.CurrentGun.InfiniteAmmo == false)
        //    {
        //        if (UnityEngine.Random.value < this.chanceActivate)
        //        {
        //            Gun gun;
        //            int gunID;
        //            do
        //            {

        //                gun = PickupObjectDatabase.GetRandomGun();
        //                gunID = gun.PickupObjectId;
        //            }
        //            while (gun.HasShootStyle(ProjectileModule.ShootStyle.Beam));
        //            Projectile projectile2 = ((Gun)ETGMod.Databases.Items[gunID]).DefaultModule.projectiles[0];
        //            GameObject gameObject = SpawnManager.SpawnProjectile(projectile2.gameObject, base.Owner.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (base.Owner.CurrentGun == null) ? 0f : base.Owner.CurrentGun.CurrentAngle), true);
        //            Projectile component = gameObject.GetComponent<Projectile>();

        //            if (component != null)
        //            {
        //                component.Owner = base.Owner;
        //                component.Shooter = base.Owner.specRigidbody;
        //                component.baseData.speed = 25f;
        //                component.baseData.damage = 7f;

        //            }
        //        }
        //    }
        //}

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.PostProcessProjectile += this.PostProcessProjectile;
           // player.PostProcessBeamChanceTick += this.PostProcessBeamChanceTick;
            player.OnPreFireProjectileModifier = (Func<Gun, Projectile, Projectile>)Delegate.Combine(player.OnPreFireProjectileModifier, new Func<Gun, Projectile, Projectile>(this.HandlePreFireProjectileModification));
        }

        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            debrisObject.GetComponent<Mininomicon>().m_pickedUpThisRun = true;
            player.PostProcessProjectile -= this.PostProcessProjectile;
           // player.PostProcessBeamChanceTick -= this.PostProcessBeamChanceTick;
            player.OnPreFireProjectileModifier = (Func<Gun, Projectile, Projectile>)Delegate.Remove(player.OnPreFireProjectileModifier, new Func<Gun, Projectile, Projectile>(this.HandlePreFireProjectileModification));
            return debrisObject;
        }
        
        private float chanceActivate = .15f;
        
    }
}
