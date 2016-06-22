using System.Data.Entity;
using System.Configuration;

namespace BTCE.Models
{
    public class DataContext: DbContext
    {
        public DataContext()
            : this(ConfigurationManager.ConnectionStrings["AnoviDataServer"].ConnectionString)
        {
            
        }
        public DataContext(string path)
            : base(path)
        {
        }

        public DbSet<Ticker> Tickers { get; set; }
    }
}
