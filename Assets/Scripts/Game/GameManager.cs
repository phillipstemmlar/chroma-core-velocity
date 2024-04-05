using UnityEngine;

public class GameManager : MonoBehaviour
{
	[SerializeField] private bool _isPaused = false;
	public bool isPaused => _isPaused;

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		Time.timeScale = _isPaused ? 0.0f : 1.0f;
	}

	public void PauseGame() => _isPaused = true;
	public void ResumeGame() => _isPaused = false;
	public void TogglePauseGame() => _isPaused = !_isPaused;
}
