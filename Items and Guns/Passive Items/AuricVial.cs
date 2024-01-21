
using Alexandria.ItemAPI;
using UnityEngine;

namespace Items
{
    class AuricVial : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Auric Vial";

            string resourceName = "Items/Resources/ItemSprites/Passives/gilded_vial.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<AuricVial>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Good For Baiting Ghosts";
            string longDesc = "Each room gives three extra casings. \n\nMade from the pure essence of wealth, this serum increases the drinker's Fortune.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "ck");

            item.quality = ItemQuality.C;
            item.sprite.IsPerpendicular = true;

            //
        }

        public void GoldOnRoomClear(PlayerController player)
        {
            int amtToGive = 3;
            if (Owner.HasPickupID(Gungeon.Game.Items["gilded_bullets"].PickupObjectId))
            {
                amtToGive *= 2;
            }
            for(int i = 0; i < amtToGive; i++)
            {
                LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(68).gameObject, player);
            }
        }

        public override void Pickup(PlayerController player)
        {

            base.Pickup(player);
            player.OnRoomClearEvent += this.GoldOnRoomClear;
        }

        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debris = base.Drop(player);
            player.OnRoomClearEvent -= this.GoldOnRoomClear;
            debris.GetComponent<AuricVial>().m_pickedUpThisRun = true;
            return debris;
        }
       
    }
}
