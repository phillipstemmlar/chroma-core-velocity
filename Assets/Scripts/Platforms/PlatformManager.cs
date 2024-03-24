using Baracuda.Monitoring;
using System.Collections.Generic;
using UnityEngine;


public class PlatformManager : MonoBehaviour
{
	[Header("Platform Spawning")]
	[SerializeField] private Transform _platformParent;
	[SerializeField] private Vector2 _platformSize = new Vector2(1.7f, 0.45f);

	[SerializeField] private float _spawnX;
	[SerializeField] private float _destroyX;
	public float destroyX => _destroyX;
	public float spawnX => _spawnX;

	[SerializeField] private float _topY;
	[SerializeField] private float _bottomY;
	public float topY => _topY;
	public float bottomY => _bottomY;

	[SerializeField] private List<GameObject> _platforms;
	[SerializeField] private List<float> _probabilities;

	[Monitor] private ulong idCounter = 0;

	//[Monitor]
	//[MShowIf(Condition.CollectionNotEmpty)]
	private List<GameObject> _spawnedPlatforms = new List<GameObject>();
	private GameObject _latestPlatform;
	private Vector2 _latestPlatformSize;

	[Header("Platform Movement")]
	[SerializeField] private float _baseLevelSpeed = 3.0f;

	private float _levelSpeed;
	[Monitor] public float levelSpeed => _levelSpeed;

	[Monitor] public int currentLevel { get; private set; }

	private Vector2 _trackingSize;
	private float _elapsedTime = 0;

	private Vector2 _startingPlatformSize => new Vector2(_spawnX - _destroyX, _platformSize.y);
	private float startingPlatformOffset = 1.0f;

	private DifficultyManager difficultyManager;

	void Start()
	{
		difficultyManager = GetComponent<DifficultyManager>();
		_levelSpeed = _baseLevelSpeed;
		spawnStartingPlatform();
		_trackingSize = _startingPlatformSize;
		_elapsedTime = (_trackingSize.x - startingPlatformOffset - _platformSize.x * 0.5f) / _levelSpeed;
	}

	void Update()
	{
		_elapsedTime += Time.deltaTime;
		float waitTime = _trackingSize.x / _levelSpeed - Time.deltaTime;
		if (_elapsedTime >= waitTime)
		{
			_trackingSize = _platformSize;
			spawnRandomPlatform();
			_elapsedTime = 0;
		}

		currentLevel = difficultyManager.calculateLevel(idCounter);
		_levelSpeed = difficultyManager.calculateLevelSpeed(currentLevel, _baseLevelSpeed);
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

		Vector2 position = new Vector2(_spawnX - _levelSpeed * Time.deltaTime, _bottomY);
		spawnPlatform(platform, position, _trackingSize);
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

		ulong id = idCounter++;

		sPlatform.name = $"Platform {id}";
		movement.Id = id;
		movement.registerPlatform(this);
		renderer.size = size;
		collider.size = size;

		_latestPlatform = sPlatform;
		_latestPlatformSize = size;
	}
	private void destroyPlatform(GameObject platform)
	{
		unregisterPlatform(platform);
		if (Application.isEditor) DestroyImmediate(platform);
		else Destroy(platform);
	}

	public void registerPlatform(GameObject platform) => _spawnedPlatforms.Add(platform);
	public bool unregisterPlatform(GameObject platform) => _spawnedPlatforms.Remove(platform);

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

	private void Awake()
	{
		this.StartMonitoring();
	}

	private void OnDestroy()
	{
		this.StopMonitoring();
	}
}
