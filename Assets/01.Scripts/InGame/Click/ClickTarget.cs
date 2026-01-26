using UnityEngine;

public class ClickTarget : MonoBehaviour, IClickable
{
    [SerializeField] private string _planetName;
    private IFeedback[] _feedbacks;

    private void Awake()
    {
        _feedbacks = GetComponentsInChildren<IFeedback>();
    }
    
    public bool OnClick(ClickInfo clickInfo)
    {
        foreach (IFeedback feedback in _feedbacks)
        {
            feedback.Play(clickInfo);
        }
        return true;
    }
    
}
