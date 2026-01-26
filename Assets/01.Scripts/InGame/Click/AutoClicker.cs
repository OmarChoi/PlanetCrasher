using UnityEngine;
using UnityEngine.UIElements;

public class AutoClicker : MonoBehaviour
{
    [SerializeField] private float _interval;
    private float _timer;

    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer < _interval) return;
        _timer = 0;

        GameObject[] clickables = GameObject.FindGameObjectsWithTag("Clickable");
        foreach (var clickable in clickables)
        {
            IClickable clickableComponent = clickable.GetComponent<IClickable>();
            ClickInfo clickInfo = new ClickInfo
            {
                Type = EClickType.Auto,
                Damage = GameManager.Instance.AutoDamage,
                Position = clickable.transform.position
            };
                
            clickableComponent.OnClick(clickInfo);
        }
    }
}
