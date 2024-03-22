using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlatformManager))]
public class PlatformManagerEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		PlatformManager myTarget = (PlatformManager)target;

		if (GUILayout.Button("Spawn Platform"))
		{
			myTarget.spawnRandomPlatform();
		}

		if (GUILayout.Button("Clear Platforms"))
		{
			myTarget.destroyAllPlatforms();
		}
	}
}