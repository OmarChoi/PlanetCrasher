using UnityEngine;

public class ClickTarget : MonoBehaviour, IClickable
{
    [SerializeField] private string _planetName;
    public bool OnClick()
    {
        Debug.Log($"{_planetName} 파괴 진행 중");
        return true;
    }
}
