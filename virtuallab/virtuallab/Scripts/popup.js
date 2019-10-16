$(window).bind("resize", function(e) {
    window.clearTimeout(resizeTimeoutId);
    resizeTimeoutId = window.setTimeout('dowindowResize();', 500);
});

var offsettolerance = 50;
var cachedoffsetwidth = document.documentElement.offsetWidth;
var resizeTimeoutId;
function dowindowResize() {

    if (cachedoffsetwidth != document.documentElement.offsetWidth &&
        Math.abs(cachedoffsetwidth - document.documentElement.offsetWidth) > offsettolerance) {
        cachedoffsetwidth = document.documentElement.offsetWidth;
        $("#POPUPTAB").children("ul").children('li["dirty"]').attr('dirty', 'dirty');
        refreshPopupTabForResize();
    }
}

//drill down for the Popup page
//drill down works slightly different here.
//rather than opening the link in the popup,
//we add it to the current breadcrumb.
function DrillDown(context, controller, title, action, params) {

    //get the breadcrumb
    var menu = "POPUPTAB";
    var mainTabControl = $("#POPUPTAB").children("ul");

    //create a tabpage and tab for the new page
    var tabpageid = menu + "_" + controller;
    var tabid = tabpageid + "_tab";
    var tab = $("#" + tabid);
    if (tab.length == 0) {

        var newtab = "<li id='" + tabid + "' " +
            "class='POPUPTABtab' " +
            ((controller == null) ? "" : "controller='" + controller + "' ") +
            ((action == null) ? "" : "action=\"" + action + "\" ") +
            ((params == null) ? "" : "params=\"" + params + "\" ") +
            "context='" + context + "' onclick='selectPopupTabItem(event);'>" +
            title + "</li>";

        var newtabpage = "<div id='" + tabpageid + "' class='POPUPTABtabpage' status='EMPTY'></div>";

        //add the new divs to the DOM
        mainTabControl.append(newtab);
        $("#" + menu + "_TabPage").append(newtabpage)
    }
    else {
        //the action or params may have changed. update them.
        tab.attr("action", action);
        tab.attr("params", params);
    }

    //create a command to select the tab and call that command.
    selectPopupTab(context, menu, tabpageid, controller, true, action, params, false);
}

function refreshCurrentPopupTab(params) {
    var selectedTab = $("#POPUPTAB").children("ul").children('li[class$="selectedtab"]');
    var tab = selectedTab.attr("id").split('_tab')[0];
    var menu = selectedTab.parent().attr("id");
    var controller = selectedTab.attr("controller");
    var context = selectedTab.attr("context");
    var action = selectedTab.attr("action");
    if (context == null || context == '')
        context = selectedTab.parent().parent().attr('context');
    var existingparams = selectedTab.attr("params");
    if (params) {
        selectedTab.attr("params", params);
        existingparams = params;
    }
    selectPopupTab(context, menu, tab, controller, true, action, params, false);
}

function refreshPopupTabForResize() {
    var selectedTab = $("#POPUPTAB").children("ul").children('li[class$="selectedtab"]');
    var tab = selectedTab.attr("id").split('_tab')[0];
    var menu = selectedTab.parent().attr("id");
    var controller = selectedTab.attr("controller");
    var context = selectedTab.attr("context");
    var action = selectedTab.attr("action");
    if (context == null || context == '')
        context = selectedTab.parent().parent().attr('context');
    var existingparams = selectedTab.attr("params");

    if (controller == "Details" && context == "KPI")
        params = "";
    else
        params = existingparams;

    // Donnot refresh for some controller, they do not support self refresh.
    // If we are really need such behavior, add the feature to these controller.
    if (controller == "ListManagement"
        || controller == "WorkflowConfiguration"
        || controller == "ScorecardChart")
        return;

    // In details, the task id=abdd72c3d09944419f712f38705f2233 is Report page. this page needs no refresh
    if (controller == "Details") {
        if ($(".defaulttabselectedtabpage", "#POPUPTAB_TabPage").attr("taskid") == "abdd72c3d09944419f712f38705f2233")
            return;
    }
    // ignoreSelectTask = false, ONLY in refresh !
    selectPopupTab(context, menu, tab, controller, true, action, params, true);
}

