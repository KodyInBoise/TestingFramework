using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TestingFramework.Models
{
    public class ScorecardResultModel
    {
        public Guid ID { get; set; }
        public Guid ScorecardID { get; set; }
        public string User { get; set; }
        public string ScorecardNotes { get; set; }
        public string ResultsJson { get; set; }
        public int TotalTestCount { get; set; }
        public DateTime CompletedTimestamp { get; set; }
        public DateTime StartedTimestamp { get; set; }

        public static ScorecardResultModel FromProgress(ScorecardProgressModel progress)
        {
            return new ScorecardResultModel
            {
                ScorecardID = progress.ScorecardID,
                User = progress.User,
                ResultsJson = progress.ResultsJson,
                CompletedTimestamp = DateTime.Now,
                ScorecardNotes = progress.ScorecardNotes,
                StartedTimestamp = progress.Started,
            };
        }

        public List<ScorecardTestResultModel> GetTestResults()
        {
            if (string.IsNullOrEmpty(ResultsJson))
            {
                return new List<ScorecardTestResultModel>();
            }

            return JsonConvert.DeserializeObject<List<ScorecardTestResultModel>>(ResultsJson);
        }
    }
}
