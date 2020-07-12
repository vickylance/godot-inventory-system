using Godot;
using System;

public class MainMenu : Control
{
	[Export]
	public string cfName = "settings.cgf";

	private ConfigFile cf = new ConfigFile();
	private Button continueButton;
	private Button newGameButton;
	private Button loadGameButton;
	private Button optionsButton;
	private Button exitButton;

	public override void _Ready()
	{
		continueButton = FindNode("Continue") as Button;
		newGameButton = FindNode("NewGame") as Button;
		loadGameButton = FindNode("LoadGame") as Button;
		optionsButton = FindNode("Options") as Button;
		exitButton = FindNode("Exit") as Button;

		continueButton.Connect("pressed", this, nameof(_ContinueButtonPressed));
		newGameButton.Connect("pressed", this, nameof(_NewGameButtonPressed));
		loadGameButton.Connect("pressed", this, nameof(_LoadGameButtonPressed));
		optionsButton.Connect("pressed", this, nameof(_OptionsButtonPressed));
		exitButton.Connect("pressed", this, nameof(_ExitButtonPressed));

		if (cf.Load("user://" + cfName) != Error.Ok)
		{
			Save();
		}
		else
		{
			Load();
		}
	}

	public void Save()
	{
		cf.SetValue("Main", "ResolutionWidth", OS.WindowSize.x);
		cf.SetValue("Main", "ResolutionHeight", OS.WindowSize.y);
		if (cf.Save("user://" + cfName) == Error.Ok)
			GD.Print("Saving data to: " + OS.GetUserDataDir() + "/" + cfName);
	}

	public void Load()
	{
		GD.Print("Loading data from: " + OS.GetUserDataDir() + "/" + cfName);
		Vector2 screenSize = new Vector2(
			(float)cf.GetValue("Main", "ResolutionWidth", OS.WindowSize.x),
			(float)cf.GetValue("Main", "ResolutionHeight", OS.WindowSize.y)
		);
		OS.WindowSize = screenSize;
	}

	private void _ExitButtonPressed()
	{
		throw new NotImplementedException();
	}

	private void _OptionsButtonPressed()
	{
		throw new NotImplementedException();
	}

	private void _LoadGameButtonPressed()
	{
		throw new NotImplementedException();
	}

	private void _NewGameButtonPressed()
	{
		throw new NotImplementedException();
	}

	private void _ContinueButtonPressed()
	{
		throw new NotImplementedException();
	}
}
