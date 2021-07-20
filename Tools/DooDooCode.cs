namespace Items
{

    public class UnchangeableRangeController : BraveBehaviour
    {
        public void Awake()
        {
            if (base.projectile != null)
            {
                this.m_origRange = base.projectile.baseData.range;
            }
        }

        public void Update()
        {
            if (base.projectile != null && base.projectile.baseData.range != this.m_origRange)
            {
                base.projectile.baseData.range = this.m_origRange;
            }
        }

        private float m_origRange;
        
    }   
}
