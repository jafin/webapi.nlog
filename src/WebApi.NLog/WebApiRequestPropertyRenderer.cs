using System;
using System.Net.Http;
using System.Text;
using System.Web;
using NLog;
using NLog.Config;
using NLog.LayoutRenderers;

namespace WebApi.NLog
{
    /// <summary>
    /// Layout renderer for asp.net's Web API traced http request message.
    /// </summary>
    [LayoutRenderer("webapi-request-property")]
    public class WebApiRequestPropertyRenderer : LayoutRenderer
    {
        /// <summary>
        /// </summary>
        [DefaultParameter]
        public string Key { get; set; }

        protected override void Append(StringBuilder builder, LogEventInfo logEvent)
        {
            if (String.IsNullOrEmpty(Key))
            {
                return;
            }

            //Ref:http://stackoverflow.com/questions/16670329/how-to-access-the-current-httprequestmessage-object-globally?rq=1
            //Note: No Item MS_HttpRequestMessage appears to exist in Web API V1. You may be able to hack it with a actionFilter like HttpContext.Current.Items.Add("MS_HttpRequestMessage", actionContext.Request);
            var httpRequestMessage = HttpContext.Current.Items["MS_HttpRequestMessage"] as HttpRequestMessage;
            if (httpRequestMessage == null)
            {
                //what to do?
                return;
            }

            if (httpRequestMessage.Properties.ContainsKey(Key))
            {
                builder.Append(httpRequestMessage.Properties[Key]);
            }
        }
    }
}