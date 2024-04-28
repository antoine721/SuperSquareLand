using UnityEngine;

public class CameraChangeProfileTrigger : MonoBehaviour
{
    [Header("Camera Profile")]
    [SerializeField] private CameraProfile _cameraProfile;
    
    [Header("Trigger Box")]
    [SerializeField] private BoxCollider2D _boxCollider;

    private void OnDrawGizmos()
    {
        if (_boxCollider == null) return;
        Color gizmosColor = Color.green;
        gizmosColor.a = 0.05f;
        Gizmos.color = gizmosColor;
        Vector3 boxCenter = _boxCollider.transform.position + (Vector3)_boxCollider.offset;
        Vector3 boxSize = _boxCollider.size;
        Gizmos.DrawCube(boxCenter, boxSize);
        Gizmos.color = Color.white;
    }
}