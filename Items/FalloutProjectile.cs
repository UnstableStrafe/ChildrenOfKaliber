
using UnityEngine;


namespace Items
{
    internal class FalloutProjectile : MonoBehaviour
    {
        public void Start()
        {
            this.projectile = base.GetComponent<Projectile>();


            this.player = (this.projectile.Owner as PlayerController);
            Projectile proj = this.projectile;
            
            this.projectile.sprite.spriteId = this.projectile.sprite.GetSpriteIdByName("fallout_smol_projectile_001");


        }
        //public void Update()
      //  {
            
      //  }
       // public int curFrame;

      //  public int frames;

        private Projectile projectile;

        private PlayerController player;
    }
}
