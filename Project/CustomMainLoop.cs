using Godot;

[GlobalClass]
public partial class CustomMainLoop : SceneTree
{

    private static CustomMainLoop _mainLoop = null;
    public static CustomMainLoop MainLoop => _mainLoop;
    
    public override void _Initialize()
    {
        GD.Print("Initialize Start");
        
        LevelManager.Initialize();
        GD.Print(LevelManager.manager != null);

        _mainLoop = this;
        GD.Print("Initialize End");
    }

    public LevelManager GetLevelManager()
    {
        return LevelManager.manager;
    }
    
}