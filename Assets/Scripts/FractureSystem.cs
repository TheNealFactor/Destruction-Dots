using Unity.Entities;
using UnityEngine;
using Unity.Transforms;
using Unity.Rendering;
using Unity.Mathematics;

public class FractureSystem : MonoBehaviour
{
    [SerializeField] private Mesh unitMesh;
    [SerializeField] private Material unitMaterial;

    private void Start()
    {
        MakeEntity();
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

        //PrintMeshInfo();
    }

    [ContextMenu("Print Mesh Info")]
    public void PrintMeshInfo()
    {
        var mesh = this.GetComponent<MeshFilter>().mesh;
        Debug.Log("Positions");

        

        var positions = mesh.vertices;
        var normals = mesh.normals;
        var uvs = mesh.uv;

        for (int i = 0; i < positions.Length; i++)
        {
            Debug.Log($"Vertex {i}");
            Debug.Log($"POS | X: {positions[i].x} Y: {positions[i].y} Z: {positions[i].z}");
            Debug.Log($"NRM | X: {normals[i].x} Y: {normals[i].y} Z: {normals[i].z} LEN: {normals[i].magnitude}");
            Debug.Log($"UV  | U: {uvs[i].x} V: {uvs[i].y}");
            Debug.Log("");
        }
    }
}

//https://docs.unity.cn/Packages/com.unity.entities@0.1/manual/shared_component_data.html