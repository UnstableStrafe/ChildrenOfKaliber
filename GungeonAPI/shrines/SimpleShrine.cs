using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GungeonAPI
{
    public class SimpleShrine : SimpleInteractable, IPlayerInteractable
    {
        void Start()
        {
            talkPoint = transform.Find("talkpoint");
            m_isToggled = false;
        }

        public void Interact(PlayerController interactor)
        {
            if (TextBoxManager.HasTextBox(this.talkPoint))
                return;

            Tools.Print("Can use: " + (CanUse == null));
            m_canUse = CanUse != null ? CanUse.Invoke(interactor, this.gameObject) : m_canUse;
            StartCoroutine(this.HandleConversation(interactor));
        }

        private IEnumerator HandleConversation(PlayerController interactor)
        {
            // Show text and lock player in place
            TextBoxManager.ShowStoneTablet(this.talkPoint.position, this.talkPoint, -1f, text, true, false);
            int selectedResponse = -1;
            interactor.SetInputOverride("shrineConversation");
            yield return null;

            // Wait for player response
            if (!m_canUse)
            {
                GameUIRoot.Instance.DisplayPlayerConversationOptions(interactor, null, declineText, string.Empty);
            }
            else if (isToggle)
            {
                if (m_isToggled)
                    GameUIRoot.Instance.DisplayPlayerConversationOptions(interactor, null, declineText, string.Empty);
                else
                    GameUIRoot.Instance.DisplayPlayerConversationOptions(interactor, null, acceptText, string.Empty);
            }
            else
            {
                GameUIRoot.Instance.DisplayPlayerConversationOptions(interactor, null, acceptText, declineText);
            }


            while (!GameUIRoot.Instance.GetPlayerConversationResponse(out selectedResponse))
            {
                yield return null;
            }

            // Free player and run OnAccept/OnDecline actions
            interactor.ClearInputOverride("shrineConversation");
            TextBoxManager.ClearTextBox(this.talkPoint);
            if (!m_canUse)
                yield break;

            if (selectedResponse == 0 && isToggle)
            {
                (m_isToggled ? OnDecline : OnAccept)?.Invoke(interactor, this.gameObject);
                m_isToggled = !m_isToggled;
                yield break;
            }

            if (selectedResponse == 0)
                OnAccept?.Invoke(interactor, this.gameObject);
            else
                OnDecline?.Invoke(interactor, this.gameObject);
            yield break;
        }

        public void OnEnteredRange(PlayerController interactor)
        {
            SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.white, 1f);
            base.sprite.UpdateZDepth();
        }

        public void OnExitRange(PlayerController interactor)
        {
            SpriteOutlineManager.RemoveOutlineFromSprite(base.sprite, false);
        }

        public string GetAnimationState(PlayerController interactor, out bool shouldBeFlipped)
        {
            shouldBeFlipped = false;
            return string.Empty;
        }

        public float GetDistanceToPoint(Vector2 point)
        {
            if (base.sprite == null)
            {
                return 100f;
            }
            Vector3 v = BraveMathCollege.ClosestPointOnRectangle(point, base.specRigidbody.UnitBottomLeft, base.specRigidbody.UnitDimensions);
            return Vector2.Distance(point, v) / 1.5f;
        }

        public float GetOverrideMaxDistance()
        {
            return -1f;
        }
    }
}
