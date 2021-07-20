using System;
using ItemAPI;
using UnityEngine;

namespace Items
{
    class MarkOfWind : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Mark Of The Wind";

            string resourceName = "Items/Resources/glyph.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<MarkOfWind>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Too Fast For Eyes";
            string longDesc = "Upon entering combat, time is briefly slowed down.\n\nThose given this marking gain enhanced adrenal glands, allowing them to react to sudden danger with incredible speed.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cel");

            item.quality = ItemQuality.A;
            item.sprite.IsPerpendicular = true;


        }
        

        private void Slow()
        {
            Rad = new RadialSlowInterface
            {
                RadialSlowHoldTime = 5f,
                RadialSlowOutTime = 2f,
                RadialSlowTimeModifier = 0.4f,
                DoesSepia = false,
                UpdatesForNewEnemies = true,
                audioEvent = "Play_OBJ_time_bell_01",
            };
            PlayerController player = this.Owner;
            
            Rad.DoRadialSlow(Owner.CenterPosition, Owner.CurrentRoom);
            
        }
        
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.OnEnteredCombat = (Action)Delegate.Combine(player.OnEnteredCombat, new Action(this.Slow));
        }
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            debrisObject.GetComponent<MarkOfWind>().m_pickedUpThisRun = true;
            player.OnEnteredCombat = (Action)Delegate.Remove(player.OnEnteredCombat, new Action(this.Slow));
            return debrisObject;
        }
        private RadialSlowInterface Rad;
    }
}
