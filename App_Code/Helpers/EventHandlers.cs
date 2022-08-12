using System.Collections.Generic;
using System.Linq;
using Umbraco.Core;
using Umbraco.Core.Events;
using Umbraco.Core.Logging;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Core.Persistence;
using RevistaUFO.PetaPoco;
using Newtonsoft.Json;

namespace RevistaUFO.Helpers
{
    public class EventHandlers : ApplicationEventHandler
    {
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            var logger = LoggerResolver.Current.Logger;
            var dbContext = ApplicationContext.Current.DatabaseContext;
            var db = new DatabaseSchemaHelper(dbContext.Database, logger, dbContext.SqlSyntax);
            
            if (!db.TableExist("EventRegistrations"))
            {
                db.CreateTable<EventRegistration>(false);
					 //dbContext.Database.Execute("ALTER TABLE [table] ALTER COLUMN [column] NVARCHAR(MAX)");
            }

            if (!db.TableExist("EventRegistrations"))
            {
                db.CreateTable<EventRegistration>(false);
            }

            if (!db.TableExist("EventRegistrationUser"))
            {
                db.CreateTable<EventRegistrationUser>(false);
            }

            if (!db.TableExist("EventRegistrationUser"))
            {
                db.CreateTable<EventRegistrationUser>(false);
            }

            ///Surveys
            if (!db.TableExist("SurveyVotes"))
            {
                db.CreateTable<SurveyVotes>(false);
            }

            //Analytics
            if (!db.TableExist("Analytics"))
            {
                db.CreateTable<Analytics>(false);
                dbContext.Database.Execute("ALTER TABLE [Analytics] ALTER COLUMN [Browser] NVARCHAR(MAX)");
            }
        }
    }
}