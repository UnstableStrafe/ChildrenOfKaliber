using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GungeonAPI
{
    public abstract class SimpleInteractable : BraveBehaviour
    {
        public Action<PlayerController, GameObject> OnAccept, OnDecline;
        public List<string> conversation;
        public Func<PlayerController, GameObject, bool> CanUse;
        public Transform talkPoint;
        public string text, acceptText, declineText;
        public bool isToggle;
        protected bool m_isToggled, m_canUse = true;
    }
}
