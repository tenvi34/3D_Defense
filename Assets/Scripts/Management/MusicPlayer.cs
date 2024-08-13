using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public AudioClip backgroundMusic;

    private void Start()
    {
        if (backgroundMusic != null)
        {
            BackgroundMusicManager.Instance.PlayMusic(backgroundMusic);
        }
        else
        {
            Debug.LogWarning("배경음악이 없습니다.");
        }
    }
}
