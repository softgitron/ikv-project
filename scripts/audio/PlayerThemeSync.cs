namespace DefaultNamespace;

public class PlayerThemeSync : StateSync<Boolean>
{
    private string themeName;
    private AudioSource audioSource;

    public PlayerThemeSync(AudioSource audioSource, string themeName)
    {
        this.themeName = themeName;
        this.audioSource = audioSource;
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