using System;

namespace WebApp.Services
{
    public interface IServiceContext : IAsyncDisposable
    {

        public IInboxService Inbox { get; set; }
        
    }
}
