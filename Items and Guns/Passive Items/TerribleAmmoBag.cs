using System;
using Alexandria.ItemAPI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Items
{
    class TerribleAmmoBag : PassiveItem
    {
        public static void Init()
        {

            string itemName = "Terrible Ammo Bag";
            string resourceName = "Items/Resources/ItemSprites/Passives/terrible_ammo_bag.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<TerribleAmmoBag>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "A Miracle That It Still Works";
            string longDesc = "Killing an enemy has a chance to reduce the ammo of the currently held gun, but ammo drops are increased.\n\nHeld by the 1st gungeoneer, this ammo bag has seen better days. Full of holes and tears, ammo falls out of it easily, but sometimes ammo will appear inside it.";



            ItemBuilder.SetupItem(item, shortDesc, longDesc, "ck");
            item.quality = PickupObject.ItemQuality.D;
            item.sprite.IsPerpendicular = true;
            item.AddToSubShop(ItemBuilder.ShopType.Trorc);

        }

        public void OnKilledEnemy(PlayerController player)
        {
            if (!player.CurrentGun.InfiniteAmmo)
            {
                float chanceToDrain = .25f;
                if (Owner.HasPickupID(116))
                {
                    chanceToDrain = 0f;
                }
                if (Random.value < chanceToDrain)
                {
                    float AmmoRaw = player.inventory.CurrentGun.GetBaseMaxAmmo() * .05f;
                    int AmmoTrue = Mathf.FloorToInt(AmmoRaw);


                    player.inventory.CurrentGun.ammo -= AmmoTrue;
                    if (player.inventory.CurrentGun.ammo < 0)
                    {
                        player.inventory.CurrentGun.ammo = 0;
                    }
                }
            }
            

        }

        public void RoomClear(PlayerController obj)
        {

            float chanceOnClear = .2f;
            if(Random.value < chanceOnClear)
            {
    //            ETGModConsole.Log("Spawned Ammo");

                PickupObject byId = PickupObjectDatabase.GetById(600);
                LootEngine.SpawnItem(byId.gameObject, obj.specRigidbody.UnitCenter, Vector2.up, 1f, false, true, false);
            }

         
        }



        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.OnKilledEnemy += this.OnKilledEnemy;
            player.OnRoomClearEvent += this.RoomClear;
            
        }

        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debris = base.Drop(player);
            player.OnKilledEnemy -= this.OnKilledEnemy;
            player.OnRoomClearEvent -= this.RoomClear;
            debris.GetComponent<TerribleAmmoBag>().m_pickedUpThisRun = true;
            
            return debris;
        }
        
    }
}