//takes the last item off the breadcrumb
function popBreadcrumb(refresh) {
    //get the count of unselected tabs
    var tabs = $('#POPUPTAB .POPUPTABtab').length + $('#POPUPTAB .POPUPTABfirsttab').length;
    if (tabs == 0) {
        //this is the only tab. close the popup
        parent.ClosePopup(refresh, false);
        var closeButton = $("#ClosePopupPage");
        if (closeButton.css("visibility") == "hidden")
            closeButton.css("visibility", "visible");
    }
    else {
        //get the selected tab/tabpage
        var selectedTab = $('#POPUPTAB ul li[class$="selectedtab"]').prev();
        var tab = selectedTab.attr("id").split('_tab')[0];
        var menu = selectedTab.parent().attr("id");
        var controller = selectedTab.attr("controller");
        var context = selectedTab.attr("context");
        var action = selectedTab.attr("action");
        if (context == null || context == '')
            context = selectedTab.parent().parent().attr('context');
        var existingparams = selectedTab.attr("params");
        selectPopupTab(context, menu, tab, controller, refresh, action, existingparams, false);
       
        $('#LoadingSpinner').removeClass('topmostVisible').addClass('topmostHidden');
    }   
}


//overrid of selecttabitem() for the breadcrumb
//div structure is just a little different. and
//we don't want it to notify the client. we don't
//persist popup selected tabs.
function selectPopupTabItem(e) {
    var target = eventTarget(e);
    var tab = target.id.split('_tab')[0];
    var menu = target.parentNode.id;
    var controller = target.getAttribute('controller');
    var context = target.getAttribute('context');
    var action = target.getAttribute('action');
    var params = target.getAttribute('params');
    if (context == null || context == '')
        context = target.parentNode.parentNode.getAttribute('context');
   
    selectPopupTab(context, menu, tab, controller, false, action, params, false);
}

function selectPopupTab(context, menu, tab, controller, force, action, params, ignoreSelectTask) {
    //very similar to the normal selected tab, but it doesn't rely on task guids
    //and doesn't push selected tabs back to the server.
    var newtab = "#" + tab;
    //get the tab that used to be selected
    var oldtab = "#" + $(newtab).parent().children('div[class$="selectedtabpage"]').attr("id");

    if (oldtab != newtab) {
        var newBaseClass = $(newtab).attr("class");
        newBaseClass = newBaseClass.substring(0, newBaseClass.length - 7);
        var oldBaseClass = $(oldtab).attr("class");
        oldBaseClass = oldBaseClass.substring(0, oldBaseClass.length - 15);


        //set the old tab/tabpage to no longer be selected
        $(oldtab).attr("class", oldBaseClass + "tabpage");
        $(oldtab + "_tab").attr("class", oldBaseClass + "tab");

        //set the new tab/tabpage to be selected
        $(newtab).attr("class", newBaseClass + "selectedtabpage");
        $(newtab + "_tab").attr("class", newBaseClass + "selectedtab");
    }

    var tmpStr = oldtab.toString();
    var type = tmpStr.split('_')[1];
    if (type == "WorkflowConfiguration" || type == "DataMiner") {
        var closeButton = $("#ClosePopupPage");
        if (closeButton.css("visibility") == "hidden")
            closeButton.css("visibility", "visible");
    }

    //remove all list items after the newly selected tab button
    //remove all content divs after the the specified one.
    //this will effectively "pop" items off the breadcrumb.
    $(newtab + "_tab ~ li").remove();
    $(newtab + " ~ div").remove();

    //do we have content in the new page?
    if ($(newtab).attr('status') == 'EMPTY' || force) {
        //no. load the content via ajax.
        //the guid of the task we're loading is stored on the div as an attribute
        //when it's done, mark the tab as full. doing this as a dynamic function
        //because its easier than trying to pass all the variables around.

        var controlaction = 'DrillDown';
        if (action)
            controlaction = action;

        //construct the URL and JSON object used for the request.
        //for drilldown we allow more flexibility than normal tabs, by supporting
        //both custom actions and additional parameters.    
        var taburl = '/' + controller + '/' + controlaction + '/';
        var dataJson = "({ contextKey:'" + context + "'";
        if (params != null && params != "")
            dataJson += "," + params;
        dataJson += " })";

        var x1 = newtab.substr(1, newtab.length - 1);
        var x2 = eval(dataJson);
        if (ignoreSelectTask != null && ignoreSelectTask == true && x2.selectedTaskGuid != null && x2.selectedTaskGuid != undefined)
            x2.selectedTaskGuid = '';

        loadDiv(x1, taburl, x2, function() { $(newtab).attr('status', 'FULL'); if ($.TabChangedEvent) $.TabChangedEvent(); });
    }
    else {
        if ($.TabChangedEvent) $.TabChangedEvent();
    }
}
