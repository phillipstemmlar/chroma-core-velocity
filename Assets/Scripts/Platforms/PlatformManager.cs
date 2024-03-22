using System.Collections.Generic;
using UnityEngine;


public class PlatformManager : MonoBehaviour
{
	[Header("Platform Spawning")]
	[SerializeField] private Transform _platformParent;
	[SerializeField] private Vector2 _platformSize = new Vector2(1.7f, 0.45f);

	[SerializeField] private float _spawnX;
	[SerializeField] private float _destroyX;

	[SerializeField] private float _topY;
	[SerializeField] private float _bottomY;

	[SerializeField] private List<GameObject> _platforms;
	[SerializeField] private List<float> _probabilities;
	private List<GameObject> _spawnedPlatforms = new List<GameObject>();

	[Header("Platform Movement")]
	[SerializeField] private float _levelSpeed;
	public float levelSpeed => _levelSpeed;

	private Vector2 _trackingSize;
	private float _elapsedTime = 0;

	private Vector2 _startingPlatformSize => new Vector2(_spawnX - _destroyX, _platformSize.y);
	private float startingPlatformOffset = 1.0f;

	void Start()
	{
		spawnStartingPlatform();
		_trackingSize = _startingPlatformSize;
		_elapsedTime = (_trackingSize.x - startingPlatformOffset - _platformSize.x * 0.5f) / _levelSpeed;
	}

	void FixedUpdate()
	{
	}

	void Update()
	{
		foreach (GameObject go in _spawnedPlatforms)
		{
			if (go != null)
			{
				PlatformMovement movement = go.GetComponent<PlatformMovement>();
				movement.speed = -1 * _levelSpeed;
			}
		}

		_elapsedTime += Time.deltaTime;
		float waitTime = _trackingSize.x / _levelSpeed;
		if (_elapsedTime >= waitTime)
		{
			_trackingSize = _platformSize;
			spawnRandomPlatform();
			_elapsedTime = 0;
		}
	}

	public void spawnStartingPlatform()
	{
		int index = Random.Range(0, _platforms.Count);
		GameObject platform = _platforms[index];
		spawnPlatform(platform, new Vector2(startingPlatformOffset, _bottomY), _startingPlatformSize);
	}

	public void spawnRandomPlatform()
	{
		int index = Random.Range(0, _platforms.Count);
		GameObject platform = _platforms[index];
		spawnPlatform(platform, new Vector2(_spawnX, _bottomY), _trackingSize);
	}

	public void destroyAllPlatforms()
	{
		foreach (GameObject go in _spawnedPlatforms) destroyPlatform(go);
	}

	private void spawnPlatform(GameObject platform, Vector2 position, Vector2 size)
	{
		GameObject sPlatform = (GameObject)Instantiate(platform, position, Quaternion.identity, _platformParent);

		SpriteRenderer renderer = sPlatform.GetComponent<SpriteRenderer>();
		BoxCollider2D collider = sPlatform.GetComponent<BoxCollider2D>();
		PlatformMovement movement = sPlatform.GetComponent<PlatformMovement>();

		renderer.size = size;
		collider.size = size;

		movement.speed = -1 * _levelSpeed;
		movement.destroyX = _destroyX;

		_spawnedPlatforms.Add(sPlatform);
	}
	private void destroyPlatform(GameObject platform)
	{
		bool success = _spawnedPlatforms.Remove(platform);
		if (success) Debug.Log($"Count: ${_spawnedPlatforms.Count}");
		if (Application.isEditor) DestroyImmediate(platform);
		else Destroy(platform);
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
