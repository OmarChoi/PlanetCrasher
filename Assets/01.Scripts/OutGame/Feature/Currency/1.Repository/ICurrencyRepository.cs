public interface ICurrencyRepository
{
    public void Save(CurrencySaveData saveData);
    public CurrencySaveData Load();
}