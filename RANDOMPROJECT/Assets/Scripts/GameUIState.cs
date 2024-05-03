using UnityEngine;

public class GameUIState : MonoBehaviour
{
	[SerializeField] private GameObject MenuUI;
	[SerializeField] private GameObject GameModeUI;
	[SerializeField] private GameObject GamePlayUI;
	[SerializeField] private GameObject EndUI;

	public enum States
	{
		Menu,
		Gameplay,
		End,
		GameMode,
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

	public void SetGameModeState()
	{
		State = States.GameMode;
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

	private void SetGameState()
	{
		switch (State)
		{
			case States.Menu:
				UIActiveState(MenuUI, true);
				UIActiveState(GameModeUI, false);
				UIActiveState(GamePlayUI, false);
				UIActiveState(EndUI, false);
				break;
			case States.GameMode:
				UIActiveState(MenuUI, false);
				UIActiveState(GameModeUI, true);
				UIActiveState(GamePlayUI, false);
				UIActiveState(EndUI, false);
				break;
			case States.Gameplay:
				UIActiveState(MenuUI, false);
				UIActiveState(GameModeUI, false);
				UIActiveState(GamePlayUI, true);
				UIActiveState(EndUI, false);
				break;
			case States.End:
				UIActiveState(MenuUI, false);
				UIActiveState(GameModeUI, false);
				UIActiveState(GamePlayUI, false);
				UIActiveState(EndUI, true);
				break;
			case States.Disabled:
				UIActiveState(MenuUI, false);
				UIActiveState(GameModeUI, false);
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
