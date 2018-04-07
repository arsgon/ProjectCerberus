using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ProjectCerberus.Models;

namespace ProjectCerberus.Controllers
{
    public class WebhooksController : Controller
    {
        // GET: Webhooks
        private String PAGINA_TOKEN = "EAAMEhHI3UdsBAICGGBOI4iRWZATNyYAhZCSrUJ1CBZCoMrZApvbbRrbidlcvpf8VOz94ZBwwxDZAwru8nA71lFsF5L1eBOTrJSVHoLNtIEZAtSJi06CzZBzPykPN35Ft5KxryxZACtaIpIwrzjft5WWESJFSBbpFRLyZBuwPH4XRtPTQZDZD";
        private String URL_ARGS = "&debug=all";
        private String HUB_TOKEN = "abajur123";
        private String URL = "https://graph.facebook.com/v2.6/me/messages?access_token=";
        public ActionResult Index()
        {
            return View();
        }

        //Metodo utilizado para a primeira validacao do webhook realizada pelo Facebook.
        public ActionResult Receive()
        {
            var query = Request.QueryString;

            //Validacao de token enviado pelo Facebook para inscrever o webhook da pagina 
            if (query["hub.mode"] == "subscribe" && query["hub.verify_token"] == HUB_TOKEN)
            {
                //string type = Request.QueryString["type"];
                var retVal = query["hub.challenge"];
                return Json(int.Parse(retVal), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return HttpNotFound();
            }
        }

        private string EnviarDados(string url, string data)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = "application/json";
            request.Method = "POST";
           
                using (var requestWriter = new StreamWriter(request.GetRequestStream()))
                {
                    requestWriter.Write(data);
                }

                var response = (HttpWebResponse)request.GetResponse();
                if (response == null)
                    throw new InvalidOperationException("GetResponse retornou uma resposta invalida.");

                using (var sr = new StreamReader(response.GetResponseStream()))
                {
                    return sr.ReadToEnd();
                }
            
        }

        [ActionName("Receive")]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ReceberMensagem(FacebookChatbotModel.BotRequest data)
        {
            Task.Factory.StartNew(() =>
            {
                foreach (var entry in data.entry)
                {
                    foreach (var message in entry.messaging)
                    {
                        if (string.IsNullOrWhiteSpace(message?.message?.text))
                        continue;

                        // var json = "You said: " + message.message.text; 
                        //var json = $@"{{""recipient"":{{ ""id"":""{message.sender.id}""}}, ""message"":{{ ""attachment"":{{ ""type"":""template"", ""payload"":{{ ""template_type"":""button"", ""text"":""What do you want to do next ? "", ""buttons"":[ {{ ""type"":""web_url"", ""url"":""https://www.messenger.com"", ""title"":""Visit Messenger"" }} ] }} }} }}";;

                        var json = $@"

                        {{
                        ""recipient"":{{ 
                        ""id"": {message.sender.id} }}, 
                        ""message"":
                        {{
                                                ""attachment"":
                            {{
                                                    ""type"":""template"", ""payload"":
                                {{
                                                        ""template_type"":""button"", ""text"":""What do you want to do next ?"", 
                                ""buttons"":
                                [
                                    {{ ""type"":""web_url"", 
                                       ""url"":""https://www.messenger.com"", 
                                       ""title"":""Visit Messenger"" }} 
                                ]
                                }}
                            }}
                        }}
                        }}";

                        Console.Write(EnviarDados(URL + PAGINA_TOKEN, json));
                    }
                }
            });

            return new HttpStatusCodeResult(System.Net.HttpStatusCode.OK);
        }
    }
}
