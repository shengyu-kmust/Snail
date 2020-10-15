using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Snail.Web.Hubs
{
    public class DefaultHub:Hub
    {
        public async Task Send(string eventName, object eventValue)
        {
            await Clients.All.SendAsync(eventName, eventValue);
        }
    }
}
