namespace BTCE.Models
{
    public enum CurrencyType : byte
    {
        None    = 0,
        Btc     = 1 << 0,
        Ltc     = 1 << 1,
        Nmc     = 1 << 2,
        Nvc     = 1 << 3,
        Usd     = 1 << 4,
        Eur     = 1 << 5,
        Rur     = 1 << 6,
        Ppc     = 1 << 7
    }
}
