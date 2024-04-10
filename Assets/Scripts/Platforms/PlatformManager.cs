using Baracuda.Monitoring;
using System.Collections.Generic;
using UnityEngine;


public class PlatformManager : MonoBehaviour
{
	[Header("General")]
	[SerializeField] private GameManager gameManager;

	[Header("Platform Spawning")]
	[SerializeField] private bool _showDebugLines = true;
	[SerializeField] private bool _debugPlatformSpawning = false;
	[SerializeField] private Transform _platformParent;
	[SerializeField] private float _platformWidth = 1.7f;

	public float platformWidth => _platformWidth;

	[SerializeField] private float _spawnX;
	[SerializeField] private float _destroyX;

	public float destroyX => _destroyX;
	public float spawnX => _spawnX;

	[SerializeField] private float _topY;
	[SerializeField] private float _bottomY;
	[SerializeField][Range(1, 30)] private int _heightLevels = 13;

	public float topY => _topY;
	public float bottomY => _bottomY;

	[SerializeField] private List<GameObject> _platforms;
	[SerializeField] private List<float> _probabilities;


	//[Monitor]
	//[MShowIf(Condition.CollectionNotEmpty)]
	private List<GameObject> _spawnedPlatforms = new List<GameObject>();
	private int _latestHeightIndex;


	private Vector2 _trackingSize;
	private float _elapsedTime = 0;

	private float _platformHeight => Mathf.Abs(_topY - _bottomY) / _heightLevels;
	private Vector2 _platformSize => new Vector2(_platformWidth, _platformHeight);

	private Vector2 _startingPlatformSize => new Vector2(_spawnX - _destroyX, _platformSize.y);
	private float startingPlatformOffset = 1.0f;


	private float _startTime = 0.0f;
	public float startTime => _startTime;

	public float levelSpeed => gameManager.levelSpeed;
	public int selectedColorIndex => gameManager.selectedColorIndex;
	public int numberOfColors => gameManager.numberOfColors;

	void Start()
	{
		spawnStartingPlatform();
		_trackingSize = _startingPlatformSize;
		_elapsedTime = (_trackingSize.x - startingPlatformOffset - _platformSize.x * 0.5f) / levelSpeed;

		_startTime = Time.time;
		_latestHeightIndex = 0;
	}

	void Update()
	{
		_elapsedTime += Time.deltaTime;
		float waitTime = _trackingSize.x / levelSpeed - Time.deltaTime;
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
		spawnPlatform(platform, new Vector2(startingPlatformOffset, _bottomY), _startingPlatformSize, true);
	}

	public void spawnRandomPlatform()
	{
		int platformIndex = Random.Range(0, _platforms.Count);
		GameObject platform = _platforms[platformIndex];

		int heightIndex =
		getRandomHeightIndex(_latestHeightIndex);
		_latestHeightIndex = heightIndex;
		float yPos = indexToYPos(heightIndex);

		Vector2 position = new Vector2(_spawnX - levelSpeed * Time.deltaTime, yPos);
		spawnPlatform(platform, position, _trackingSize, false);
	}

	public void destroyAllPlatforms()
	{
		foreach (GameObject go in _spawnedPlatforms) destroyPlatform(go);
	}

	private void spawnPlatform(GameObject platform, Vector2 position, Vector2 size, bool isStartingPlatform)
	{
		GameObject sPlatform = (GameObject)Instantiate(platform, position, Quaternion.identity, _platformParent);

		SpriteRenderer renderer = sPlatform.GetComponent<SpriteRenderer>();
		BoxCollider2D collider = sPlatform.GetComponent<BoxCollider2D>();
		PlatformMovement movement = sPlatform.GetComponent<PlatformMovement>();

		ulong id = gameManager.getNextIdCounter();

		sPlatform.name = $"Platform {id}";
		movement.Id = id;
		movement.isStartingPlatform = isStartingPlatform;

		if (_debugPlatformSpawning) movement.isStartingPlatform = true;

		movement.registerPlatform(this);
		renderer.size = size;
		collider.size = size;
	}
	private void destroyPlatform(GameObject platform)
	{
		unregisterPlatform(platform);
		if (Application.isEditor) DestroyImmediate(platform);
		else Destroy(platform);
	}

	public void registerPlatform(GameObject platform) => _spawnedPlatforms.Add(platform);
	public bool unregisterPlatform(GameObject platform) => _spawnedPlatforms.Remove(platform);


	float indexToYPos(int index) => _bottomY + _platformHeight * index;
	int getRandomHeightIndex(int prevIndex)
	{
		if (_debugPlatformSpawning) return 0;

		float p = Random.value;
		int diff;

		if (p <= 0.5f) //P(Same Height) = 50%
		{
			diff = 0;
		}
		else
		{
			bool isUp = Random.Range(0, 2) > 0;
			int size = isUp ? 1 : -1;

			if (p <= 0.8f) // P(Diff 1) = 30%
			{
				diff = 1 * size;
			}
			else if (p <= 0.95f) // P(Diff 2) = 15%
			{
				diff = 2 * size;
			}
			else // P(Diff 3) = 5%
			{
				diff = 3 * size;
			}
		}
		int index = prevIndex + diff;

		if (index >= _heightLevels || index < 0)
		{
			diff *= -1;
		}
		index = prevIndex + diff;

		if (index >= _heightLevels) index = _heightLevels - 1;
		if (index < 0) index = 0;

		return index;
	}

	private void OnDrawGizmos()
	{
		if (!_showDebugLines) return;

		float height = _platformHeight;

		Gizmos.color = Color.green;
		Gizmos.DrawLine(
			new Vector3(_spawnX, _topY - height * 0.5f),
			new Vector3(_spawnX, _bottomY - height * 0.5f)
		);

		Gizmos.color = Color.red;
		Gizmos.DrawLine(
			new Vector3(_destroyX, _topY - height * 0.5f),
			new Vector3(_destroyX, _bottomY - height * 0.5f)
		);

		Gizmos.color = Color.blue;
		Gizmos.DrawLine(
			new Vector3(_spawnX, _topY - height * 0.5f),
			new Vector3(_destroyX, _topY - height * 0.5f)
		);

		Gizmos.color = Color.blue;
		Gizmos.DrawLine(
			new Vector3(_spawnX, _bottomY - height * 0.5f),
			new Vector3(_destroyX, _bottomY - height * 0.5f)
		);

		Gizmos.color = Color.yellow;
		for (int i = 1; i < _heightLevels; ++i)
		{
			float y = _bottomY + i * height - height * 0.5f;
			Gizmos.DrawLine(
				new Vector3(_spawnX, y),
				new Vector3(_destroyX, y)
			);
		}
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
