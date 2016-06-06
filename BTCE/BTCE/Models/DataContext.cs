using System.Data.Entity;

namespace BTCE.Models
{
    public class DataContext: DbContext
    {
        public DataContext(string path)
            : base(path)
        {
            Configuration.AutoDetectChangesEnabled =
            Configuration.ValidateOnSaveEnabled = false;
        }

        public DbSet<Ticker> Tickers { get; set; }
    }
}
