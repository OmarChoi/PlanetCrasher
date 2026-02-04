using Cysharp.Threading.Tasks;

public interface ICurrencyRepository
{
    UniTaskVoid Save(CurrencySaveData saveData);
    UniTask<CurrencySaveData> Load();
}