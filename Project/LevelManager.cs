using Godot;

public partial class LevelManager : GodotObject
{
    public static LevelManager manager = null;
    public const string DEFAULT_SCENE_EXTENSION = ".tscn";

    public string RootPath { get; set; } = "res://";
    // public Node currentScene => CustomMainLoop.MainLoop.GetCurrentScene();

    public static void Initialize()
    {
        manager = new LevelManager();
    }

    public void Load(string sceneName)
    {
        string path = RootPath + sceneName + DEFAULT_SCENE_EXTENSION;
        if (CustomMainLoop.MainLoop.ChangeSceneToFile(path) != Error.Ok)
        {
            GD.PrintErr("Could not load level \"" + sceneName + "\" at path : \"" + path + "\".");
        }
        
    }
}