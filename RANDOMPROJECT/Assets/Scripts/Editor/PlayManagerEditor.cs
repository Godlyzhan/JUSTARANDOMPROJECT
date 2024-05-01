using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayAreaManager))]
public class PlayManagerEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		PlayAreaManager playAreaManager = (PlayAreaManager)target;

		if (GUILayout.Button("Instantiate cards"))
		{
			playAreaManager.InstantiateCards();
		}
	}
}
