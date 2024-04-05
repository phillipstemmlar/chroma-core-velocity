using UnityEngine;

public class DeathMenuManager : MonoBehaviour
{
	[SerializeField] private GameObject _deathPanel;

	private GameManager _gameManager;

	void Start()
	{
		_gameManager = GetComponent<GameManager>();
	}

	void Update()
	{
		if (_gameManager.isPlayerDead)
		{
			_deathPanel.SetActive(true);
		} else
		{
			_deathPanel.SetActive(false);
		}
	}
}
