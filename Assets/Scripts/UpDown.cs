using UnityEngine;

//moves lift up and down
public class UpDown : MonoBehaviour
{
    public float amplitude = 1.0f; // Amp of the up and down movement
    public float speed = 1.0f;

    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.position;
    }

    void Update()
    {
        float newY = initialPosition.y + amplitude * Mathf.Sin(speed * Time.time);
        
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
