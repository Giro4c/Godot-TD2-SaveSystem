using Godot;

public partial class LevelButton : Button
{
    protected virtual string targetLevel => "main";
    
    public override void _Pressed()
    {
        GD.Print("Start press");
        base._Pressed();
        GD.Print("Super:: done");
        CustomMainLoop.MainLoop.GetLevelManager().Load(targetLevel);
        GD.Print("End press");
    }
}