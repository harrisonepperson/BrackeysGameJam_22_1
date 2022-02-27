using Godot;
using System;

public class Settings : Popup
{
	private Singleton singleton;
	private bool isModalOpen = false;
	
	private bool fullScreenEnabled;
	
	private float masterAudioLevel;
	private float musicAudioLevel;
	private float sfxAudioLevel;
	
	public override void _Ready()
	{
		singleton = GetNode<Singleton>("/root/Singleton");
		
		fullScreenEnabled = singleton.fullScreenEnabled;
		
		masterAudioLevel = singleton.masterAudioLevel;
		musicAudioLevel = singleton.musicAudioLevel;
		sfxAudioLevel = singleton.sfxAudioLevel;
	}
	
	private void _on_Button_input_event(object viewport, object @event, int shape_idx)
	{
		if (OS.HasTouchscreenUiHint() && !isModalOpen && @event is InputEventScreenTouch touch && touch.Pressed) {
			PopupCenteredRatio();
		} else if (!isModalOpen && @event is InputEventMouseButton click && click.ButtonIndex == 1 && click.Pressed) {
			PopupCenteredRatio();
		}
	}
	
	private void _on_Settings_popup_hide()
	{
		isModalOpen = false;
	}
	
	private void _on_Settings_about_to_show()
	{
		isModalOpen = true;
		
		GetNode<CheckButton>("MarginContainer/GridContainer/Full_Screen_Option").Pressed = singleton.fullScreenEnabled;
		
		GetNode<HSlider>("MarginContainer/GridContainer/Master_Audio_Level").Value = singleton.masterAudioLevel;
		GetNode<HSlider>("MarginContainer/GridContainer/Music_Audio_Level").Value = singleton.musicAudioLevel;
		GetNode<HSlider>("MarginContainer/GridContainer/SFX_Audio_Level").Value = singleton.sfxAudioLevel;
	}
	
	private void _on_Save_button_up()
	{
		fullScreenEnabled = GetNode<CheckButton>("MarginContainer/GridContainer/Full_Screen_Option").Pressed;
		
		masterAudioLevel = (float)GetNode<HSlider>("MarginContainer/GridContainer/Master_Audio_Level").Value;
		musicAudioLevel = (float)GetNode<HSlider>("MarginContainer/GridContainer/Music_Audio_Level").Value;
		sfxAudioLevel = (float)GetNode<HSlider>("MarginContainer/GridContainer/SFX_Audio_Level").Value;

		if (masterAudioLevel == -10) {
			AudioServer.SetBusMute(0, true);
		} else {
			AudioServer.SetBusMute(0, false);
			AudioServer.SetBusVolumeDb(0, masterAudioLevel);
		}

		if (musicAudioLevel == -10) {
			AudioServer.SetBusMute(1, true);
		} else {
			AudioServer.SetBusMute(1, false);
			AudioServer.SetBusVolumeDb(1, musicAudioLevel);
		}

		if (sfxAudioLevel == -10) {
			AudioServer.SetBusMute(2, true);
		} else {
			AudioServer.SetBusMute(2, false);
			AudioServer.SetBusVolumeDb(2, sfxAudioLevel);
		}
		
		OS.WindowFullscreen = fullScreenEnabled;
		
		singleton.fullScreenEnabled = fullScreenEnabled;
		
		singleton.masterAudioLevel = masterAudioLevel;
		singleton.musicAudioLevel = musicAudioLevel;
		singleton.sfxAudioLevel = sfxAudioLevel;
	}
}
