using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gungeon;
using UnityEngine;
using ItemAPI;
using Dungeonator;
namespace Items
{
    class TrueGunpowder : PassiveItem
    {
        public static void Init()
        {
            string itemName = "True Gunpowder";

            string resourceName = "Items/Resources/true_gunpowder.png";

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
                GameObject hand = UnityEngine.Object.Instantiate<GameObject>(CelsItems.hellDrag.HellDragVFX);
                AkSoundEngine.PostEvent("Play_BOSS_lichB_grab_01", gameObject);
                if (!actor.healthHaver.IsBoss)
                {
                    actor.healthHaver.ApplyDamage(10000000, Vector2.zero, "GetFuckedNerd", CoreDamageTypes.Void, DamageCategory.Unstoppable, true, null, true);
                    actor.sprite.renderer.enabled = false;
                    actor.shadowDeathType = AIActor.ShadowDeathType.None;
                    tk2dBaseSprite corpsesprite = actor.CorpseObject.GetComponent<tk2dBaseSprite>();
                    corpsesprite.sprite.renderer.enabled = false;

                }
                else
                {
                    if (actor.healthHaver.IsBoss)
                    {
                        actor.healthHaver.ApplyDamage(80, Vector2.zero, "GetFuckedNerdButSlightlyLessThanNormalNerds", CoreDamageTypes.Void, DamageCategory.Unstoppable, true, null, true);
                    }
                }
                tk2dBaseSprite component1 = hand.GetComponent<tk2dBaseSprite>();
                component1.usesOverrideMaterial = true;
                component1.PlaceAtLocalPositionByAnchor(actor.specRigidbody.UnitCenter, tk2dBaseSprite.Anchor.LowerCenter);
                component1.renderer.material.shader = ShaderCache.Acquire("Brave/Effects/StencilMasked");
                cooldown -= 120;
            }
            
            
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
