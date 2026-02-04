using Cysharp.Threading.Tasks;

public interface ICurrencyRepository
{
    public UniTaskVoid Save(CurrencySaveData saveData);
    public UniTask<CurrencySaveData> Load();
}