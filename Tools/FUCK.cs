using UnityEngine;
using ItemAPI;

namespace Items
{
    static class FUCK
    {
        public static BasicBeamController GenerateBeamPrefab(this Projectile projectile, string spritePath)
        {
            projectile.gameObject.AddComponent<MeshRenderer>();
            projectile.gameObject.AddComponent<MeshFilter>();

            tk2dSprite sprite = projectile.GetComponent<tk2dSprite>();
            int spriteID = SpriteBuilder.AddSpriteToCollection(spritePath, ETGMod.Databases.Items.ProjectileCollection);
            tk2dTiledSprite tiledSprite = projectile.gameObject.AddComponent<tk2dTiledSprite>();
            tiledSprite.sprite.SetSprite(ETGMod.Databases.Items.ProjectileCollection, spriteID);
            UnityEngine.Object.Destroy(projectile.gameObject.GetComponent<tk2dSprite>());
            projectile.gameObject.AddComponent<tk2dSpriteAnimator>();
            BasicBeamController beamController = projectile.gameObject.AddComponent<BasicBeamController>();
            return beamController;
        }

    }
}
