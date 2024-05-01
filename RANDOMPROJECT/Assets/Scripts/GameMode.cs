using System;
using UnityEngine;
using UnityEngine.UI;

public class GameMode : MonoBehaviour
{
	[field: SerializeField, Range(2, 5)] 
	public int Rows { get; private set; } = 2;

	[field: SerializeField, Range(2, 6)] 
	public int Columns { get; private set; } = 2;
}
