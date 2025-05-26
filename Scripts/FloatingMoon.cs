using UnityEngine;

public class FloatingMoon : MonoBehaviour
{
    public float floatSpeed = 1f;    // Скорость парения
    public float floatHeight = 0.5f; // Высота парения

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        // Плавное движение вверх-вниз с помощью синуса
        float newY = startPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
    }
}
