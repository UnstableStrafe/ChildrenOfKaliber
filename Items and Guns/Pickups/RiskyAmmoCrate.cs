using Dungeonator;
using ItemAPI;
using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Items
{
    class RiskyAmmoCrate : PickupObject, IPlayerInteractable
    {
        public static void Register()
        {
            string name = "Risky Ammo Crate";
            string resourcePath = "Items/Resources/test_icon.png";
            GameObject gameObject = new GameObject(name);
            RiskyAmmoCrate item = gameObject.AddComponent<RiskyAmmoCrate>();
            ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
            string shortDesc = "Current + Quick Switch Gun Refilled; All Guns Partially Refilled; Risk Up";
            string longDesc = "Refills the primary and secondary gun, and all other guns by 33%, but increases Risk by 1.";

            item.gameObject.AddComponent<RiskParticles>();
            item.SetupItem(shortDesc, longDesc, "cel");

            item.quality = ItemQuality.EXCLUDED;
            item.IgnoredByRat = true;

        }


        public override void Pickup(PlayerController player)
        {

            if (m_hasBeenPickedUp)
                return;

            m_hasBeenPickedUp = true;
            player.gameObject.GetOrAddComponent<RiskStat>().RiskAMT++;
            foreach (Gun gun in player.inventory.AllGuns)
            {
                if (gun.CanGainAmmo && gun != null)
                {
                    if (gun != player.CurrentGun)
                    {

                        if (GameManager.Options.QuickSelectEnabled && player.IsQuickEquipGun(gun))
                        {
                            gun.GainAmmo(gun.AdjustedMaxAmmo);
                        }
                        else
                        {
                            gun.GainAmmo(Mathf.FloorToInt((float)gun.AdjustedMaxAmmo * .33f));
                        }
                    }
                    else
                    {
                        gun.GainAmmo(gun.AdjustedMaxAmmo);
                        gun.ForceImmediateReload(false);
                    }
                }
                
            }
           


            AkSoundEngine.PostEvent("Play_OBJ_ammo_pickup_01", base.gameObject);
            player.RemovePassiveItem(PickupObjectDatabase.GetByName("Risky Ammo Crate").PickupObjectId);

            player.BloopItemAboveHead(base.sprite, "Items/Resources/test_icon.png");

            UnityEngine.Object.Destroy(base.gameObject);
        }



        protected void Start()
        {
            try
            {

                GameManager.Instance.PrimaryPlayer.CurrentRoom.RegisterInteractable(this);


                SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.black, 0.1f, 0f, SpriteOutlineManager.OutlineType.NORMAL);
            }
            catch (Exception er)
            {
                ETGModConsole.Log("Risky ammo bugged! Yell at me or Ski!" + er.ToString(), true);
            }
        }

        public float GetDistanceToPoint(Vector2 point)
        {
            if (!base.sprite)
            {
                return 1000f;
            }
            Bounds bounds = base.sprite.GetBounds();
            bounds.SetMinMax(bounds.min + base.transform.position, bounds.max + base.transform.position);
            float num = Mathf.Max(Mathf.Min(point.x, bounds.max.x), bounds.min.x);
            float num2 = Mathf.Max(Mathf.Min(point.y, bounds.max.y), bounds.min.y);
            return Mathf.Sqrt((point.x - num) * (point.x - num) + (point.y - num2) * (point.y - num2)) / 1.5f;
        }

        public float GetOverrideMaxDistance()
        {
            return 1f;
        }

        public void OnEnteredRange(PlayerController interactor)
        {
            if (!this)
            {
                return;
            }
            if (!interactor.CurrentRoom.IsRegistered(this) && !RoomHandler.unassignedInteractableObjects.Contains(this))
            {
                return;
            }
            SpriteOutlineManager.RemoveOutlineFromSprite(base.sprite, false);
            SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.white, 0.1f);
            base.sprite.UpdateZDepth();
        }

        public void OnExitRange(PlayerController interactor)
        {
            if (!this)
            {
                return;
            }
            SpriteOutlineManager.RemoveOutlineFromSprite(base.sprite, true);
            SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.black, 0.1f);
            base.sprite.UpdateZDepth();
        }



        public void Interact(PlayerController interactor)
        {
            try
            {

                if (!this)
                {
                    return;
                }
                if (interactor.CurrentItem)
                {
                    if (RoomHandler.unassignedInteractableObjects.Contains(this))
                    {
                        RoomHandler.unassignedInteractableObjects.Remove(this);
                    }
                    SpriteOutlineManager.RemoveOutlineFromSprite(base.sprite, true);

                    this.Pickup(interactor);
                }
                else
                {

                }
            }
            catch (Exception err)
            {
                ETGModConsole.Log("Risky ammo bugged! Yell at me or Ski!" + err.ToString(), true);
            }
        }

        public string GetAnimationState(PlayerController interactor, out bool shouldBeFlipped)
        {
            shouldBeFlipped = false;

            return string.Empty;
        }

        private bool m_hasBeenPickedUp;
    }
    
}
