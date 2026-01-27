using UnityEngine;

public class Planet : MonoBehaviour, IClickable
{
    [SerializeField] private string _planetName;
    [SerializeField] private float _rotationSpeed = 30f;
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

    private void Update()
    {
        transform.Rotate(0, 0, _rotationSpeed * Time.deltaTime);
    }
}
