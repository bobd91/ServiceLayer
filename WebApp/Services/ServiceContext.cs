using System.Threading.Tasks;
using WebApp.Data;

namespace WebApp.Services
{
    public class ServiceContext : IServiceContext
    {
        public ServiceContext(DatabaseContext context)
        {
            Inbox = new InboxService(context);
        }

        public IInboxService Inbox { get; set; }

        public async ValueTask DisposeAsync()
        {
            await Inbox.DisposeAsync();
        }
    }
}
