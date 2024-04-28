using UnityEngine;

public class ApplySpeedXExample : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    
    private void Update()
    {
        Vector3 position = transform.position;
        position.x += _speed * Time.deltaTime;
        transform.position = position;
    }
}

