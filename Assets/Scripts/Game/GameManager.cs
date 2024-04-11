using System.Collections.Generic;
using Baracuda.Monitoring;
using UnityEngine;

public class GameState
{
	public int currentLevel;
	public float levelSpeed;
	public int numberOfColors;
	public int selectedColorIndex;
}

public class GameManager : MonoBehaviour, ObserverBroadcaster<GameState>
{
	// ===== OBSERVER =====
	private List<ObserverListener<GameState>> observers = new List<ObserverListener<GameState>>();
	private GameState prevState = null;
	private GameState state;

	// ====================

	[SerializeField] private bool _showDebugLines = false;

	[SerializeField] private Transform _robotPlayer;
	[SerializeField] private float _deathY;

	[SerializeField] private bool _isPlayerDead = false;
	public bool isPlayerDead => _isPlayerDead;

	[SerializeField] private bool _isPaused = false;
	public bool isPaused => _isPaused;

	[SerializeField] private int _initialSelectedColorIndex = 1;
	public int initialSelectedColorIndex => _initialSelectedColorIndex;
	public int selectedColorIndex => state.selectedColorIndex;

	[Header("Platform Movement")]
	[SerializeField] private DifficultyManager difficultyManager;
	[Monitor] private ulong idCounter = 0;
	[SerializeField] private float _baseLevelSpeed = 3.0f;
	public float baseLevelSpeed => _baseLevelSpeed;

	[Monitor] public float levelSpeed => state.levelSpeed;

	[Monitor] public int currentLevel => state.currentLevel;

	[SerializeField] private int _baseNumberOfColors = 3;
	[Monitor] public int numberOfColors => state.numberOfColors;

	// Start is called before the first frame update
	void Start()
	{
		state = new GameState
		{
			levelSpeed = _baseLevelSpeed,
			currentLevel = 0,
			numberOfColors = _baseNumberOfColors,
			selectedColorIndex = _initialSelectedColorIndex,
		};
		updateObserverState(state);
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

		state.currentLevel = difficultyManager.calculateLevel(idCounter);
		state.numberOfColors = difficultyManager.calculateLevelNumberOfColors(currentLevel, _baseNumberOfColors);
		state.levelSpeed = difficultyManager.calculateLevelSpeed(currentLevel, _baseLevelSpeed);
		updateObserverState(state);
	}

	public void PauseGame() => _isPaused = true;
	public void ResumeGame() => _isPaused = false;
	public void TogglePauseGame() => _isPaused = !_isPaused;

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

	// ===== STATE UPDATES =====

	public void updateSelectedColorIndex(int index)
	{
		state.selectedColorIndex = index;
		updateObserverState(state);
	}

	public void updateCurrentLevel(int level)
	{
		state.currentLevel = level;
		updateObserverState(state);
	}

	public void updateLevelSpeed(float speed)
	{
		state.levelSpeed = speed;
		updateObserverState(state);
	}

	public void updateNumberOfColors(int numColors)
	{
		state.numberOfColors = numColors;
		updateObserverState(state);
	}


	// ===== OBSERVER =====

	public void registerObserver(ObserverListener<GameState> observer)
	{
		observer.registerBroadcaster(this);
		observers.Add(observer);
		observer.onObserverStateChange(state, prevState);
	}

	public void deregisterObserver(ObserverListener<GameState> observer)
	{

		observers.Remove(observer);
		observer.deregisterBroadcaster();
	}

	public void updateObserverState(GameState state)
	{
		foreach (var ob in observers)
		{
			ob.onObserverStateChange(state, prevState);
		}
		prevState = state;
	}

}
