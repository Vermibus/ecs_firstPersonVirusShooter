using Unity.Entities;
using Unity.Jobs;
using Unity.Collections;
using Unity.Physics;
using Unity.Physics.Systems;

[UpdateAfter(typeof(EndFramePhysicsSystem))]
public class DropletCollisionEventSystem : JobComponentSystem {

    BuildPhysicsWorld buildPhysicsWorld;
    StepPhysicsWorld stepPhysicsWorld;

    protected override void OnCreate() {
        buildPhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
        stepPhysicsWorld  = World.GetOrCreateSystem<StepPhysicsWorld>();
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps) {

        JobHandle jobHandle = new CollisionEventImpulseJob {
            DropletGroup = GetComponentDataFromEntity<DropletData>(),
            VirusGroup = GetComponentDataFromEntity<VirusData>()
        }.Schedule(stepPhysicsWorld.Simulation, ref buildPhysicsWorld.PhysicsWorld, inputDeps);

        jobHandle.Complete();

        return jobHandle;
    }

    struct CollisionEventImpulseJob : ICollisionEventsJob {

        [ReadOnly]
        public ComponentDataFromEntity<DropletData> DropletGroup;
        
        public ComponentDataFromEntity<VirusData> VirusGroup;
        

        public void Execute(CollisionEvent collisionEvent) {
            Entity entityA = collisionEvent.Entities.EntityA;
            Entity entityB = collisionEvent.Entities.EntityB;

            bool isTargetA = VirusGroup.Exists(entityA);
            bool isTargetB = VirusGroup.Exists(entityB);

            bool isDropletA = DropletGroup.Exists(entityA);
            bool isDropletB = DropletGroup.Exists(entityB);

            if (isDropletA && isTargetB) {
                var aliveComponent = VirusGroup[entityB];
                aliveComponent.alive = false;
                VirusGroup[entityB] = aliveComponent;
            }

            if (isDropletB && isTargetA) {
                var aliveComponent = VirusGroup[entityA];
                aliveComponent.alive = false;
                VirusGroup[entityA] = aliveComponent;
            }
        }
    }
}