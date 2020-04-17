using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Jobs;
using Unity.Collections;
using Unity.Physics;

public class MoveDropletSystem : JobComponentSystem {
    protected override JobHandle OnUpdate(JobHandle inputDeps) {
        
        float deltaTime = Time.DeltaTime;

        var jobHandle = Entities.WithName("MoveDropletSystem").ForEach((ref PhysicsVelocity physics, ref Translation position, ref Rotation rotation, ref DropletData dropletData) => {
            physics.Angular = float3.zero;
            physics.Linear += deltaTime * dropletData.speed * math.forward(rotation.Value);
        }).Schedule(inputDeps);

        jobHandle.Complete();

        return jobHandle;
    }
}
