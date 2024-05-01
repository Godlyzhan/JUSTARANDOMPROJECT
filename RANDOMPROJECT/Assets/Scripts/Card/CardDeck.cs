using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardDeck", menuName = "MatchCardProject/Card Deck", order = 1)]
public class CardDeck : ScriptableObject
{
	public List<GameObject> Cards = new List<GameObject>();
}
