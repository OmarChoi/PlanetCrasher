using System;
using UnityEditor.Overlays;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance;
    // CRUD
    // 재화에 대한 생성 / 조회 / 사용 / 소모 / 이벤트

    public Currency Gold => Get(ECurrencyType.Gold);
    public static event Action OnDataChanged;

    private readonly Currency[] _currencies = new Currency[(int)ECurrencyType.Count];
    private ICurrencyRepository _repository;

    private void Awake()
    {
        Instance = this;
        _repository = new LocalCurrencyRepository();
        OnDataChanged += Save;
    }

    private void Start()
    {
        double[] currencyValues = _repository.Load().Currencies;
        for (int i = 0; i < _currencies.Length; i++)
        {
            _currencies[i] = currencyValues[i];
        }
    }

    public Currency Get(ECurrencyType type)
    {
        return _currencies[(int)type];
    }

    public void Add(ECurrencyType type, Currency amount)
    {
        _currencies[(int)type] += amount;
        OnDataChanged?.Invoke();
    }
    
    public bool TrySpendGold(ECurrencyType type, Currency amount)
    {
        if (_currencies[(int)type] >= amount)
        {
            _currencies[(int)type] -= amount;
            OnDataChanged?.Invoke();
            return true;    
        }
        
        return false;
    }

    public bool CanAfford(ECurrencyType type, Currency amount)
    {
        return _currencies[(int)type] >= amount;
    }

    private void Save()
    {
        var saveData = new CurrencySaveData
        {
            Currencies = new double[_currencies.Length]
        };
        _repository.Save(saveData);
    }
}
