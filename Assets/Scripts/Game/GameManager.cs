using Baracuda.Monitoring;
using UnityEngine;

public class GameManager : MonoBehaviour
{

	[SerializeField] private bool _showDebugLines = false;

	[SerializeField] private Transform _robotPlayer;
	[SerializeField] private float _deathY;

	[SerializeField] private bool _isPlayerDead = false;
	public bool isPlayerDead => _isPlayerDead;

	[SerializeField] private bool _isPaused = false;
	public bool isPaused => _isPaused;

	public int _selectedColorIndex = 1;
	public int selectedColorIndex => _selectedColorIndex;

	[Header("Platform Movement")]
	[SerializeField] private DifficultyManager difficultyManager;
	[Monitor] private ulong idCounter = 0;
	[SerializeField] private float _baseLevelSpeed = 3.0f;
	public float baseLevelSpeed => _baseLevelSpeed;
	private float _levelSpeed;
	[Monitor] public float levelSpeed => _levelSpeed;

	[Monitor] public int currentLevel { get; private set; }

	[SerializeField] private int _baseNumberOfColors = 3;
	[Monitor] public int numberOfColors { get; set; }


	// Start is called before the first frame update
	void Start()
	{
		_levelSpeed = _baseLevelSpeed;
		updateSelectedColorIndex(1);
	}

	// Update is called once per frame
	void Update()
	{
		Time.timeScale = _isPaused || _isPlayerDead ? 0.0f : 1.0f;

		if (!_isPlayerDead)
		{
			if (_robotPlayer.position.y < _deathY)
			{
				_isPlayerDead = true;
			}
		}

		currentLevel = difficultyManager.calculateLevel(idCounter);
		numberOfColors = difficultyManager.calculateLevelNumberOfColors(currentLevel, _baseNumberOfColors);
		_levelSpeed = difficultyManager.calculateLevelSpeed(currentLevel, _baseLevelSpeed);
	}

	public void PauseGame() => _isPaused = true;
	public void ResumeGame() => _isPaused = false;
	public void TogglePauseGame() => _isPaused = !_isPaused;

	public void updateSelectedColorIndex(int index) => _selectedColorIndex = index;
	public ulong getNextIdCounter() => idCounter++;

	private void OnDrawGizmos()
	{
		if (!_showDebugLines) return;

		Vector2 center = Camera.main.transform.position;
		float width = Camera.main.scaledPixelWidth;

		Gizmos.color = Color.red;
		Gizmos.DrawLine(
			new Vector3(center.x - width * 0.5f, _deathY),
			new Vector3(center.x + width * 0.5f, _deathY)
		);
	}
}
