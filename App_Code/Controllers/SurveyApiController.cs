using Newtonsoft.Json;
using RevistaUFO.PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Umbraco.Core.Models;
using Umbraco.Core.Persistence;
using Umbraco.Core.Services;
using Umbraco.Web;
using Umbraco.Web.WebApi;

/// <summary>
/// Summary description for SurveyApiController
/// </summary>
public class SurveyApiController : UmbracoApiController
{
    UmbracoHelper umbracoHelper = new UmbracoHelper(UmbracoContext.Current);
    private IContentService service = null;
    static Database db = null;

    public SurveyApiController()
    {
        db = ApplicationContext.DatabaseContext.Database;
        service = ApplicationContext.Services.ContentService;
    }
    private Surveys GetSurvey(int nodeId)
    {
        var surveyNode = umbracoHelper.TypedContent(nodeId);
        var survey = new Surveys { NodeId = surveyNode.Id, Name = surveyNode.Name, Question = surveyNode.GetPropertyValue<string>("Survey_title") };
        survey.Options = JsonConvert.DeserializeObject<IEnumerable<SurveyOptions>>(surveyNode.GetPropertyValue<string>("Survey_Options"));
        //survey.Votes = db.Query<SurveyVotes>("where NodeId=" + nodeId);
        var votes = db.Query<SurveyVotes>("where NodeId=" + nodeId);
        survey.VoteCount = votes.Count();
        foreach (var option in survey.Options)
        {
            option.Votes = votes.Where(x => x.OptionId == option.Id).Count();
            if (survey.VoteCount > 0)
                option.Percentage = ((double)option.Votes / survey.VoteCount) * 100;
            else option.Percentage = 0;
        }
        return survey;
    }
    [HttpGet]
    public IHttpActionResult GetVotes(int nodeId)
    {
        var survey = GetSurvey(nodeId);
        return Ok(survey);
    }
    [HttpPost]
    public IHttpActionResult Vote(int nodeId, string ip, int optionId)
    {
        var vote = new SurveyVotes
        {
            IPAddress = ip,
            OptionId = optionId,
            NodeId = nodeId,
            VoteDate = DateTime.Now
        };
        db.Insert(vote);
        var survey = GetSurvey(nodeId);

        return Ok(new { VotedOn = survey.Question, YourIP = ip, Results = survey });
    }
    [HttpGet]
    public IHttpActionResult VotesForSurvey(int nodeId)
    {
        return Ok(GetSurvey(nodeId));
    }
    [HttpGet]
    public IHttpActionResult AlreadyVoted(int nodeId, string ip)
    {
        bool voted = db.ExecuteScalar<int>(string.Format("SELECT COUNT([IPAddress]) FROM [SurveyVotes] WHERE [nodeid] = {0} AND [IPAddress] = \'{1}\'", nodeId, ip)) > 0 ? true : false;
        return Ok(voted);
    }
}