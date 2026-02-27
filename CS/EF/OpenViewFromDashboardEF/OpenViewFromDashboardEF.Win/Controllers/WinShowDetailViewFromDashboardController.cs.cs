using DevExpress.DashboardCommon;
using DevExpress.DashboardCommon.ViewerData;
using DevExpress.ExpressApp.Dashboards.Win;
using DevExpress.ExpressApp;
using DevExpress.DashboardWin;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;
using OpenViewFromDashboardEF.Module.BusinessObjects;

public class WinShowDetailViewFromDashboardController : ObjectViewController<DetailView, IDashboardData> {
    private ParametrizedAction openDetailViewAction;

    protected override void OnActivated() {
        base.OnActivated();
        WinDashboardViewerViewItem dashboardViewerViewItem = View.FindItem("DashboardViewer") as WinDashboardViewerViewItem;
        if (dashboardViewerViewItem != null) {
            if (dashboardViewerViewItem.Viewer != null) {
                dashboardViewerViewItem.Viewer.DashboardItemDoubleClick += Viewer_DashboardItemDoubleClick;
            }
            dashboardViewerViewItem.ControlCreated += DashboardViewerViewItem_ControlCreated;
        }
    }

    private void DashboardViewerViewItem_ControlCreated(object sender, EventArgs e) {
        WinDashboardViewerViewItem dashboardViewerViewItem = sender as WinDashboardViewerViewItem;
        dashboardViewerViewItem.Viewer.DashboardItemDoubleClick += Viewer_DashboardItemDoubleClick;
    }
    private bool IsGridDashboardItem(Dashboard dashboard, string dashboardItemName) {
        DashboardItem dashboardItem = dashboard.Items.SingleOrDefault(item => item.ComponentName == dashboardItemName);
        return dashboardItem is GridDashboardItem;
    }
    private static string GetID(DashboardItemMouseActionEventArgs e) {
        MultiDimensionalData data = e.Data.GetSlice(e.GetAxisPoint());
        MeasureDescriptor descriptor = data.GetMeasures().SingleOrDefault(item => item.DataMember == "ID");
        MeasureValue measureValue = data.GetValue(descriptor);
        return measureValue.Value.ToString();
    }
    private void Viewer_DashboardItemDoubleClick(object sender, DashboardItemMouseActionEventArgs e) {
        Dashboard dashboard = ((DashboardViewer)sender).Dashboard;

        if (IsGridDashboardItem(dashboard, e.DashboardItemName) &&
            openDetailViewAction.Enabled && openDetailViewAction.Active) {
            openDetailViewAction.DoExecute(GetID(e));
        }
    }

    private void OpenDetailViewAction_Execute(object sender, ParametrizedActionExecuteEventArgs e) {
        IObjectSpace objectSpace = Application.CreateObjectSpace(typeof(Contact));
        Guid contactId;
        // Try to parse the parameter as a Guid
        if (Guid.TryParse(e.ParameterCurrentValue.ToString(), out contactId)) {
            Contact contact = objectSpace.FirstOrDefault<Contact>(c => c.ID == contactId);
            if (contact != null) {
                e.ShowViewParameters.CreatedView = Application.CreateDetailView(objectSpace, contact, View);
            }
        }
    }

    protected override void OnDeactivated() {
        WinDashboardViewerViewItem dashboardViewerViewItem = View.FindItem("DashboardViewer") as WinDashboardViewerViewItem;
        if (dashboardViewerViewItem != null) {
            dashboardViewerViewItem.ControlCreated -= DashboardViewerViewItem_ControlCreated;
        }
        base.OnDeactivated();
    }
    public WinShowDetailViewFromDashboardController() {
        openDetailViewAction = new ParametrizedAction(this, "Dashboard_OpenDetailView", "Dashboard", typeof(string));
        openDetailViewAction.Caption = "OpenDetailView";
        openDetailViewAction.SelectionDependencyType = SelectionDependencyType.RequireSingleObject;
        openDetailViewAction.Execute += OpenDetailViewAction_Execute;
    }
}