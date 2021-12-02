"use strict";

globalThis.customScript = {
    showDetailViewController: null,
    onBeforeRender: function (dashboardControl) {
        const viewerApi = dashboardControl.findExtension("viewerApi");
        viewerApi.on("itemClick", globalThis.customScript.processItemClick);
    },
    registerController: function (controller) {
        globalThis.customScript.showDetailViewController = controller;
    },
    processItemClick: function (args) {
        const itemData = args.getData(),
            dataSlice = itemData.getSlice(args.getAxisPoint()),
            oidMeasure = dataSlice.getMeasures().find((measure) => measure.dataMember === 'Oid').id,
            measureValue = dataSlice.getMeasureValue(oidMeasure),
            objectId = measureValue.getValue();
        globalThis.customScript.showDetailViewController.invokeMethodAsync("ShowDetailView", objectId);
    }
}

if (!globalThis.xafBlazorDashboardUserScripts) {
    globalThis.xafBlazorDashboardUserScripts = [];
}
globalThis.xafBlazorDashboardUserScripts.push(globalThis.customScript);