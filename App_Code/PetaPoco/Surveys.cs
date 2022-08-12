using System;
using System.Collections.Generic;
using RevistaUFO.Models;
using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.DatabaseAnnotations;

namespace RevistaUFO.PetaPoco
{
    public class Surveys
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Question { get; set; }

        public int NodeId { get; set; }

        public IEnumerable<SurveyOptions> Options { get; set; }
        public IEnumerable<SurveyVotes> Votes { get; set; }

        public int VoteCount { get; set; }

        public double Rated { get; set; }
    }

    public class SurveyOptions
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public int Order { get; set; }
        public int Votes { get; set; }
        public bool IsCorrectAnswer { get; set; }
        public double Percentage { get; set; }
    }

    [TableName("SurveyVotes")]
    [PrimaryKey("Id", autoIncrement = true)]
    public class SurveyVotes
    {
        [PrimaryKeyColumn(AutoIncrement = true)]
        public int Id { get; set; }
        public string IPAddress { get; set; }

        public int NodeId { get; set; }

        public int OptionId { get; set; }

        public DateTime VoteDate { get; set; }
    }
}