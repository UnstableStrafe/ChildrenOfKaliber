namespace Items
{
    public class SulfurFuseEffect : GameActorEffect
    {
        public override void OnEffectApplied(GameActor actor, RuntimeGameActorEffectData effectData, float partialAmount = 1)
        {
            base.OnEffectApplied(actor, effectData, partialAmount);
            AIActor Grenade= EnemyDatabase.GetOrLoadByGuid("4d37ce3d666b4ddda8039929225b7ede");
            ExplodeOnDeath DoYouWantToExplode = actor.gameObject.AddComponent<ExplodeOnDeath>();
            ExplosionData explosionData = Grenade.GetComponent<ExplodeOnDeath>().explosionData;
            explosionData.damageToPlayer = 0;
            DoYouWantToExplode.explosionData = explosionData;
        }
    }

}
