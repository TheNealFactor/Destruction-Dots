using Unity.Entities;
using UnityEngine;
using Unity.Transforms;
using Unity.Rendering;
using Unity.Mathematics;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Mesh unitMesh;
    [SerializeField] private Material unitMaterial;
    [SerializeField] private GameObject gameObjectPrefab;

    [SerializeField] int xSize = 10;
    [SerializeField] int ySize = 10;
    [Range(0.1f, 2f)]
    [SerializeField] float spacing = 1f;

    private Entity entityPrefab;
    private World defaultWorld;
    private EntityManager entityManager;

    public Text cubeCounterText;
    private int cubeCount;

    private void Start()
    {

        defaultWorld = World.DefaultGameObjectInjectionWorld;
        entityManager = defaultWorld.EntityManager;

        GameObjectConversionSettings settings = GameObjectConversionSettings.FromWorld(defaultWorld, null);

        entityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(gameObjectPrefab, settings);

        // InstantiateEntity(new float3(4f, 0f, 4f));
        InstantiateEntityGrid(xSize, ySize, spacing);
    }

    private void InstantiateEntity(float3 position)
    {
        Entity myEntity = entityManager.Instantiate(entityPrefab);
        entityManager.SetComponentData(myEntity, new Translation
        {
            Value = position
        });
    }

    private void InstantiateEntityGrid(int dimX, int dimY, float spacing)
    {
        for (int i = 0; i < dimX; i++)
        {
            for (int j = 0; j < dimY; j++)
            {
                InstantiateEntity(new float3(i * spacing, j * spacing, 0f));
                cubeCount++;
            }
        }
        cubeCounterText.text = $"Cube count: {cubeCount}";
    }

    private void MakeEntity()
    {
        EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        EntityArchetype archetype = entityManager.CreateArchetype(
            typeof(Translation),
            typeof(Rotation),
            typeof(RenderMesh),
            typeof(RenderBounds),
            typeof(LocalToWorld)
            );

        Entity myEntity = entityManager.CreateEntity(archetype);

        entityManager.AddComponentData(myEntity, new Translation
        {
            Value = new float3(0f, 0f, 0f)
        });

        RenderMeshUtility.AddComponents(myEntity, entityManager, new RenderMeshDescription(unitMesh, unitMaterial));
        entityManager.AddSharedComponentData(myEntity, new RenderMesh
        {
            mesh = unitMesh,
            material = unitMaterial
        });


    }
}

//https://docs.unity.cn/Packages/com.unity.entities@0.1/manual/shared_component_data.html
