using UnityEngine;

public class ApplySpeedVectorExample : MonoBehaviour
{
    [SerializeField] private Vector3 _velocity 
        = new Vector3(5f, 2f, 0f);

    private void Update()
    {
        transform.position += _velocity * Time.deltaTime;
    }
}

