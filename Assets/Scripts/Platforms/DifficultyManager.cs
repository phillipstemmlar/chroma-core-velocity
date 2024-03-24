using UnityEngine;

public class DifficultyManager : MonoBehaviour
{

	public int calculateLevel(ulong platformId)
	{
		//return level(linear(platformId, 1.0f / 10.0f));
		return level(logarithmic(platformId, 60.0f));
	}
	public float calculateLevelSpeed(int level, float baseSpeed)
	{
		return linear(level, 1, baseSpeed);
	}

	// AUXILARY FUNCTIONS

	private int level(float value) => Mathf.FloorToInt(value);


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
