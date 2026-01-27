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
            int damage = GameManager.Instance.AutoDamage;
            ClickInfo clickInfo = new ClickInfo
            {
                Type = EClickType.AutoClick,
                Damage = damage,
                Position = clickable.transform.position,
                EffectParticle = null
            };
            
            clickableComponent.OnClick(clickInfo);
            GameManager.Instance.AddGold(damage);
        }
    }
}
