using Godot;
using System;

public partial class Player : CharacterBody2D
{
	private const SPEED = 130.0;
	@onready var animated_sprite = $AnimatedSprite2D
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	public override void _Physics_Process(double delta)
	{
		GetInput();
		move_and_slide();
		return;
	}
	
	public void GetInput(){
		// Get the direction
		var direction = Input.get_vector("left", "right", "up", "down");
		
		// Visual
			// Animation
		if (direction){
			animated_sprite.play("run");
		}
		else {
			animated_sprite.play("idle");
		}
			// Direction facing
		if (direction.x){
			if (direction.x > 0) {
				animated_sprite.flip_h = false;
			}
			else {
				animated_sprite.flip_h = true;
			}
		}
			
		// Apply velocity for movement
		if (direction != Vector2.ZERO) {
			velocity = direction * SPEED;
		}
		else {
			velocity.x = move_toward(velocity.x, 0, SPEED);
			velocity.y = move_toward(velocity.y, 0, SPEED);
		}
		return;
	}
}
