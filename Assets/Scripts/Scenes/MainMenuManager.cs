using UnityEngine;

public class MainMenuManager : MonoBehaviour
{

	[SerializeField] private RectTransform _mainPanel;
	[SerializeField] private RectTransform _optionsPanel;
	[SerializeField] private RectTransform _creditsPanel;

	[SerializeField] private Vector2 shownOffset = Vector2.zero;
	[SerializeField] private Vector2 hiddenOffset;

	[SerializeField] private float transitionMilliseconds = 1000.0f;

	[SerializeField] private AnimationCurve transitionCurve = AnimationCurve.Linear(0, 0, 1, 1);

	private float elapsedTime = 0.0f;

	public enum MenuPanel { Main, Options, Credits }

	private MenuPanel targetPanel = MenuPanel.Main;
	private MenuPanel currentPanel = MenuPanel.Main;

	private Vector2 mainStart;
	private Vector2 optionsStart;
	private Vector2 creditsStart;

	private Vector2 hiddenOffsetOther => new Vector2(-1 * hiddenOffset.x, -1 * hiddenOffset.y);

	void Start()
	{
		if (_mainPanel != null) _mainPanel.anchoredPosition = shownOffset;
		if (_optionsPanel != null) _optionsPanel.anchoredPosition = hiddenOffset;
		if (_creditsPanel != null) _creditsPanel.anchoredPosition = hiddenOffset;

		mainStart = shownOffset;
		optionsStart = hiddenOffset;
		creditsStart = hiddenOffset;
	}

	void Update()
	{
		if (targetPanel == currentPanel) return;

		elapsedTime += Time.deltaTime;

		float durationInSeconds = transitionMilliseconds / 1000.0f;
		float t = elapsedTime / durationInSeconds;

		bool isComplete = false;
		if (t >= 1.0f)
		{
			isComplete = true;
			t = 1.0f;
		}

		Vector2 mainTarget = mainStart;
		Vector2 optionsTarget = optionsStart;
		Vector2 creditTarget = creditsStart;

		if (targetPanel == MenuPanel.Main) mainTarget = shownOffset;
		else if (targetPanel == MenuPanel.Options) optionsTarget = shownOffset;
		else if (targetPanel == MenuPanel.Credits) creditTarget = shownOffset;

		if (currentPanel == MenuPanel.Main) mainTarget = hiddenOffsetOther;
		else if (currentPanel == MenuPanel.Options) optionsTarget = hiddenOffset;
		else if (currentPanel == MenuPanel.Credits) creditTarget = hiddenOffset;

		t = transitionCurve.Evaluate(t);

		if (_mainPanel != null && mainTarget != mainStart) _mainPanel.anchoredPosition = Vector2.Lerp(mainStart, (Vector2)mainTarget, t);
		if (_optionsPanel != null && optionsTarget != optionsStart) _optionsPanel.anchoredPosition = Vector2.Lerp(optionsStart, (Vector2)optionsTarget, t);
		if (_creditsPanel != null && creditTarget != creditsStart) _creditsPanel.anchoredPosition = Vector2.Lerp(creditsStart, (Vector2)creditTarget, t);

		if (isComplete)
		{
			elapsedTime = 0.0f;
			currentPanel = targetPanel;
		}
	}

	public void TransitionToMainPanel()
	{
		elapsedTime = 0.0f;
		targetPanel = MenuPanel.Main;
		if (_mainPanel != null) mainStart = _mainPanel.anchoredPosition;
		if (_optionsPanel != null) optionsStart = _optionsPanel.anchoredPosition;
		if (_creditsPanel != null) creditsStart = _creditsPanel.anchoredPosition;
	}

	public void TransitionToOptionPanel()
	{
		elapsedTime = 0.0f;
		targetPanel = MenuPanel.Options;
		if (_mainPanel != null) mainStart = _mainPanel.anchoredPosition;
		if (_optionsPanel != null) optionsStart = _optionsPanel.anchoredPosition;
		if (_creditsPanel != null) creditsStart = _creditsPanel.anchoredPosition;
	}

	public void TransitionToCreditsPanel()
	{
		elapsedTime = 0.0f;
		targetPanel = MenuPanel.Credits;
		if (_mainPanel != null) mainStart = _mainPanel.anchoredPosition;
		if (_optionsPanel != null) optionsStart = _optionsPanel.anchoredPosition;
		if (_creditsPanel != null) creditsStart = _creditsPanel.anchoredPosition;
	}

}
