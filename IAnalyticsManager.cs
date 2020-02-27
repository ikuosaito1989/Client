using System.Collections.Generic;
using mygkrnk.Models;

namespace mygkrnk.Manager
{
    public interface IAnalyticsManager
    {
        IEnumerable<AnalyticsReport> GetAnalyticsReport();
    }
}
