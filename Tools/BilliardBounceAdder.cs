using UnityEngine;
namespace Items
{
    class BilliardBounceAdder : MonoBehaviour
    {
        public BilliardBounceAdder()
        {

        }
        private void Awake()
        {
            this.projectile = base.GetComponent<Projectile>();
        }
        private void Update()
        {
            bool flag = this.projectile == null;
            if (flag)
            {
                this.projectile = base.GetComponent<Projectile>();

            }
            this.elapsed += BraveTime.DeltaTime;
            bool flag3 = this.elapsed > .5f;
            if (flag3 && this.on == false)
            {
                this.projectile.collidesOnlyWithPlayerProjectiles = true;
                this.projectile.collidesWithProjectiles = true;
                this.projectile.UpdateCollisionMask();
                this.on = true;
                this.elapsed = 0;
            }
        }
        private bool on = false;
        private float elapsed;
        private Projectile projectile;
    }
}
