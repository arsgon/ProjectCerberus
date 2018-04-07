using Microsoft.AspNet.WebHooks;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ProjectCerberus
{
    public class GenericWebhookHandler : WebHookHandler
    {
        private static String WebhookData;
        public override Task ExecuteAsync(string receiver, WebHookHandlerContext context)
        {

            // Console.WriteLine("Received notification with payload:");
            SetWebHookData(context);
            return Task.FromResult(true);
        }


        private String SetWebHookData(WebHookHandlerContext context)
        {
            JObject contextData = context.GetDataOrDefault<JObject>();
            WebhookData = contextData.ToString();
            return null;
        }

        public static String GetWebHookData()
        {
            if (WebhookData != null)
                return WebhookData;

            else
                return "OOOPS MAL AE";
        }

    }
}

 