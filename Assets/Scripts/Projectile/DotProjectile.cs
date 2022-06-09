using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Extensions;
using Unity.Transforms;
using Unity.Collections;
using UnityEngine;



public class DotProjectile : MonoBehaviour
{
    public GameObject projectile;
    public float initialVelocity;
    public KeyCode FireKey;
    public Transform GunBarrel;

    private EntityManager entityManager;
    private BlobAssetStore blobAssetStore;
    private GameObjectConversionSettings settings;

    Entity projectileEntityPrefab;

    void Start()
    {
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        blobAssetStore = new BlobAssetStore();
        settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, blobAssetStore);
        projectileEntityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(projectile, settings);
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(FireKey))
        {
            // Remove other projectiles from the scene
            //foreach(GameObject obj in GameObject.FindGameObjectsWithTag("Projectile"))
            //{
            //    GameObject.Destroy(obj);
            //}
            Vector3 rotation = GunBarrel.rotation.eulerAngles;
            rotation.x = 0f;


            Entity bullet = entityManager.Instantiate(projectileEntityPrefab);
            entityManager.SetComponentData(bullet, new Translation { Value = GunBarrel.position });
            entityManager.SetComponentData(bullet, new Rotation { Value = Quaternion.Euler(rotation) });
            entityManager.SetComponentData(bullet, new LocalToWorld());

        }
    }
}

