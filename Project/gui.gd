extends VBoxContainer


func _ready() -> void:
	# Don't allow loading files that don't exist yet.
	($SaveLoad/LoadButton as Button).disabled = not FileAccess.file_exists("user://save_config_file.ini")

func _on_open_user_data_folder_pressed() -> void:
	OS.shell_open(ProjectSettings.globalize_path("user://"))
