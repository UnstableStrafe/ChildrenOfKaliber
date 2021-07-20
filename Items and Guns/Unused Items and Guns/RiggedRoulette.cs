using ItemAPI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Items
{
    class RiggedRoulette : PlayerItem
    {
        public RiggedRoulette()
        {
            this.BadCh = 25f;
            this.GoodCh = 25f; 
        }
        public static void Init()
        {
            

            string itemName = "Rigged Roulette";

            string resourceName = "Items/Resources/test_icon.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<RiggedRoulette>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "The Whims of Fate";
            string longDesc = "On use takes 3 casings and gives either a good, bad, or nuetral outcome. Depending on the result, increases chance of getting inverse reward type. \n\nAn old roulette, faded and worn by years of use.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cel");

            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Timed, 1f);


            item.consumable = false;
            item.quality = ItemQuality.B;





        }
        protected override void DoEffect(PlayerController user)
        {
            user.carriedConsumables.Currency -= 3;
            float ChRoll = Random.Range(0f, 100f);
            if(ChRoll <= BadCh)
            {
                GoodCh += 10f;
                BadCh -= 10f;
                ETGModConsole.Log("Good = " + GoodCh.ToString());
                ETGModConsole.Log("Bad = " + BadCh.ToString());
            }
            if(ChRoll >= GoodCh)
            {
                BadCh += 10f;
                GoodCh -= 10f;                
                ETGModConsole.Log("Good = " + GoodCh.ToString());
                ETGModConsole.Log("Bad = " + BadCh.ToString());
            }
            if(ChRoll > BadCh && ChRoll < GoodCh)

            
            base.DoEffect(user);
        }

        public override bool CanBeUsed(PlayerController user)
        {
            return user.carriedConsumables.Currency > 0;
        }

        private float GoodCh;
        private float BadCh;
        private float NCh;
    }
}
