using System.Collections.Generic;
using UnityEngine;


public class PlatformManager : MonoBehaviour
{
	[SerializeField] private Transform _platformContainer;

	[SerializeField] private float _spawnX;
	[SerializeField] private float _destroyX;

	[SerializeField] private float _topY;
	[SerializeField] private float _bottomY;

	[SerializeField] private Vector2 _platformSize = new Vector2(1.7f, 0.45f);

	[SerializeField] private float _levelSpeed;

	[SerializeField] private List<GameObject> _platforms;
	[SerializeField] private List<float> _probabilities;


	private List<GameObject> _spawnedPlatforms;

	void Start() { }

	void Update()
	{

	}

	public void spawnRandomPlatform()
	{
		int index = Random.Range(0, _platforms.Count);
		GameObject platform = _platforms[index];

		float y = Random.Range(_bottomY, _topY);

		spawnPlatform(platform, y);
	}

	public void destroyAllPlatforms()
	{
		foreach (GameObject go in _spawnedPlatforms) destroyPlatform(go);
	}

	private void spawnPlatform(GameObject platform, float yPosition)
	{
		GameObject sPlatform = (GameObject)Instantiate(platform, new Vector3(_spawnX, yPosition, 0.0f), Quaternion.identity, _platformContainer);

		SpriteRenderer renderer = sPlatform.GetComponent<SpriteRenderer>();
		BoxCollider2D collider = sPlatform.GetComponent<BoxCollider2D>();

		renderer.size = _platformSize;
		collider.size = _platformSize;

		_spawnedPlatforms.Add(sPlatform);
	}
	private void destroyPlatform(GameObject platform)
	{
		if (Application.isEditor) Object.DestroyImmediate(platform);
		else Object.Destroy(platform);
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawLine(
			new Vector3(_spawnX, _topY),
			new Vector3(_spawnX, _bottomY)
		);

		Gizmos.color = Color.red;
		Gizmos.DrawLine(
			new Vector3(_destroyX, _topY),
			new Vector3(_destroyX, _bottomY)
		);

		Gizmos.color = Color.blue;
		Gizmos.DrawLine(
			new Vector3(_spawnX, _topY),
			new Vector3(_destroyX, _topY)
		);

		Gizmos.color = Color.blue;
		Gizmos.DrawLine(
			new Vector3(_spawnX, _bottomY),
			new Vector3(_destroyX, _bottomY)
		);
	}
}
