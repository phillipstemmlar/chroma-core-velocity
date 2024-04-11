using UnityEngine;

public class DifficultyManager : MonoBehaviour
{

	[SerializeField] private bool _debugDifficultyLevel;

	public int calculateLevel(ulong platformId)
	{
		if (_debugDifficultyLevel)
		{
			return floor(linear(platformId, 1.0f / 10.0f));
			// return floor(linear(platformId, 1.0f / 100.0f));
		}
		return floor(logarithmic(platformId, 60.0f));
	}
	public float calculateLevelSpeed(int level, float baseSpeed)
	{
		return linear(level, 1, baseSpeed);
	}

	public int calculateLevelNumberOfColors(int level, float baseNumberOfColors)
	{
		return floor(linear(level, 1, baseNumberOfColors));
	}
	// AUXILARY FUNCTIONS

	private int floor(float value) => Mathf.FloorToInt(value);


	// MATH FUNCTIONS


	private float linear(float x, float m = 1, float q = 0)
	{
		return m * x + q;
	}

	private float logarithmic(float x, float n = 60.0f)
	{
		//f\left(x\right) =\ln\left(\frac{ x + n}/{ n}\right)
		return Mathf.Log((x + n) / n);
	}
}
