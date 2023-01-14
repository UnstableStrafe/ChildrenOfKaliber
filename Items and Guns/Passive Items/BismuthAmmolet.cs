using System.Collections.Generic;
using Alexandria.ItemAPI;
using UnityEngine;
using Dungeonator;

namespace Items
{
    class BismuthAmmolet : SpecialBlankModificationItem
    {
        public static void Init()
        {
            string itemName = "Bismuth Ammolet";

            string resourceName = "Items/Resources/ItemSprites/Passives/bismuth_ammolet.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<BismuthAmmolet>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "Silvery Rainbow";
            string longDesc = "Blanks envenom enemies in the room. Gives one extra blank per floor.\n\nUpon extreme force being applied to the ammolet by a blank, toxic particles are released into the air. Thankfully only gundead are vulnerable to them. The Aztecs believed the pyramid-like crytals contained vast power.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cel");
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.AdditionalBlanksPerFloor, 1, StatModifier.ModifyMethod.ADDITIVE);

            item.quality = ItemQuality.B;
            item.sprite.IsPerpendicular = true;

        }
        protected override void OnBlank(SilencerInstance silencerInstance, Vector2 centerPoint, PlayerController user)
        {
            EnemyListing();
            base.OnBlank(silencerInstance, centerPoint, user);
        }
        
        private void EnemyListing()
        {
            RoomHandler absoluteRoom = base.transform.position.GetAbsoluteRoom();
            List<AIActor> activeEnemies = absoluteRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
            bool flag = activeEnemies != null;
            if (flag)
            {
                for (int i = 0; i < activeEnemies.Count; i++)
                {
                    this.AffectEnemy(activeEnemies[i]);
                }
            }
        }
        private void AffectEnemy(AIActor target)
        {
            target.ApplyEffect(Library.Venom);
            
        }
       
        public override void Pickup(PlayerController player)
        {
           
            base.Pickup(player);
        }
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            debrisObject.GetComponent<BismuthAmmolet>().m_pickedUpThisRun = true;

            return debrisObject;
        }
    }
}
