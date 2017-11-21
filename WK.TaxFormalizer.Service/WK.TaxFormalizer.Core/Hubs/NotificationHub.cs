
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;

namespace WK.TaxFormalizer.Hubs
{

    public class NotificationHub : Hub
    {
        /// <summary>
        /// Method for subscription to get notification
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task Subscribe(int? userId)
        {
            string s = "hello";
            //await Groups.Add(Context.ConnectionId, GetGroup(accountId));
            //Clients.All.notify(1,"hi there!!");
        }

        /// <summary>
        /// Method which creates group
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public static string GetGroup(int? userId)
        {
            return "notification:" + userId;
        }
    }
}