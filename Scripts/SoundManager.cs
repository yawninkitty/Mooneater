using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour
{
    public AudioClip[] sounds; // ������ ������
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

    // ����� ��� ��������������� ����� �� �������
    public void PlaySound(int soundIndex)
    {
        if (soundIndex >= 0 && soundIndex < sounds.Length && sounds[soundIndex] != null)
        {
            audioSource.PlayOneShot(sounds[soundIndex]);
        }
        else
        {
            Debug.LogWarning("���� � �������� " + soundIndex + " �� ������ ��� ������ ����!");
        }
    }
}
