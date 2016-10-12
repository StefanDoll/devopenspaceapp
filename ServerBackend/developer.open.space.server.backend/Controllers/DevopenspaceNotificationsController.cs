﻿using developer.open.space.server.backend.Models;
using Microsoft.Azure.Mobile.Server.Config;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace developer.open.space.server.backend.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [MobileAppController]
    public class DevopenspaceNotificationsController : ApiController
    {

        public async Task<HttpResponseMessage> Post(string pns, string password, [FromBody]string message)
        {

            Microsoft.Azure.NotificationHubs.NotificationOutcome outcome = null;
            HttpStatusCode ret = HttpStatusCode.InternalServerError;

            if (string.IsNullOrWhiteSpace(message) || password != ConfigurationManager.AppSettings["NotificationsPassword"])
                return Request.CreateResponse(ret);


            switch (pns.ToLower())
            {
                case "wns":
                    // Windows 8.1 / Windows Phone 8.1
                    var toast = @"<toast><visual><binding template=""ToastText01""><text id=""1"">" +
                                message + "</text></binding></visual></toast>";
                    outcome = await DevopenspaceNotifications.Instance.Hub.SendWindowsNativeNotificationAsync(toast);
                    break;
                case "apns":
                    // iOS
                    var alert = "{\"aps\":{\"alert\":\"" + message + "\"}}";
                    outcome = await DevopenspaceNotifications.Instance.Hub.SendAppleNativeNotificationAsync(alert);
                    break;
                case "gcm":
                    // Android
                    var notif = "{ \"data\" : {\"message\":\"" + message + "\"}}";
                    outcome = await DevopenspaceNotifications.Instance.Hub.SendGcmNativeNotificationAsync(notif);
                    break;
            }

            if (outcome != null)
            {
                if (!((outcome.State == Microsoft.Azure.NotificationHubs.NotificationOutcomeState.Abandoned) ||
                    (outcome.State == Microsoft.Azure.NotificationHubs.NotificationOutcomeState.Unknown)))
                {
                    ret = HttpStatusCode.OK;
                }
            }

            return Request.CreateResponse(ret);
        }
    }
}