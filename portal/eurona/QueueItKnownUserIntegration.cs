
using QueueIT.KnownUserV3.SDK;
using QueueIT.KnownUserV3.SDK.IntegrationConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;

namespace Eurona {
    public static class QueueItKnownUserIntegration {
        public static void DoValidation(Page page) {
            try {
                var customerId = "Your Queue-it customer ID";
                var secretKey = "Your 72 char secrete key as specified in Go Queue-it self-service platform";

                var queueitToken = page.Request.QueryString[KnownUser.QueueITTokenKey];
                var currentUrlWithoutQueueITToken = Regex.Replace(page.Request.Url.AbsoluteUri, @"([\?&])(" + KnownUser.QueueITTokenKey + "=[^&]*)", string.Empty, RegexOptions.IgnoreCase);
                // The currentUrlWithoutQueueITToken is used to match Triggers and as the Target url (where to return the users to)
                // It is therefor important that the currentUrlWithoutQueueITToken is exactly the url of the users browsers. So if your webserver is 
                // e.g. behind a load balancer that modifies the host name or port, reformat the currentUrlWithoutQueueITToken before proceeding
                var integrationConfig = IntegrationConfigProvider.GetCachedIntegrationConfig(customerId);

                //Verify if the user has been through the queue
                var validationResult = KnownUser.ValidateRequestByIntegrationConfig(currentUrlWithoutQueueITToken, queueitToken, integrationConfig, customerId, secretKey);

                if (validationResult.DoRedirect) {
                    //Adding no cache headers to prevent browsers to cache requests
                    page.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    page.Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
                    page.Response.Cache.SetNoStore();
                    //end
                    //Send the user to the queue - either because hash was missing or because is was invalid
                    page.Response.Redirect(validationResult.RedirectUrl, false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();

                } else {
                    //Request can continue - we remove queueittoken form querystring parameter to avoid sharing of user specific token
                    if (HttpContext.Current.Request.Url.AbsoluteUri.Contains(KnownUser.QueueITTokenKey)) {
                        page.Response.Redirect(currentUrlWithoutQueueITToken, false);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                    }
                }
            } catch (Exception ex) {
                // There was an error validating the request
                // Use your own logging framework to log the error
                // This was a configuration error, so we let the user continue
            }
        }
    }
}