using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Items
{
    class RiskPassiveItem : PassiveItem
    {
        public override void Pickup(PlayerController player)
        {

            base.Pickup(player);
            if (gameObject.GetComponent<RiskParticles>() != null)
            {
                Destroy(gameObject.GetComponent<RiskParticles>());
            }
            playerC = player;
            player.gameObject.GetOrAddComponent<RiskStat>().RiskAMT += RiskToGive;
            cachedRisk = RiskToGive;
        }
        
        protected override void OnDestroy()
        {
            base.OnDestroy();
            playerC.gameObject.GetOrAddComponent<RiskStat>().RiskAMT -= cachedRisk;
        }
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject result = base.Drop(player);
            result.gameObject.AddComponent<RiskParticles>();
            playerC.gameObject.GetOrAddComponent<RiskStat>().RiskAMT -= cachedRisk;

            return result;
        }
        private float cachedRisk;
        public float RiskToGive;
        private PlayerController playerC;
    }
}
