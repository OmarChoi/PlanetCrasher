using System;
using UnityEditor.Overlays;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance;
    // CRUD
    // 재화에 대한 생성 / 조회 / 사용 / 소모 / 이벤트

    public double Gold => Get(ECurrencyType.Gold);
    public static event Action OnDataChanged;

    private double[] _currencies = new double[(int)ECurrencyType.Count];
    private ICurrencyRepository _repository;
    
    private void Awake()
    {
        Instance = this;
        _repository = new LocalCurrencyRepository();
        OnDataChanged += Save;
    }

    private void Start()
    {
        _currencies = _repository.Load().Currencies;
    }

    public double Get(ECurrencyType type)
    {
        return _currencies[(int)type];
    }

    public void Add(ECurrencyType type, double amount)
    {
        _currencies[(int)type] = amount;
        OnDataChanged?.Invoke();
    }
    
    public bool TrySpendGold(ECurrencyType type, double amount)
    {
        if (_currencies[(int)type] >= amount)
        {
            _currencies[(int)type]  -= amount;
            OnDataChanged?.Invoke();
            return true;    
        }
        
        return false;
    }

    public bool CanAfford(ECurrencyType type, double amount)
    {
        return _currencies[(int)type] >= amount;
    }

    private void Save()
    {
        _repository.Save(new CurrencySaveData()
        {
            Currencies = _currencies
        });
    }
}
