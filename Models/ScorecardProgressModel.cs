﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TestingFramework.Models
{
    public class ScorecardProgressModel
    {
        public Guid ID { get; set; }
        public Guid ScorecardID { get; set; }
        public string User { get; set; }
        public int CurrentIndex { get; set; }
        public DateTime Started { get; set; }
        public DateTime Ended { get; set; }
        public string ResultsJson { get; set; }
        public string ScorecardNotes { get; set; }


        [NotMapped]
        List<ScorecardTestResultModel> _resultsList { get; set; }

        void AddOrUpdateResult(ScorecardTestResultModel result)
        {
            _resultsList = GetResults();

            var existing = _resultsList.FirstOrDefault(r => r.TestID == result.TestID);

            if (existing == null)
            {
                _resultsList.Add(result);
            }
            else
            {
                existing.Passed = result.Passed;
                existing.Notes = result.Notes;
            }

            ResultsJson = JsonConvert.SerializeObject(_resultsList);
        }

        public void AddOrUpdateResult(Guid testID, Guid categoryID, bool passed, string notes = "")
        {
            _resultsList = GetResults();

            var existing = _resultsList.FirstOrDefault(r => r.TestID == testID);

            if (existing == null)
            {
                var result = new ScorecardTestResultModel
                {
                    ProgressID = ID,
                    TestID = testID,
                    CategoryID = categoryID,
                    Passed = passed,
                };

                _resultsList.Add(result);
            }
            else
            {
                existing.Passed = passed;

                if (!string.IsNullOrEmpty(notes))
                    existing.Notes = notes;
            }

            ResultsJson = JsonConvert.SerializeObject(_resultsList);
        }

        public List<ScorecardTestResultModel> GetResults()
        {
            if (string.IsNullOrEmpty(ResultsJson))
            {
                return new List<ScorecardTestResultModel>();
            }

            return JsonConvert.DeserializeObject<List<ScorecardTestResultModel>>(ResultsJson);
        }
    }
}
