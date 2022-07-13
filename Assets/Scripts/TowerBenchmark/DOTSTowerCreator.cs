using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.UI;

public class DOTSTowerCreator : MonoBehaviour
{
    public GameObject cubePrefab;
    public int length;
    public int width;
    public int height;
    public Text cubeCounterText;

    private Entity cubeEntity;
    private EntityManager entityManager;
    private BlobAssetStore blobAssetStore;
    private GameObjectConversionSettings settings;


    void Start()
    {
        // using DOTS, a bit of boilerplate to instantiate the entities from a gameobject prefab is required
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        blobAssetStore = new BlobAssetStore();
        settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, blobAssetStore);
        // now this line converts the cubePrefab to an Entity inside this script
        cubeEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(cubePrefab, settings);


        int cubeCount = 0;

        for (int h = 0; h < height; h++)
        {
            for(int w = 0; w < width; w++)
            {
                for(int l = 0; l < length; l++)
                {
                    if (l != 0 && w != 0 && l != length - 1 && w != width -1)
                    {
                        continue;
                    }

                    Entity cube = entityManager.Instantiate(cubeEntity);
                    Translation cubeTranslation = new Translation
                    {
                        Value = new Unity.Mathematics.float3(l, h + 0.5f, w)
                    };
                    entityManager.SetComponentData(cube, cubeTranslation);
                    cubeCount++;
                }
            }
        }
        cubeCounterText.text = $"Cube count: {cubeCount}";
    }

    private void OnDisable()
    {
        blobAssetStore.Dispose();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
