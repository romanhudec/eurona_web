﻿
using CMS;
using QueueIT.KnownUserV3.SDK;
using QueueIT.KnownUserV3.SDK.IntegrationConfig;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;

namespace Eurona {
    public static class QueueItKnownUserIntegration {
        public static void DoValidation(Page page) {

            string traceQueueIT = ConfigurationManager.AppSettings["QueueIT:Trace"];

            try {
                var customerId = "euronabycerny";
                var secretKey = "a2032ca8-bc41-4363-a9a8-0f631e9cb3774d7a315d-42a1-4b14-8002-acff93fed702";

                var queueitToken = page.Request.QueryString[KnownUser.QueueITTokenKey];
                var currentUrlWithoutQueueITToken = Regex.Replace(page.Request.Url.AbsoluteUri, @"([\?&])(" + KnownUser.QueueITTokenKey + "=[^&]*)", string.Empty, RegexOptions.IgnoreCase);
                // The currentUrlWithoutQueueITToken is used to match Triggers and as the Target url (where to return the users to)
                // It is therefor important that the currentUrlWithoutQueueITToken is exactly the url of the users browsers. So if your webserver is 
                // e.g. behind a load balancer that modifies the host name or port, reformat the currentUrlWithoutQueueITToken before proceeding
                var integrationConfig = IntegrationConfigProvider.GetCachedIntegrationConfig(customerId);
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                var integrationConfigJson = serializer.Serialize(integrationConfig);

                //Verify if the user has been through the queue
                var validationResult = KnownUser.ValidateRequestByIntegrationConfig(currentUrlWithoutQueueITToken, queueitToken, integrationConfig, customerId, secretKey);
                var validationResultJson = serializer.Serialize(validationResult);
                string serializedData = page.Request.Url.PathAndQuery + "\r\n" + validationResultJson + "\r\n\r\nIntergartionConfig:\r\n" + integrationConfigJson;
                if (!String.IsNullOrEmpty(traceQueueIT)) {
                    QueueIT.KnownUserV3.SDK.EvenLog.WritoToEventLog(serializedData, System.Diagnostics.EventLogEntryType.Information);
                }
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