using Alexandria.ItemAPI;
using UnityEngine;

class IronHeart : PlayerItem
{
    public static void Init()
    {
        string itemName = "Iron Heart";

        string resourceName = "Items/Resources/ItemSprites/Actives/iron_heart.png";

        GameObject obj = new GameObject();

        var item = obj.AddComponent<IronHeart>();

        ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

        string shortDesc = "Proof You Have a Heart";
        string longDesc = "Removes all but one heart container and gives double the amount in armor, plus 2 extra.\n\n" +
            "This heart-shaped pin converts all iron, carbon, and other 'necessary' trace metals that " +
            "scientists claim are 'vital for human survival' and 'required for bodily functions' into an ultimate shield!";


        ItemBuilder.SetupItem(item, shortDesc, longDesc, "cel");

        ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Timed, 1.5f);



        item.consumable = true;
        item.quality = ItemQuality.B;
        item.sprite.IsPerpendicular = true;
    }
    
    public override void DoEffect(PlayerController user)
    {
        float curHealth = user.healthHaver.GetMaxHealth();
        float trueHP = curHealth - 1;       
            AkSoundEngine.PostEvent("Play_OBJ_dead_again_01", base.gameObject);
            user.healthHaver.Armor += curHealth*2;
            if (user.HasPickupID(314))
            {
                user.healthHaver.Armor += 3;
            }
            StatModifier statModifierH = new StatModifier()
            {
                statToBoost = PlayerStats.StatType.Health,
                amount = -trueHP,
                modifyType = StatModifier.ModifyMethod.ADDITIVE,

            };
            user.ownerlessStatModifiers.Add(statModifierH);
            user.stats.RecalculateStats(user, false, false);
        
    }
    
    public override void Pickup(PlayerController player)
    {
        if (this.m_pickedUp)
        {
            return;
        }
        base.Pickup(player);

    }
    public DebrisObject Drop(PlayerController player)
    {
        DebrisObject debrisObject = base.Drop(player);

        debrisObject.GetComponent<IronHeart>().m_pickedUpThisRun = true;
       
        return debrisObject;
    }

    
}