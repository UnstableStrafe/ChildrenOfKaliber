using UnityEngine;
using Alexandria.ItemAPI;
using Dungeonator;
namespace Items
{
    class TrueGunpowder : PassiveItem
    {
        public static int itemID;
        public static void Init()
        {
            string itemName = "True Gunpowder";

            string resourceName = "Items/Resources/ItemSprites/Passives/true_gunpowder.png";

            GameObject obj = new GameObject(itemName);

            var item = obj.AddComponent<TrueGunpowder>();

            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);

            string shortDesc = "A Weapon That Cuts On Its Own";
            string longDesc = "Every 120 damage dealt, a random enemy in the room is killed. Bosses take heavy damage instead.\n\n\"At last, I have found it: the gunpowder of the first gun fired within the Gungeon. With it, perhaps I can finally escape. But I must be quick. They know I have it. It's only a matter of time until they catch up.\"- Torn note.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "cel");

            item.quality = ItemQuality.EXCLUDED;
            item.sprite.IsPerpendicular = true;
            item.CanBeDropped = false;
            item.CanBeSold = false;
            itemID = item.PickupObjectId;
        }
        private float cooldown = 0;
        private void GrabbyGrabTime(PlayerController player, float oof)
        {
            
            cooldown += oof;
            if(cooldown >= 120)
            {
                AIActor actor;
                RoomHandler absoluteRoom = base.transform.position.GetAbsoluteRoom();
                actor = Owner.CurrentRoom.GetRandomActiveEnemy(true);
                var hand = (EnemyDatabase.GetOrLoadByGuid("cd88c3ce60c442e9aa5b3904d31652bc")).GetComponent<LichDeathController>().HellDragVFX;
                HellDraggerArbitrary component2 = UnityEngine.Object.Instantiate<GameObject>(hand).GetComponent<HellDraggerArbitrary>();

                AkSoundEngine.PostEvent("Play_BOSS_lichB_grab_01", gameObject);
                if (!actor.healthHaver.IsBoss)
                {
                    Grab(actor, component2.HellDragVFX);
                    actor.CorpseObject = null;
                    actor.healthHaver.ApplyDamage(10000000, Vector2.zero, "GetFuckedNerd", CoreDamageTypes.Void, DamageCategory.Unstoppable, true, null, true);
                   

                }
                else
                {
                    if (actor.healthHaver.IsBoss)
                    {
                        Grab(actor, component2.HellDragVFX);
                        actor.healthHaver.ApplyDamage(80, Vector2.zero, "GetFuckedNerdButSlightlyLessThanNormalNerds", CoreDamageTypes.Void, DamageCategory.Unstoppable, true, null, true);
                        
                    }
                }

                cooldown -= 120;
            }
            
            
        }
        private void Grab(AIActor enemy, GameObject HellDragVFX)
        {
            GameObject gameObject = enemy.PlayEffectOnActor(HellDragVFX, new Vector3(0f, 0, 0f), true, false, false);

            tk2dBaseSprite component = gameObject.GetComponent<tk2dBaseSprite>();
            component.UpdateZDepth();
            component.attachParent = null;
            component.IsPerpendicular = false;
            component.HeightOffGround = 1f;
            component.UpdateZDepth();
            component.transform.position = component.transform.position.WithX(component.transform.position.x + 0.25f);
            component.transform.position = component.transform.position.WithY((float)enemy.ParentRoom.area.basePosition.y + 55f);
            component.usesOverrideMaterial = true;
            component.renderer.material.shader = ShaderCache.Acquire("Brave/Effects/StencilMasked");
        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.OnDealtDamage += GrabbyGrabTime;
        }
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            debrisObject.GetComponent<TrueGunpowder>().m_pickedUpThisRun = true;

            return debrisObject;
        }
    }
}
