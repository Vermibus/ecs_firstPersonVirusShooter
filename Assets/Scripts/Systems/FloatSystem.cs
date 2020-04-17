using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Jobs;
using Unity.Collections;
using Unity.Physics;

public class FloatSystem : JobComponentSystem {
    protected override JobHandle OnUpdate(JobHandle inputDeps) {
        
        float deltaTime = Time.DeltaTime;
        var jobHandle = Entities.WithName("FloatSystem").ForEach((ref PhysicsVelocity physics, ref Translation position, ref Rotation rotation, ref FloatData floatData) => {
            float sin = math.sin((deltaTime + position.Value.x) * 0.5f) * floatData.speed;
            float cos = math.cos((deltaTime + position.Value.y) * 0.5f) * floatData.speed;

            float3 direction = new float3(sin, cos, sin);
            physics.Linear += direction;
        }).Schedule(inputDeps);

        jobHandle.Complete();

        return jobHandle;
    }
}
