using Godot;
using System;

public partial class Head : Node3D
{
	private const float SENSITIVITY = 0.005f;
	private Camera3D _camera;


    public override void _Ready()
    {
        base._Ready();
		_camera = GetNode<Camera3D>("Camera3D");
		Input.MouseMode = Input.MouseModeEnum.Captured;
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        base._UnhandledInput(@event);
		if (@event is InputEventMouseMotion eMouseMotion) {
			RotateY(-eMouseMotion.Relative.X * SENSITIVITY);
			_camera.RotateX(-eMouseMotion.Relative.Y * SENSITIVITY);
		}
    }
}
