"use strict";

globalThis.customScript = {
    showDetailViewControllers: {},
    onBeforeRender: function (dashboardControl) {
        const viewerApi = dashboardControl.findExtension("viewerApi");
        viewerApi.on("itemClick", globalThis.customScript.processItemClick.bind(dashboardControl));
    },
    registerController: function (key, controller) {
        globalThis.customScript.showDetailViewControllers[key] = controller;
    },
    unregisterController: function (key) {
        delete globalThis.customScript.showDetailViewControllers[key];
    },
    processItemClick: function (args) {
        const itemData = args.getData(),
            dataSlice = itemData.getSlice(args.getAxisPoint()),
            oidMeasure = dataSlice.getMeasures().find((measure) => measure.dataMember === 'Oid').id,
            measureValue = dataSlice.getMeasureValue(oidMeasure),
            objectId = measureValue.getValue(),
            controllerId = this.element().dataset["showdetailid"];
        globalThis.customScript.showDetailViewControllers[controllerId].invokeMethodAsync("ShowDetailView", objectId);
    }
}

if (!globalThis.xafBlazorDashboardUserScripts) {
    globalThis.xafBlazorDashboardUserScripts = [];
}
globalThis.xafBlazorDashboardUserScripts.push(globalThis.customScript);