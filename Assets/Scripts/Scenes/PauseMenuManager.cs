using UnityEngine;

public class PauseMenuManager : MonoBehaviour
{

	[SerializeField] private GameObject _pausePanel;
	[SerializeField] private GameObject _pauseButton;

	private GameManager _gameManager;

	void Start()
	{
		_gameManager = GetComponent<GameManager>();
	}

	void Update()
	{
		if (_gameManager.isPaused)
		{
			_pausePanel.SetActive(true);
			_pauseButton.SetActive(false);
		} else
		{
			_pausePanel.SetActive(false);
			_pauseButton.SetActive(true);
		}
	}

}
