using System.IO;
using System.Collections.Generic;
using System.Linq;
using Google.Apis.AnalyticsReporting.v4;
using Google.Apis.AnalyticsReporting.v4.Data;
using Google.Apis.Auth.OAuth2;
using mygkrnk.Models;

namespace mygkrnk.Manager
{
    public class AnalyticsManager : IAnalyticsManager
    {
        GoogleCredential _credential;
        string _viewId;
        public AnalyticsManager(string path, string viewId)
        {
            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                _credential = GoogleCredential.FromStream(stream).CreateScoped(AnalyticsReportingService.Scope.AnalyticsReadonly);
            }
            _viewId = viewId;
        }

        public IEnumerable<AnalyticsReport> GetAnalyticsReport()
        {
            var service = new AnalyticsReportingService(new AnalyticsReportingService.Initializer()
            {
                HttpClientInitializer = _credential,
                ApplicationName = "app",
            });

            var request = new GetReportsRequest
            {
                ReportRequests = new[] {
                    new ReportRequest
                    {
                        ViewId = _viewId,
                        Metrics = new[] { new Metric { Expression = "ga:pageviews" } },
                        Dimensions = new[] { new Dimension { Name = "ga:pagePath" } },
                        DateRanges = new[] { new DateRange { StartDate = "2018-04-01", EndDate = "today" } },
                        OrderBys = new [] { new OrderBy { FieldName = "ga:pageviews", SortOrder = "DESCENDING" } }
                    }
                }
            };

            var batchRequest = service.Reports.BatchGet(request);
            var response = batchRequest.Execute();
            return response.Reports.First().Data.Rows.Select(x => new AnalyticsReport() { Dimensions = x.Dimensions.First(), Views = x.Metrics.First().Values.First() });
        }
    }
}