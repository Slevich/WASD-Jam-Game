using TMPro;
using UnityEngine;

public class TextEquality : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _resource;
    [SerializeField] private TextMeshProUGUI _target;

    public void EqualText()
    {
        if(_resource != null && _target != null)
        {
            _target.text = _resource.text;
        }
    }
}
