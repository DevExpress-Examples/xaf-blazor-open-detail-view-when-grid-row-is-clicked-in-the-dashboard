using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Dashboards.Blazor.Components;
using DevExpress.Persistent.Base;
using OpenViewFromDashboard.Module.BusinessObjects;
using Microsoft.JSInterop;

namespace OpenViewFromDashboard.Blazor.Server.Controllers;
public class BlazorShowDetailViewFromDashboardController : ObjectViewController<DetailView, IDashboardData> {
    private string clientId = Guid.NewGuid().ToString();
    private DotNetObjectReference<BlazorShowDetailViewFromDashboardController> controllerReference;
    public BlazorShowDetailViewFromDashboardController() {
        controllerReference = DotNetObjectReference.Create(this);
    }
    protected override void OnActivated() {
        base.OnActivated();
        Application.ServiceProvider.GetRequiredService<IJSRuntime>().InvokeVoidAsync("customScript.registerController", clientId, controllerReference).Preserve();
        View.CustomizeViewItemControl<BlazorDashboardViewerViewItem>(this, CustomizeDashboardViewerViewItem);
    }
    private void CustomizeDashboardViewerViewItem(BlazorDashboardViewerViewItem dashboardViewerViewItem) {
        dashboardViewerViewItem.ComponentModel.SetAttribute("data-showdetailid", clientId);
    }
    protected override void OnDeactivated() {
        Application.ServiceProvider.GetRequiredService<IJSRuntime>().InvokeVoidAsync("customScript.unregisterController", clientId).Preserve();
        base.OnDeactivated();
    }
    protected override void Dispose(bool disposing) {
        base.Dispose(disposing);
        controllerReference.Dispose();
    }
    [JSInvokable]
    public void ShowDetailView(string oidString) {
        if(!Guid.TryParse(oidString, out var oid)) {
            return;
        }
        var objectSpace = Application.CreateObjectSpace(typeof(Contact));
        var item = objectSpace.FirstOrDefault<Contact>(c => c.Oid == oid);
        if(item is not null) {
            var detailView = Application.CreateDetailView(objectSpace, item, true);
            Application.ShowViewStrategy.ShowViewFromCommonView(detailView);
        } else {
            objectSpace.Dispose();
        }
    }
}
