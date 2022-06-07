using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.UI;


public class DotProjectile : MonoBehaviour
{
    public GameObject projectilePrefab;
    public KeyCode FireKey;
    public KeyCode DestroyKey;

    private Entity projectileEntity;
    private EntityManager entityManager;
    private BlobAssetStore blobAssetStore;
    private GameObjectConversionSettings settings;
    private List<Entity> projectiles;
    

    void Start()
    {
        // using DOTS, a bit of boilerplate to instantiate the entities from a gameobject prefab is required
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        blobAssetStore = new BlobAssetStore();
        settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, blobAssetStore);
        // now this line converts the cubePrefab to an Entity inside this script
        projectileEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(projectilePrefab, settings);

        projectiles = new List<Entity>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(FireKey))
        {
            //Remove other projectiles from the scene

            Entity projectile = entityManager.Instantiate(projectileEntity);

            Translation projectileTranslation = new Translation
            {
                Value = new Unity.Mathematics.float3(this.transform.position)
            };
            entityManager.SetComponentData(projectile, projectileTranslation);

            projectiles.Add(projectile);

            //Old
            //var projectileInstance = GameObject.Instantiate(projectile, this.transform.position, Quaternion.identity);
            //projectileInstance.GetComponent<Rigidbody>().velocity = initialVelocity * this.transform.forward;
        }

        if (Input.GetKeyDown(DestroyKey))
        {
            foreach (Entity ent in projectiles)
            {
                entityManager.DestroyEntity(ent);

            }
            projectiles.Clear();
        }
    }

    private void OnDisable()
    {
        blobAssetStore.Dispose();
    }

    private void OnDestroy()
    {
        //blobAssetStore.Dispose();
    }
}
