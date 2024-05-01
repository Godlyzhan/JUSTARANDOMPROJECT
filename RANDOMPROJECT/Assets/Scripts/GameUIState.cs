using System;
using UnityEngine;

public class GameUIState : MonoBehaviour
{
	[SerializeField] private GameObject MenuUI;
	[SerializeField] private GameObject GamePlayUI;
	[SerializeField] private GameObject EndUI;

	public enum States
	{
		Menu,
		Gameplay,
		End,
		Disabled
	}

	public States State;

	private void Start()
	{
		SetGameState();
	}

	public void SetMenuState()
	{
		State = States.Menu;
		SetGameState();
	}

	public void SetGameplayState()
	{
		State = States.Gameplay;
		SetGameState();
	}

	public void SetEndState()
	{
		State = States.End;
		SetGameState();
	}

	public void SetDisableState()
	{
		State = States.Disabled;
		SetGameState();
	}

	public void DisableObject(GameObject gameObjectToDisable)
	{
		gameObjectToDisable.SetActive(false);
	}

	public void EnableObject(GameObject gameObjectToEnable)
	{
		gameObjectToEnable.SetActive(true);
	}

	private void SetGameState()
	{
		switch (State)
		{
			case States.Menu:
				UIActiveState(MenuUI, true);
				UIActiveState(GamePlayUI, false);
				UIActiveState(EndUI, false);
				break;
			case States.Gameplay:
				UIActiveState(MenuUI, false);
				UIActiveState(GamePlayUI, true);
				UIActiveState(EndUI, false);
				break;
			case States.End:
				UIActiveState(MenuUI, false);
				UIActiveState(GamePlayUI, false);
				UIActiveState(EndUI, false);
				break;
			case States.Disabled:
				UIActiveState(MenuUI, false);
				UIActiveState(GamePlayUI, false);
				UIActiveState(EndUI, false);
				break;
		}
	}

	private void UIActiveState(GameObject ui, bool active)
	{
		ui.SetActive(active);
	}
}
