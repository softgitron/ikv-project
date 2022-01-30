using System;

public class PlayerThemeSync : StateSync<Boolean>
{
	private string themeName;
	private AudioSource audioSource;
	public StateSyncImpl<Boolean> stateSync;

	public PlayerThemeSync(AudioSource audioSource, string themeName)
	{
		this.themeName = themeName;
		this.audioSource = audioSource;
		stateSync = new StateSyncImpl<Boolean>(this, false, "ThemeSync");
	}

	public void SetThemeState(bool state)
	{
		this.stateSync.SetState(state);
	}
	
	public void StateTrigger(Boolean newState) {
		if (newState)
		{
			this.audioSource.FadeInTrack(this.themeName);
		}
		else
		{
			this.audioSource.FadeOutTrack(this.themeName);
		}
	}
}
