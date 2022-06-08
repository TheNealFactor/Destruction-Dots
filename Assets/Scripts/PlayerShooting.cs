using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
	public bool useECS = false;
	public bool spreadShot = false;


	public Transform gunBarrel;
	public ParticleSystem shotVFX;
	public AudioSource shotAudio;
	public float fireRate = .1f;
	public int spreadAmount = 20;


	public GameObject bulletPrefab;
	private EntityManager entityManager;
	private BlobAssetStore blobAssetStore;
	private GameObjectConversionSettings settings;
	float timer;


	Entity bulletEntityPrefab;


	void Start()
	{
		if (useECS)
		{
			entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
			blobAssetStore = new BlobAssetStore();
			settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, blobAssetStore);
			bulletEntityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(bulletPrefab, settings);
		}
	}

	void Update()
	{
		timer += Time.deltaTime;

		if (Input.GetButton("Fire1") && timer >= fireRate)
		{
			Vector3 rotation = gunBarrel.rotation.eulerAngles;
			rotation.x = 0f;

			if (useECS)
			{
				if (spreadShot)
					SpawnBulletSpreadECS(rotation);
				else
					SpawnBulletECS(rotation);
			}
			else
			{
				if (spreadShot)
					SpawnBulletSpread(rotation);
				else
					SpawnBullet(rotation);
			}

			timer = 0f;

			if (shotVFX)
				shotVFX.Play();

			if (shotAudio)
				shotAudio.Play();
		}
	}

	void SpawnBullet(Vector3 rotation)
	{
		GameObject bullet = Instantiate(bulletPrefab) as GameObject;

		bullet.transform.position = gunBarrel.position;
		bullet.transform.rotation = Quaternion.Euler(rotation);
	}

	void SpawnBulletSpread(Vector3 rotation)
	{
		int max = spreadAmount / 2;
		int min = -max;

		Vector3 tempRot = rotation;
		for (int x = min; x < max; x++)
		{
			tempRot.x = (rotation.x + 3 * x) % 360;

			for (int y = min; y < max; y++)
			{
				tempRot.y = (rotation.y + 3 * y) % 360;

				GameObject bullet = Instantiate(bulletPrefab) as GameObject;

				bullet.transform.position = gunBarrel.position;
				bullet.transform.rotation = Quaternion.Euler(tempRot);
			}
		}
	}

	void SpawnBulletECS(Vector3 rotation)
	{
		Entity bullet = entityManager.Instantiate(bulletEntityPrefab);

		entityManager.SetComponentData(bullet, new Translation { Value = gunBarrel.position });
		entityManager.SetComponentData(bullet, new Rotation { Value = Quaternion.Euler(rotation) });
	}

	void SpawnBulletSpreadECS(Vector3 rotation)
	{
		int max = spreadAmount / 2;
		int min = -max;
		int totalAmount = spreadAmount * spreadAmount;

		Vector3 tempRot = rotation;
		int index = 0;

		NativeArray<Entity> bullets = new NativeArray<Entity>(totalAmount, Allocator.TempJob);
		entityManager.Instantiate(bulletEntityPrefab, bullets);

		for (int x = min; x < max; x++)
		{
			tempRot.x = (rotation.x + 3 * x) % 360;

			for (int y = min; y < max; y++)
			{
				tempRot.y = (rotation.y + 3 * y) % 360;

				entityManager.SetComponentData(bullets[index], new Translation { Value = gunBarrel.position });
				entityManager.SetComponentData(bullets[index], new Rotation { Value = Quaternion.Euler(tempRot) });

				index++;
			}
		}
		bullets.Dispose();
	}
}