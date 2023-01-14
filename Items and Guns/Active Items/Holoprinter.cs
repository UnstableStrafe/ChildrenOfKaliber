using System.Collections;
using Alexandria.ItemAPI;
using UnityEngine;

namespace Items
{
    class Holoprinter : PlayerItem
    {
        public static void Init()
        {
            string itemName = "Holoprinter";

            string resourceName = "Items/Resources/ItemSprites/Actives/holoprinter.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<Holoprinter>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Property of Hyperion";
            string longDesc = "On the first use, destroys the currently held gun, storing it's ID. Afterwards, using the item will spawn a copy of the gun for a short time.\n\nThe Retro Emitter-Transmitter Operator, R.E.T.O for short, was invented" +
               "by an extremely handsome individual. This prototype is extremely unstable and is prone to destroying the original copy it replicates.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cel");
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Damage, 600);

            item.quality = ItemQuality.A;
            item.sprite.IsPerpendicular = true;


        }
        public override void DoEffect(PlayerController user)
        {
            base.DoEffect(user);
            if(StoredGunId == -3)
            {
                user.StartCoroutine(Scan(user));
            }
            else
            {
                Deploy(user);
                StartCoroutine(ItemBuilder.HandleDuration(this, 14, user, Remove));
            }
            
        }
        public IEnumerator Scan(PlayerController user)
        {
            user.inventory.GunLocked.SetOverride("Holoprinter Scan", true);
            StoredGunId = user.CurrentGun.PickupObjectId;
            this.CanBeSold = false;
            this.CanBeDropped = false;
            float elapsed = 0;
            while(elapsed < 1)
            {
                elapsed += Time.deltaTime;
                yield return null;
            }
            user.inventory.DestroyCurrentGun();
            user.inventory.GunLocked.SetOverride("Holoprinter Scan", false);
            this.CanBeDropped = true;
            this.CanBeSold = true;
            yield break;
        }
        public void Deploy(PlayerController user)
        {
            Gun StoredGun = PickupObjectDatabase.GetById(StoredGunId) as Gun;
            HologramGun = user.inventory.AddGunToInventory(StoredGun, true);
            HologramGun.CanBeDropped = false;
            HologramGun.CanBeSold = false;
            this.CanBeSold = false;
            this.CanBeDropped = false;
            HologramGun.sprite.usesOverrideMaterial = true;
            Material material = HologramGun.sprite.renderer.material;
            material.shader = ShaderCache.Acquire("Brave/Internal/HologramShader");
            material.SetFloat("_IsGreen", 0f);
            user.inventory.GunLocked.SetOverride("Holoprinter Deploy", true);
        }
        private void Remove(PlayerController user)
        {
            user.inventory.DestroyCurrentGun();
            user.inventory.GunLocked.SetOverride("Holoprinter Deploy", false);
            this.CanBeDropped = true;
            this.CanBeSold = true;
        }
        private Gun HologramGun;
        public override void Pickup(PlayerController player)
        {
           base.Pickup(player);
        }
        public override bool CanBeUsed(PlayerController user)
        {
            return !user.CurrentGun.InfiniteAmmo;
        }

        [SerializeField]
        private int StoredGunId = -3;

        public override void OnPreDrop(PlayerController user)
        {
            user.inventory.GunLocked.SetOverride("Holoprinter Deploy", false);
            user.inventory.DestroyGun(HologramGun);
            
            this.CanBeSold = true;
            this.CanBeDropped = true;
            base.OnPreDrop(user);
        }

    }
}
