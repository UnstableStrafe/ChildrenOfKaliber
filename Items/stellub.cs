using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;



namespace Items
{
    class Stellub : GunVolleyModificationItem
    {
        public static void Init()
        {

            string itemName = "stelluB";
            string resourceName = "Items/Resources/stellub.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<Stellub>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "?kroW nevE sihT seoD woH";
            string longDesc = "Increases damge and firerate, but bullets fire backwards.\n\n" +
                ".elbissopmi smeed scisyhp tahw erac t'nod stellub esuaceB .yawyna stoohs ,esruoc fo ,tellub ehT .redwopnug eht fo noitingi esuac ot noitcerid gnorw eht ni si remirp stI .toohs ot elba eb dluohs stellub eseht yaw on si ereht ,scimanydorea fo swal nwonk lla ot gnidroccA";

            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Damage, 1.2f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.RateOfFire, 1.15f, StatModifier.ModifyMethod.MULTIPLICATIVE);


            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cel");
            item.quality = PickupObject.ItemQuality.EXCLUDED;




        }

        void PostProcessProjectile(Projectile projectile, float effectChanceScalar)
        {
           
            base.Owner.SpawnShadowBullet(projectile, false);
            projectile.enabled = false;
        }

        public override void Pickup(PlayerController player)
        {
            if (this.m_pickedUp)
            {
                return;
            }
            base.Pickup(player);
            player.PostProcessProjectile += this.PostProcessProjectile;
        }

        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            debrisObject.GetComponent<Stellub>().m_pickedUpThisRun = true;
            player.PostProcessProjectile -= this.PostProcessProjectile;
            return debrisObject;
        }

    }

   
}
