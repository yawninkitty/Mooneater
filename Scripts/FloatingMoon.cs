using UnityEngine;

public class FloatingMoon : MonoBehaviour
{
    public float floatSpeed = 1f;    // �������� �������
    public float floatHeight = 0.5f; // ������ �������

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        // ������� �������� �����-���� � ������� ������
        float newY = startPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
    }
}
