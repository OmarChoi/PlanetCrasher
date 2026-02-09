using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class CurrencyManager : Singleton<CurrencyManager>
{
    // CRUD
    // 재화에 대한 생성 / 조회 / 사용 / 소모 / 이벤트

    public Currency Gold => Get(ECurrencyType.Gold);
    public static event Action OnDataChanged;

    private readonly Currency[] _currencies = new Currency[(int)ECurrencyType.Count];
    private ICurrencyRepository _repository;

    protected override void Initialize()
    {
        // _repository = new LocalCurrencyRepository(AccountManager.Instance.Email);
        _repository = new FirebaseCurrencyRepository();
        LoadData().Forget();
        OnDataChanged += SaveData;
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
    
    public bool TrySpend(ECurrencyType type, Currency amount)
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

    #region Save/Load
    private void SaveData()
    {
        double[] values = new double[_currencies.Length];
        for (int i = 0; i < _currencies.Length; i++)
        {
            values[i] = (double)_currencies[i];
        }
        
        var saveData = new CurrencySaveData
        {
            Currencies = values
        };
        _repository.Save(saveData);
    }

    private async UniTaskVoid LoadData()
    {
        CurrencySaveData loadData = await _repository.Load();
        for (int i = 0; i < _currencies.Length; i++)
        {
            _currencies[i] = loadData.Currencies[i];
        }
        OnDataChanged?.Invoke();
    }
    #endregion Save/Load
}
