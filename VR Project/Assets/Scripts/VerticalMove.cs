using UnityEngine;

public class VerticalMove : MonoBehaviour
{
    public Vector3 moveOffset = new Vector3(0, 5, 0); // Move up 5 meters
    public float speed = 1f;
    private Vector3 startPos;

    void Start() { startPos = transform.position; }

    void Update()
    {
        // Move back and forth using Sine wave
        transform.position = startPos + moveOffset * Mathf.PingPong(Time.time * speed, 1f);
    }
}