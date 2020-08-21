using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ItemAPI;
using Gungeon;
using UnityEngine;

namespace Items
{
    class Holoprinter : PlayerItem
    {
         public static void Init()
         {
             string itemName = "Holoprinter";

             string resourceName = "Items/Resources/test_icon.png";

             GameObject obj = new GameObject(itemName);

             var item = obj.AddComponent<Holoprinter>();

             ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

             string shortDesc = "Property of Hyperion";
             string longDesc = "On the first use, destroys the currently held gun, storing it's ID. Afterwards, using the item will spawn a copy of the gun for a short time.\n\nThe Retro Emitter-Transmitter Operator, R.E.T.O for short, was invented" +
                "by an extremely handsome individual. This prototype is extremely unstable and is prone to destroying the original copy it replicates.";

             ItemBuilder.SetupItem(item, shortDesc, longDesc, "cel");
             ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Damage, 600);

             item.quality = ItemQuality.EXCLUDED;
             item.sprite.IsPerpendicular = true;


         }
        protected override void DoEffect(PlayerController user)
        {
            base.DoEffect(user);
            if(HasScanned == false)
            {
                CloneGun(user);
                StartCoroutine(ItemBuilder.HandleDuration(this, 12, user, this.DecloneGun));
            }
            if(HasScanned == true)
            {
                BeginScan(user);
                StartCoroutine(ItemBuilder.HandleDuration(this, 1, user, this.StoreID));
            }

        }
        private bool HasScanned = false;
        //Scanning
        private void BeginScan(PlayerController user)
        {
            user.inventory.GunLocked.SetOverride("Scanning Gun!", true, null);
            ScannedGun = user.CurrentGun;
            this.CanBeDropped = false;
            this.CanBeSold = false;
        }
        private void StoreID(PlayerController user)
        {
            user.inventory.DestroyCurrentGun();
            user.inventory.GunLocked.RemoveOverride("Scanning Gun!");
            this.ClearCooldowns();
            this.CanBeSold = true;
            this.CanBeDropped = true;
            HasScanned = true;
        }
        //Cloning
        private void CloneGun(PlayerController user)
        {         
            user.inventory.AddGunToInventory(ScannedGun, true);
            user.inventory.GunLocked.SetOverride("Hologun!", true, null);
            ScannedGun.sprite.usesOverrideMaterial = true;
            Material material = ScannedGun.sprite.renderer.material;
            material.shader = ShaderCache.Acquire("Brave/Internal/HologramShader");
            material.SetFloat("_IsGreen", 0f);
            ScannedGun.CanBeDropped = false;
            ScannedGun.CanBeSold = false;
            this.CanBeDropped = false;
            this.CanBeSold = false;
        }
        private void DecloneGun(PlayerController user)
        {
            user.inventory.DestroyGun(ScannedGun);
            user.inventory.GunLocked.RemoveOverride("Hologun!");
            this.CanBeSold = true;
            this.CanBeDropped = true;
        }

        private Gun ScannedGun = null;
        public override void Pickup(PlayerController player)
        {
           base.Pickup(player);

        }

        public DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            debrisObject.GetComponent<Holoprinter>().m_pickedUpThisRun = true;

            return debrisObject;
        }
         
    }
}
