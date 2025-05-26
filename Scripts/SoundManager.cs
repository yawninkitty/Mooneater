using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour
{
    public AudioClip[] sounds; // Массив звуков
    private AudioSource audioSource;
    private bool isFootstepPlaying;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    // Метод для воспроизведения звука по индексу
    public void PlaySound(int soundIndex)
    {
        if (soundIndex >= 0 && soundIndex < sounds.Length && sounds[soundIndex] != null)
        {
            audioSource.PlayOneShot(sounds[soundIndex]);
        }
        else
        {
            Debug.LogWarning("Звук с индексом " + soundIndex + " не найден или массив пуст!");
        }
    }
}
