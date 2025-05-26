using UnityEngine;
using System.Collections;
using UnityEditor;

public class PlayerStatusEffects : MonoBehaviour
{
    private PlayerController _playerController;
    public Color originalColor = Color.white;
    public SoundManager soundManager;

    private void Start()
    {
        _playerController = GetComponent<PlayerController>();
        soundManager = GetComponent<SoundManager>();
    }

    public void Stun(float duration)
    { 
        if (_playerController == null) return;
        soundManager.PlaySound(4);
        StartCoroutine(StunRoutine(duration));
    }

    private IEnumerator StunRoutine(float time)
    {
        SpriteRenderer playerSprite = GetComponent<SpriteRenderer>();

        playerSprite.color = Color.yellow; // Жёлтый = оглушён
        _playerController.enabled = false;

        yield return new WaitForSeconds(time);

        playerSprite.color = originalColor;
        _playerController.enabled = true;
    }
}
