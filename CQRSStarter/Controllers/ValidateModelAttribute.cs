using Newtonsoft.Json.Serialization;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace CQRSStarter.API.Controllers
{
    public class ValidateModelAttribute: ActionFilterAttribute
    {
        private JsonMediaTypeFormatter _camelCasingFormatter = new JsonMediaTypeFormatter();

        public ValidateModelAttribute()
        {
            _camelCasingFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            if(actionExecutedContext.Response.Content is ObjectContent content)
            {
                if(content.Formatter is JsonMediaTypeFormatter)
                {
                    actionExecutedContext.Response.Content = new ObjectContent(content.ObjectType, content.Value, _camelCasingFormatter);
                }
            }
        }

        public override void OnActionExecuting(HttpActionContext actionExecutedContext)
        {

            if(actionExecutedContext.ModelState.IsValid == false)
            {
                actionExecutedContext.Response = actionExecutedContext.Request.CreateErrorResponse(
                System.Net.HttpStatusCode.BadRequest, actionExecutedContext.ModelState);
            }

            if(actionExecutedContext.Response.Content is ObjectContent content)
            {
                if(content.Formatter is JsonMediaTypeFormatter)
                {
                    foreach(var i in content.Value as dynamic)
                    {
                         
                    }
                    actionExecutedContext.Response.Content = new ObjectContent(content.ObjectType, content.Value, _camelCasingFormatter);
                }
            }

        }

    }
}
