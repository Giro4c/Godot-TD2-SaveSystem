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
	
	private Godot.Collections.Dictionary<string, Variant> Save(CharacterBody2D playerNode)
	{
		string sceneFilePath = "res://Player.tscn";
		bool orientation = (bool)playerNode.Get("orientation");
		return new Godot.Collections.Dictionary<string, Variant>()
		{
			{ "Filename", sceneFilePath },
			{ "Parent", playerNode.GetParent().GetPath() },
			{ "PosX", playerNode.Position.X },
			{ "PosY", playerNode.Position.Y },
			{ "Orientation", orientation }
		};
	}
	public void SaveGame()
	{
		var playerNode = GetNode<CharacterBody2D>(playerNodePath);
		if (playerNode == null)
		{
			GD.PrintErr("Player node not found!");
			return;
		}
		var nodeData = Save(playerNode);
		using var saveFile = FileAccess.Open("user://savegame.save", FileAccess.ModeFlags.Write);

		var saveNode = GetNode(playerNodePath);
		if (string.IsNullOrEmpty(saveNode.SceneFilePath))
		{
			GD.Print($"persistent node '{saveNode.Name}' is not an instanced scene, skipped");
		}
		var jsonString = Json.Stringify(nodeData);

		saveFile.StoreLine(jsonString);
	}
	public async  void LoadGame()
	{
		if (!FileAccess.FileExists("user://savegame.save"))
		{
			return;
		}

		var saveNode = GetNode<CharacterBody2D>(playerNodePath);
		saveNode.QueueFree();
		var timer = GetTree().CreateTimer(0.1f);
		await ToSignal(timer, "timeout");
		
		using var saveFile = FileAccess.Open("user://savegame.save", FileAccess.ModeFlags.Read);

		while (saveFile.GetPosition() < saveFile.GetLength())
		{
			var jsonString = saveFile.GetLine();

			// Creates the helper class to interact with JSON.
			var json = new Json();
			var parseResult = json.Parse(jsonString);
			if (parseResult != Error.Ok)
			{
				GD.Print($"JSON Parse Error: {json.GetErrorMessage()} in {jsonString} at line {json.GetErrorLine()}");
				continue;
			}

			// Get the data from the JSON object.
			var nodeData = new Godot.Collections.Dictionary<string, Variant>((Godot.Collections.Dictionary)json.Data);
			// Firstly, we need to create the object and add it to the tree and set its position.
			var newObjectScene = GD.Load<PackedScene>(nodeData["Filename"].ToString());
			var newObject = newObjectScene.Instantiate<CharacterBody2D>();
			GetNode(nodeData["Parent"].ToString()).AddChild(newObject);
			newObject.Set(CharacterBody2D.PropertyName.Position, new Vector2((float)nodeData["PosX"], (float)nodeData["PosY"]));
			newObject.Set("orientation", (bool)nodeData["Orientation"]);
			newObject.Scale = new Vector2(4f, 4f);
			

		}
	}
}
