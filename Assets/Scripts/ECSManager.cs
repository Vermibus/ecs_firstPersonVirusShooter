using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Physics;

public class ECSManager : MonoBehaviour {

    public static EntityManager manager;
    public GameObject virusPrefab;
    public GameObject redBloodPrefab;
    public GameObject whiteBloodPrefab;
    public GameObject dropletPrefab;
    public GameObject playerCamera;

    int numVirus = 500;
    int numBlood = 500;
    int numDroplet = 10;

    BlobAssetStore store; 

    Entity droplet;
    public static Entity whiteBlood;

    void Start() {
        store = new BlobAssetStore();
        manager = World.DefaultGameObjectInjectionWorld.EntityManager;
        var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, store);

        Entity virus = GameObjectConversionUtility.ConvertGameObjectHierarchy(virusPrefab, settings);
        Entity redBlood = GameObjectConversionUtility.ConvertGameObjectHierarchy(redBloodPrefab, settings);
        droplet = GameObjectConversionUtility.ConvertGameObjectHierarchy(dropletPrefab, settings);
        whiteBlood = GameObjectConversionUtility.ConvertGameObjectHierarchy(whiteBloodPrefab, settings);

        for (int i = 0; i < numVirus; i++) {
            var instance = manager.Instantiate(virus);
            float3 position = new float3(
                UnityEngine.Random.Range(-50, 50),
                UnityEngine.Random.Range(-50, 50),
                UnityEngine.Random.Range(-50, 50)
            );
            manager.SetComponentData(instance, new Translation {
                Value = position
            });
            manager.SetComponentData(instance, new FloatData { 
                speed = UnityEngine.Random.Range(1, 5) / 10f,
            });
        }

        for (int i = 0; i < numBlood; i++) {
            var instance = manager.Instantiate(redBlood);
            float3 position = new float3(
                UnityEngine.Random.Range(-50, 50),
                UnityEngine.Random.Range(-50, 50),
                UnityEngine.Random.Range(-50, 50)
            );
            manager.SetComponentData(instance, new Translation {
                Value = position
            });
            manager.SetComponentData(instance, new FloatData { 
                speed = UnityEngine.Random.Range(1, 5) / 10f,
            });
        }
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            for (int i = 0; i < numDroplet; i++) {
                var instance = manager.Instantiate(droplet);
                var startPosition = playerCamera.transform.position + UnityEngine.Random.insideUnitSphere * 2;

                manager.SetComponentData(instance, new Translation { 
                    Value = startPosition
                });

                manager.SetComponentData(instance, new Rotation {
                    Value = playerCamera.transform.rotation
                });
            }
        }

        if(Input.GetKeyDown("space")) {
            Debug.Break();
        }
    }

    void OnDestroy() {
        store.Dispose();
    }
}
