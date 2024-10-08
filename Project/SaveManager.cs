using Godot;

public partial class SaveManager : Node
{
	private const string SAVE_PATH = "user://save_config_file.ini";

	[Export]
	public NodePath gameNodePath;

	[Export]
	public NodePath playerNodePath;
	public override void _Ready()
	{
		var saveButton = GetNode<Button>("../UI/VBoxContainer/SaveLoad/SaveButton");
		saveButton.Pressed += SaveGame;
		var loadButton = GetNode<Button>("../UI/VBoxContainer/SaveLoad/LoadButton");
		loadButton.Pressed += LoadGame;
	}
	

	public void SaveGame()
	{
		var config = new ConfigFile();

		// Get the Player node (which is a GDScript node extending CharacterBody2D)
		var player = GetNode<CharacterBody2D>(playerNodePath);

		// Save player properties (retrieving them via the Get() method)
		var playerPosition = player.Position;
		var playerOrientation = player.GetOrientation();

		config.SetValue("player", "position", playerPosition);
		config.SetValue("player", "orientation", playerOrientation);

		// Save the config file
		config.Save(SAVE_PATH);

		// Enable the load button
		var loadButton = GetNode<Button>("../UI/VBoxContainer/SaveLoad/LoadButton");
		loadButton.Disabled = false;
		GD.Print("The state has been saved!");
	}
	public void LoadGame()
	{
		var config = new ConfigFile();
		var error = config.Load(SAVE_PATH);

		if (error != Error.Ok)
		{
			GD.Print("Failed to load config file: ", error);
			return;
		}

		// Get the Player node
		var player = GetNode<Node2D>(playerNodePath);

		player.Position = (Vector2)config.GetValue("player", "position");
		player.Orientation = (float)config.GetValue("player", "orientation");

		
		GD.Print("The state has been loaded!");
	}
}
