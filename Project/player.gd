extends CharacterBody2D

const SPEED = 400.0

@onready var animated_sprite = $AnimatedSprite2D
var orientation = false
# Update
func _process(delta: float) -> void:
	pass

# Fixed update
func _physics_process(delta: float) -> void:
	get_input()
	move_and_slide()
	pass

func get_input():
	# Get the direction
	var direction = Input.get_vector("left", "right", "up", "down")
	# Visual
		# Animation
	if direction : 
		animated_sprite.play("run")
	else:
		animated_sprite.play("idle")
	
		# Direction facing
	if direction.x :
		if direction.x > 0 :
			orientation = false
		else :
			orientation = true
	animated_sprite.flip_h = orientation
	
	# Apply velocity for movement
	if direction != Vector2.ZERO :
		velocity = direction * SPEED
	else:
		velocity.x = move_toward(velocity.x, 0, SPEED)
		velocity.y = move_toward(velocity.y, 0, SPEED)
	pass
