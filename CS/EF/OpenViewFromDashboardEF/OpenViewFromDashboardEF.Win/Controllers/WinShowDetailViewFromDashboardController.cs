using DevExpress.DashboardCommon;
using DevExpress.DashboardCommon.ViewerData;
using DevExpress.DashboardWin;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Dashboards.Win;
using DevExpress.Persistent.Base;
using OpenViewFromDashboardEF.Module.BusinessObjects;

public class WinShowDetailViewFromDashboardController :ObjectViewController<DetailView, IDashboardData> {

    protected override void OnActivated() {
        base.OnActivated();
        View.CustomizeViewItemControl<WinDashboardViewerViewItem>(this, item => {
            item.Viewer.DashboardItemDoubleClick += Viewer_DashboardItemDoubleClick;
        });
    }
    private bool IsGridDashboardItem(Dashboard dashboard, string dashboardItemName) {
        DashboardItem dashboardItem = dashboard.Items.SingleOrDefault(item => item.ComponentName == dashboardItemName);
        return dashboardItem is GridDashboardItem;
    }
    private static Guid GetID(DashboardItemMouseActionEventArgs e) {
        MultiDimensionalData data = e.Data.GetSlice(e.GetAxisPoint());
        MeasureDescriptor descriptor = data.GetMeasures().SingleOrDefault(item => item.DataMember == "ID");
        MeasureValue measureValue = data.GetValue(descriptor);
        return (Guid)measureValue.Value;
    }
    private void Viewer_DashboardItemDoubleClick(object sender, DashboardItemMouseActionEventArgs e) {
        Dashboard dashboard = ((DashboardViewer)sender).Dashboard;
        if(IsGridDashboardItem(dashboard, e.DashboardItemName)) {
            OpenDetailView(GetID(e));
        }
    }

    private void OpenDetailView(Guid contactId) {
        IObjectSpace objectSpace = Application.CreateObjectSpace<Contact>();
        Contact contact = objectSpace.FirstOrDefault<Contact>(c => c.ID == contactId);
        if(contact != null) {
            DetailView detailView = Application.CreateDetailView(objectSpace, contact, View);
            Application.ShowViewStrategy.ShowViewFromCommonView(detailView);
        }
    }

}