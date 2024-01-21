using UnityEngine;
using Alexandria.ItemAPI;


namespace Items
{
    public class NeedleBullets: PassiveItem
    {
        public static int itemID;
        public static void Init()
        {
            
            string itemName = "Needle Bullets";
            string resourceName = "Items/Resources/ItemSprites/Passives/needle_bullets.png";
          
            GameObject obj = new GameObject(itemName);
          
            var item = obj.AddComponent<NeedleBullets>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
           
            string shortDesc = "You'll Shoot Your Eye Out!";
            string longDesc = "Massively increases shot speed. Bullets pierce all enemies.\n\n" +
                "These old, limited edition Gungeon-themed thumb tacks can be found in large quantities throughout the Gungeon's depths. They have become a favorite among Gundead, despite their lack of thumbs. " +
                "Nobody knows why or how these bullets gained their arcane powers. Perhaps the Gungeon favors its own likeness...";
         
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "ck");
           
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.ProjectileSpeed, 2.5f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Curse, 1);
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Damage, 1.10f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.AdditionalShotPiercing, 100);
      

            item.quality = PickupObject.ItemQuality.A;
            item.sprite.IsPerpendicular = true;
            item.AddToSubShop(ItemBuilder.ShopType.Cursula);
            item.AddToSubShop(ItemBuilder.ShopType.Trorc);
            itemID = item.PickupObjectId;
        }
    }
}
