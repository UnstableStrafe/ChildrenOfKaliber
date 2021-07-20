using ItemAPI;
using Random = UnityEngine.Random;
using UnityEngine;

namespace Items
{
    class VenomRounds : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Venom Rounds";

            string resourceName = "Items/Resources/venom_rounds.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<VenomRounds>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "The Good Stuff";
            string longDesc = "Shots can inflict a powerful venom on enemies they hit.\n\nFew can withstand a small dosage of a Bashellisk's vemon for more than a couple seconds.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cel");

            item.quality = ItemQuality.A;
            item.sprite.IsPerpendicular = true;

        }
        
        public void Post(Projectile projectile, float eff)
        {
            projectile.OnHitEnemy = Proj;
        }
        public void Proj(Projectile bullet, SpeculativeRigidbody enemy, bool what)
        {
            float ran = .125f;
            
            if(Random.value < ran)
            {
                enemy.aiActor.ApplyEffect(Library.Venom);
            }
            
        }
        public override void Pickup(PlayerController player)
        {
            player.PostProcessProjectile += this.Post;
            base.Pickup(player);

        }
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            player.PostProcessProjectile -= this.Post;
            debrisObject.GetComponent<VenomRounds>().m_pickedUpThisRun = true;

            return debrisObject;
        }
    }
}
