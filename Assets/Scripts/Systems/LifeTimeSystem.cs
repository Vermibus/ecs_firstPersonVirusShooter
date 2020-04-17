using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Jobs;
using Unity.Collections;
using Unity.Physics;

public class LifeTimeSystem : JobComponentSystem {
    protected override JobHandle OnUpdate(JobHandle inputDeps) {
        
        float deltaTime = Time.DeltaTime;

        Entities.WithoutBurst().WithStructuralChanges().ForEach((Entity entity, ref Translation position, ref LifeTimeData lifeTimeData) => {
            lifeTimeData.lifeLeft -= deltaTime;

            if (lifeTimeData.lifeLeft <= 0) {
                EntityManager.DestroyEntity(entity);
            }

        }).Run();

        Entities.WithoutBurst().WithStructuralChanges().ForEach((Entity entity, ref Translation position, ref VirusData virusData) => {

            if (!virusData.alive) {

                for (int i = 0; i < 100; i++) {
                    float3 offset = (float3) UnityEngine.Random.insideUnitSphere * 2.0f;
                    var whiteBloodEntity = ECSManager.manager.Instantiate(ECSManager.whiteBlood);
                    float3 randomDirection = new float3(
                        UnityEngine.Random.Range(-1, 1),
                        UnityEngine.Random.Range(-1, 1),
                        UnityEngine.Random.Range(-1, 1)
                    );

                    ECSManager.manager.SetComponentData(whiteBloodEntity, new Translation {
                        Value = position.Value + offset
                    });

                    ECSManager.manager.SetComponentData(whiteBloodEntity, new PhysicsVelocity {
                        Linear = randomDirection * 2
                    });
                }

                EntityManager.DestroyEntity(entity);
            }

        }).Run();

        return inputDeps;
    }
}
