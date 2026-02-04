public class FirebaseCurrencyRepository : ICurrencyRepository
{
    public void Save(CurrencySaveData saveData)
    {
        
    }
    
    public CurrencySaveData Load()
    {
        // todo. Firebase 연동
        return CurrencySaveData.Default;
    }
}