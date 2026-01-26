using UnityEngine;

public class ClickTarget : MonoBehaviour, IClickable
{
    [SerializeField] private string _planetName;
    public bool OnClick(ClickInfo clickInfo)
    {
        Debug.Log($"{_planetName} 파괴 진행 중");
        return true;
    }
}
