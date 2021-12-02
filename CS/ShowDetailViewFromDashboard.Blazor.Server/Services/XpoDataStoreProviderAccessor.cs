using System;
using DevExpress.ExpressApp.Xpo;

namespace ShowDetailViewFromDashboard.Blazor.Server.Services {
    public class XpoDataStoreProviderAccessor {
        public IXpoDataStoreProvider DataStoreProvider { get; set; }
    }
}
