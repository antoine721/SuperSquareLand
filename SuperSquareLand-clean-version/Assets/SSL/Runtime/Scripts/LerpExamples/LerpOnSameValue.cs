using UnityEngine;

public class LerpOnSameValue : MonoBehaviour
{
    [SerializeField] private float _startValue = 0f;
    [SerializeField] private float _destinationValue = 20f;

    private float _currentValue = 0f;

    private void Start()
    {
        _currentValue = _startValue;
    }

    private void Update()
    {
        _currentValue = Mathf.Lerp(_currentValue, _destinationValue, 0.5f);
        transform.position = new Vector3(_currentValue, 0f, 0f);
    }

    private void OnGUI()
    {
        GUILayout.BeginVertical(GUI.skin.box);
        GUILayout.Label(gameObject.name);
        GUILayout.Label($"LerpCurrentValue = {_currentValue}");
        GUILayout.EndVertical();
    }
}


