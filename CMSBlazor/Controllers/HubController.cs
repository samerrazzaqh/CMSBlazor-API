using System;
using CMSBlazor.Shared.ViewModels.SignalR;
using Microsoft.AspNetCore.SignalR;

namespace CMSBlazor.Controllers
{
	public class HubController : Hub
    {
        public async Task SendMessage()
        {
            await Clients.All.SendAsync("ReceiveMessage");
        }

        //public async Task SendMessage(string user, string message)
        //{
        //    await Clients.All.SendAsync("ReceiveMessage", user, message);
        //}
    }
}

