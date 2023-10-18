﻿using DevExpress.ExpressApp.Blazor;
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using Microsoft.JSInterop;
using static System.Net.Mime.MediaTypeNames;
using dxTestSolution.Module.BusinessObjects;

namespace OpenViewFromDashboard.Blazor.Server.Controllers;
public class BlazorShowDetailViewFromDashboardController : ObjectViewController<DetailView, IDashboardData> {
    private IJSRuntime JSRuntime { get; set; }
    private DotNetObjectReference<BlazorShowDetailViewFromDashboardController> controllerReference;
    public BlazorShowDetailViewFromDashboardController() {
        controllerReference = DotNetObjectReference.Create(this);
    }
    protected override void OnActivated() {
        base.OnActivated();
        var application = (BlazorApplication)Application;
        JSRuntime = application.ServiceProvider.GetRequiredService<IJSRuntime>();
        _ = JSRuntime.InvokeVoidAsync("customScript.registerController", controllerReference).Preserve();
    }
    protected override void Dispose(bool disposing) {
        base.Dispose(disposing);
        controllerReference.Dispose();
    }
    [JSInvokable]
    public void ShowDetailView(string oidString) {
        if (!Guid.TryParse(oidString, out var oid)) {
            return;
        }
        var objectSpace = Application.CreateObjectSpace(typeof(Contact));
        var item = objectSpace.FirstOrDefault<Contact>(c => c.ID == oid);
        if (item is not null) {
            var detailView = Application.CreateDetailView(objectSpace, item);
            Frame.SetView(detailView);
        } else {
            objectSpace.Dispose();
        }
    }
}

