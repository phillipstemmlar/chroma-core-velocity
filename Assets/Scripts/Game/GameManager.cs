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

	// Start is called before the first frame update
	void Start()
	{

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
	}

	public void PauseGame() => _isPaused = true;
	public void ResumeGame() => _isPaused = false;
	public void TogglePauseGame() => _isPaused = !_isPaused;

	public void updateSelectedColorIndex(int index) => _selectedColorIndex = index;

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
