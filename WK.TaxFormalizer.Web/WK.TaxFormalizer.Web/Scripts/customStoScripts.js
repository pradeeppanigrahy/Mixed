//Placeholders for script or functions used in STO App. Kunal Deshmukh on 7th November 2014

function CurrentVMenuItem(id) {
    $('#' + id).addClass("active");
    $('#' + id).removeAttr('href');

}
var defaultCompany;
var defaultLfe;
var isCompanyDropdownChanged = false;
var isReloadPage = true;
//var firstLoad = true;

// Code block which prompts user to save before leaving the page without appropriate action
(function ($) {
    isAnyUnSavedData = false;
    isTransactionPage = false;
    var unSavedDataWarningMessage = 'You have unsaved data. Do you want to continue without saving ?';

    $(window).bind('beforeunload', function () {
        var promise=updateUserAccessMatrix();
        if (isTransactionPage && !stoSaas.flags.isLogOffClicked)
        {
            saveTransactionGridSettings();
            isTransactionPage = false;
        }
        if (isAnyUnSavedData) {
            return unSavedDataWarningMessage;
        }
    });

    window.onload = function () {
        keepOpenCascadeDropDownListOnSelection();
        $(document).on('click', '.k-grid-edit,.k-grid-add', function () {
            $(".k-grid-cancel").attr("title", "Cancel");
            $(".k-grid-update").attr("title", "Update");
            //Set includeAnyUnSavedDataCheck=true to override below logic
            if (typeof includeAnyUnSavedDataCheck == "undefined" || includeAnyUnSavedDataCheck == null || includeAnyUnSavedDataCheck) {
                isAnyUnSavedData = true;
                $(".k-window-actions").click(function () {
                    isAnyUnSavedData = false;
                })
            //    disableKendoGridEdit();
            //    disableKendoGridNew();
            //    disableKendoGridDelete();
            //    disableKendoGridPaging();
            //    DisableGridFilterRows();
            //    DisableKendoGridExpand();
            //    DisableKendoGridSelectButton();
            //    hideCustomError();
            }
            keepOpenCascadeDropDownListOnSelection();

            //$(".k-grid-cancel,.k-grid-update").on("click", function () {
            //    isAnyUnSavedData = false;
            //    //enableKendoGridEdit();
            //    //enableKendoGridNew();
            //    //enableKendoGridDelete();
            //    //enableKendoGridPaging();
            //    //EnableGridFilterRows();
            //    //EnableKendoGridExpand();
            //    //EnableKendoGridSelectButton();
            //    hideCustomError();
            //});
            DisableGridSortingInEditModes();
        });
        DisplayKendoTooltip();
    };
})(jQuery);

//Include all cascade dropdown ids here which you want to keep open on value selection
listOfCascadeDropdownIds = "State;ItemId;DdlBRLFE;DdlBRCompany;RoleLevel;DDlLocationState;DdlLocationCounty;DDlAddressState;DdlAddressCounty;ShipTo_State;ShipFrom_State;OrderApproval_State;OrderPlacement_State;LineItemShipTo_State;LineItemShipFrom_State;LineItemOrderApproval_State;LineItemOrderPlacement_State;LineItemItemId;SitusInformationModelFOB;SitusInformationModelMailOrder;SitusInformationModelDeliveryBy;SitusInformationModelSolicitedOutside;SitusInformationModelCanRejectDelivery;SitusInformationModelShipFromPOB;BusinessRules;CompanyAndLFE;ConsumerUseTax;CustomerExemptions;Customers;CustomExemptions;CustomGroupItemsOrCustomTaxes;DatabaseUpdates;Locations;LoginSetup;Lookup;Modules;Nexus;Procurement;Reports;SalesTaxHolidays;SecurityLocks;SecurityLog;StockKeepingUnits;STROLReports;SubscriptionUpdates;TansactionManagement;TaxOverrides;TaxRateOverrides;UsersAndRoles;DdlNexusCompany;CompanySurrogate;LfeSurrogate;TaxLiabilityLookupGroupId;TaxLiabilityLookupItemId;GroupId;StateProvince;TaxType;TaxCategory;SystemItemId;TaxTypeId;TaxCategoryId;StateProvinceCode;ItemCode;GroupCode;RuleType;CountryCode;ConfigurationSettings";

//Method to help keeping cascade kendo dropdownlist open on selection
var keepOpenCascadeDropDownListOnSelection = function () {
    $("[data-role=dropdownlist]").each(function () {
        var control = $(this).data("kendoDropDownList");
        var isKeyDown = false;
        control.bind("close", function (e) {
            if (isKeyDown)
                e.preventDefault();
            isKeyDown = false;
        })

        control.bind("select", function (e) {
            isKeyDown = false;
        })

        control.bind("open", function (e) {
            if (isKeyDown)
                e.preventDefault();
            isKeyDown = false;
        })

        control.wrapper.on("keydown", function (e) {
            isKeyDown = true;
            //firstLoad = false;
            var arrOfCascadeDropdownIds = listOfCascadeDropdownIds.split(";");
            if (arrOfCascadeDropdownIds.indexOf(e.currentTarget.lastChild.id) > -1) {
                isKeyDown = true;
            }
            else {
                e.stopImmediatePropagation();
            }
        });
    });
}

function EnableGridOperations(gridId) {
    if (gridId != undefined && gridId != '') {
        gridId = "#" + gridId;
        enableKendoGridEdit(gridId);
        enableKendoGridNew(gridId);
        enableKendoGridDelete(gridId);
        enableKendoGridPaging(gridId);
        EnableGridFilterRows(gridId);
        EnableKendoGridExpand(gridId);
        EnableKendoGridSelectButton(gridId);
        EnableGridSorting(gridId);
    }
    else {
        enableKendoGridEdit();
        enableKendoGridNew();
        enableKendoGridDelete();
        enableKendoGridPaging();
        EnableGridFilterRows();
        EnableKendoGridExpand();
        EnableKendoGridSelectButton();
        EnableGridSorting();
    }
}

function DisableGridOperations(gridId) {
    if (gridId != undefined && gridId != '') {
        gridId = "#" + gridId;
        disableKendoGridEdit(gridId);
        disableKendoGridNew(gridId);
        disableKendoGridDelete(gridId);
        disableKendoGridPaging(gridId);
        DisableGridFilterRows(gridId);
        DisableKendoGridExpand(gridId);
        DisableKendoGridSelectButton(gridId);
        DisableGridSorting(gridId);
    }
    else {
        disableKendoGridEdit();
        disableKendoGridNew();
        disableKendoGridDelete();
        disableKendoGridPaging();
        DisableGridFilterRows();
        DisableKendoGridExpand();
        DisableKendoGridSelectButton();
        DisableGridSorting();
    }
}

//Method to disable kendo grid paging
function disableKendoGridPaging(context) {
    $(".k-pager-wrap .k-pager-numbers .k-link,.k-pager-nav,.k-pager-nav.k-pager-last", context).addClass("k-state-disabled");
}

//Method to enable kendo grid paging
function enableKendoGridPaging(context) {
    if ($("ul.k-pager-numbers .k-state-selected", context).parent().is(":first-child"))
    {        
        $(".k-pager-nav.k-pager-last", context).removeClass("k-state-disabled");
        $(".k-pager-nav .k-i-arrow-e", context).parent().removeClass("k-state-disabled");
    }
    else if ($("ul.k-pager-numbers .k-state-selected", context).parent().is(":last-child"))
    {
        $(".k-pager-nav.k-pager-first", context).removeClass("k-state-disabled");
        $(".k-pager-nav .k-i-arrow-w", context).parent().removeClass("k-state-disabled");
    }
    else
    {
        $(".k-pager-nav,.k-pager-nav.k-pager-last", context).removeClass("k-state-disabled");
    }
    $(".k-pager-wrap .k-pager-numbers .k-link", context).removeClass("k-state-disabled");
}

//Function disables kendo grid edit
function disableKendoGridEdit(context) {
    $(".k-grid-edit", context).css({ opacity: 0.3, cursor: 'default' }).click(function (e) { e.stopImmediatePropagation(); return false; });
}

//Function disables new button on kendo grid
function disableKendoGridNew(context) {
    $(".k-grid-toolbar .k-grid-add", context).css({ opacity: 0.3, cursor: 'default' }).click(function (e) { e.stopImmediatePropagation(); return false; });
}

//Function disables delete button on kendo grid
function disableKendoGridDelete(context) {
    $(".k-grid-delete,.k-grid-remove", context).css({ opacity: 0.3, cursor: 'default' }).click(function (e) { e.stopImmediatePropagation(); return false; });
}

//Function disables expand button on kendo grid
function DisableKendoGridExpand(context) {
    $(".k-hierarchy-cell a", context).css({ opacity: 0.3, cursor: 'default' }).click(function (e) { e.stopImmediatePropagation(); return false; });
}

//Function enables kendo grid edit
function enableKendoGridEdit(context) {
    $(".k-grid-edit", context).removeAttr("style").unbind();
}

//Function enables new button on kendo grid
function enableKendoGridNew(context) {
    $(".k-grid-toolbar .k-grid-add", context).removeAttr("style").unbind();
}

//Function enables delete button on kendo grid
function enableKendoGridDelete(context) {
    $(".k-grid-delete,.k-grid-remove", context).removeAttr("style").unbind();
}

//Function enables expand button on kendo grid
function EnableKendoGridExpand(context) {
    $(".k-hierarchy-cell a", context).removeAttr("style").unbind();
}

//Function enables select button on kendo grid
function EnableKendoGridSelectButton(context) {
    $(".k-grid-Select", context).removeAttr("style").unbind();
}

//Function disables select button on kendo grid
function DisableKendoGridSelectButton(context) {
    $(".k-grid-Select", context).css({ opacity: 0.3, cursor: 'default' }).click(function (e) { e.stopImmediatePropagation(); return false; });
}

//Function hides custom error 
function hideCustomError() {
    $("#customError").hide();
}

//Function hides custom error 
function hideCustomError(ID) {
    $("#"+ID).hide();
}

//Function shows custom error 
function showCustomError(ID,args, timeout) {
    var customError = $("#" + ID);
    customError.css("display", "block");
    customError.addClass("alert alert-danger");
    customError.html(args.errors);
    scrollTop();
    setTimeout(function () { customError.slideUp('slow'); }, timeout);
}

//Function shows custom error , without scroll to top
function showCustomErrorNoScrollTop(ID, args, timeout) {
    var customError = $("#" + ID);
    customError.css("display", "block");
    customError.addClass("alert alert-danger");
    customError.html(args.errors);
    setTimeout(function () { customError.slideUp('slow'); }, timeout);
}

function scrollTop()
{
    $(window).scrollTop(0);
}

function scrollBottom()
{
    //window.scrollTo(0, document.body.scrollHeight);
    //$(window).scrollTop(100);
    var $target = $('html,body');
    $target.animate({ scrollTop: $target.height() }, 1000);
}



function showCustomErrorWithNoTimeout(ID, args) {
    var customError = $("#" + ID);
    customError.css("display", "block");
    customError.addClass("alert alert-danger");
    customError.html(args.errors);
    scrollTop();
}

//Method to show toster message
function showToasterMessage(Id, message, timeout) {
    var divSuccessGlobal = $('#' + Id);
    divSuccessGlobal.children("p").html(message).parent().slideDown('slow');
    divSuccessGlobal.children("a").click(function () { divSuccessGlobal.slideUp('slow'); });
    if (timeout > 0) {
        setTimeout(function () { divSuccessGlobal.slideUp('slow'); }, timeout);
    }
    else
    {
        setTimeout(function () { divSuccessGlobal.slideUp('slow'); }, 10000);
    }
}


//Method checks if datasource of kendo grid has changed
function doesDataSourceHaveChanges(dataSource) {
    var dirty = false;

    $.each(dataSource._data, function () {
        if (this.dirty == true) {
            dirty = true;
        }
    });

    if (dataSource._destroyed.length > 0) {
        dirty = true;
    }
    return dirty;
}

/*Trimming Text to Maintain UI Constraints - KD - 4-12-2014*/
function TrimText(e) {
    if (e != null && e.trim().length > 30) {
        var value = e;
        var modvalue = value.substring(0, 20);
        return modvalue + "..."
    }
    else {
        if (e == undefined)
            return "";
        else
            return e;
    }
}


function TrimText(e,length) {
    if (e != null && e.trim().length > length) {
        var value = e;
        var modvalue = value.substring(0, length-10);
        return modvalue + "..."
    }
    else {
        if (e == undefined)
            return "";
        else
            return e;
    }
}

//Method opens Custom pop up
var openCustomPopUp = function (header, body, yesCallBack, noCallBack, okCallBack, args, popUpType, isPreventDefault) {
    scrollTop();
    var customModal = $("#customModal");
    var btnOk = $("#btnOk");
    var btnYes = $("#btnYes");
    var btnNo = $("#btnNo");
   

    var noHandler = function () {
        if ((noCallBack != undefined || noCallBack != null) && typeof noCallBack === "function") {
            if (args != undefined || args != null) {
                noCallBack(args);
            }
            else {
                noCallBack();
            }
        }
    };

    customModal.show();
    showOverlay1();
    btnYes.hide(); btnNo.hide(); btnOk.hide();
    $(document).keydown(function (e) {
        if (e.keyCode == 9) {
            e.preventDefault();
        }
    });
    $("#btnModalClose").off().on('click', function () {
        noHandler();
        customModal.hide();
        hideOverlay1();
        $(document).unbind("keydown");
    });

    $("#customModal .modal-header .modal-title b").html(header);
    $("#customModal .modal-body>p").html(body);

    if (popUpType == "confirm" || popUpType == undefined || popUpType == null || popUpType == "default") {
        btnYes.show(); btnNo.show();

        btnNo.off().on('click', function () {
            noHandler();
            customModal.hide();
            hideOverlay1();
            $(document).unbind("keydown");
            return false;
        });

        btnYes.off().on('click', function () {
            customModal.hide();
            hideOverlay1();
            if ((yesCallBack != undefined || yesCallBack != null) && typeof yesCallBack === "function") {
                if (args != undefined || args != null) {
                    yesCallBack(args);
                }
                else {
                    yesCallBack();
                }
            }
            
            return true;
        });
       
    }
    else {
        btnYes.hide(); btnNo.hide();
        btnOk.show();

        btnOk.off().on('click', function () {
            if ((okCallBack != undefined || okCallBack != null) && typeof okCallBack === "function") {
                if (args != undefined || args != null) {
                    okCallBack(args);
                }
                else {
                    okCallBack();
                }
            }
            customModal.hide();
            hideOverlay1();
            $(document).unbind("keydown");
            return true;
        });
      
    }

    if (isPreventDefault && (args != undefined || args != null))
        args.preventDefault();
}

//Method deletes a particular row from kendo grid based on uid
function deleteRowKendoGridByUID(kendoGrid, uID) {
    var dataSource = kendoGrid.dataSource;
    var dataItem = dataSource.getByUid(uID);
    dataSource.remove(dataItem);
    dataSource.sync();
}

//Method deletes a particular row from kendo grid based on dataitem
function deleteRowKendoGridByDataItem(kendoGrid, dataItem) {
    var dataSource = kendoGrid.dataSource;
    dataSource.remove(dataItem);
    dataSource.sync();    
}

function CompletelyRefreshKendoGrid(_gridId) {
    var grid = $("#" + _gridId).data('kendoGrid');
    grid.dataSource.query({
        page: 1,
        pageSize: 10,
        sort: {},
        filter: []
    });
}

function RefreshKendoGridWithIntactConfig(_gridId) {
    var grid = $("#" + _gridId).data('kendoGrid');
    if (grid.dataSource.view().length == 0) {
        var currentPage = grid.dataSource.page();
        if (currentPage > 1) {
            grid.dataSource.page(currentPage - 1);
        } else {
            //Refresh data, page 1
            grid.dataSource.page(1);
        }
    } else {
        //Refresh data, same page
        grid.dataSource.read();
    }
}

    function showOverlay()
    {
        $("#divOverlay").show();
    }

    function hideOverlay() {
        $("#divOverlay").hide();
    }

    function showOverlay1() {
        $("#divOverlay1").show();
    }

    function hideOverlay1() {
        $("#divOverlay1").hide();
    }

    //Method opens modal dialog
    var openModalDialog = function (content, unSavedDataHandler, args, isPreventDefault) {
        scrollTop();
        modalDialog = $("#ModalDialog");
        modalDialog.show();
        showOverlay();
        $(document).keydown(function (e) {
            if (e.keyCode == 9) {
                e.preventDefault();
            }
        });
        $("#btnModalDialogClose").off().on('click', function () {
            if ((unSavedDataHandler != undefined || unSavedDataHandler != null) && typeof unSavedDataHandler === "function")
                unSavedDataHandler();
            else
            {
                modalDialog.hide();
                hideOverlay();
                $(document).unbind("keydown");
            }        
        });

        if (content != null || content != undefined)
            $("#modalDialogContent").html(content);
   
    }


    function HideVerticalScrollBars(gridID) {
        $("#" + gridID + " .k-grid-header").css("padding-right", "0!important");
        $("#" + gridID + " .k-grid-content").css("overflow-y", "visible");
        $("#" + gridID + " .k-grid-content-locked").css("height", "auto!important");
    }

    function FixFilterScroll(gridID)
    {
        gridID.thead.closest(".k-grid-header-wrap").on("scroll", function () {
            var offset = $(this).scrollLeft();
            gridID.content.scrollLeft(offset);
        })

    }

    // function to display tooltip for kendo grid cells
    function DisplayKendoTooltip()
    {
        kendo.ui.Tooltip.fn._show = function (show) {
            return function (target) {
                var e = {
                    sender: this,
                    target: target,
                    preventDefault: function () {
                        this.isDefaultPrevented = true;
                    }
                };

                if (typeof this.options.beforeShow === "function") {
                    this.options.beforeShow.call(this, e);
                }
                if (!e.isDefaultPrevented) {
                    show.call(this, target);
                }
            };
        }(kendo.ui.Tooltip.fn._show);


        $("div[data-role='grid']").kendoTooltip({
            showOn: "mouseenter",
            position: "relative",
            filter: "table tr:not(.k-grid-edit-row) td[role=gridcell]:not(.noDefaultKendoTooltip),table th.k-header .k-link",
            beforeShow: function (e) {

                if ((e.target.attr("data-container-for") != undefined) || ((e.target.children().length > 0 && e.target.children().get(0).tagName == "A") || e.target.text().trim() == "")) {
                    e.preventDefault();
                }
            },
            content: function (e) {
                var text;
                if (e.target.children().length > 0 && e.target.children().get(0).tagName == "SELECT") {
                    text = $(e.target).children().first().val();
                } else {
                    text = e.target.text();
                }
                return text;
            }
        });
    }



    //Begin--Grid state management----------------------------------------------------

    //retrieves grid state like list of visible and hidden columns
    function retrieveGridState(gridID) {
        var grid = $('#' + gridID).data('kendoGrid');
        var hiddenColumns = retrieveGridHiddenColumns(grid);
        var visibleColumns = retrieveGridVisibleColumns(grid);
        var gridSettings = formatGridSettings(hiddenColumns, visibleColumns);
        return gridSettings;
    }

    //retrieves list of hidden columns
    function retrieveGridHiddenColumns(grid) {
        var hiddenColumns = [];
        var length = grid.columns.length;
        for (var i = 0; i < length; i++) {
            var column = grid.columns[i];
            if (column.hidden) {
                hiddenColumns.push(column.field);
            }
        }
        return hiddenColumns;
    }

    //retrieves list of visible columns
    function retrieveGridVisibleColumns(grid) {
        var visibleColumns = [];
        var length = grid.columns.length;
        for (var i = 0; i < length; i++) {
            var column = grid.columns[i];
            if (!column.hidden) {
                visibleColumns.push(column.field);
            }
        }
        return visibleColumns;
    }

    //formats grid state to json
    function formatGridSettings(hiddenColumns, visibleColumns) {
        return (kendo.stringify(hiddenColumns) + '%' + kendo.stringify(visibleColumns));
    }

    //sets grid state
    function setGridState(gridID,gridSettings) {
        var grid = $('#' + gridID).data('kendoGrid');
        var datasource = grid.dataSource;
        if (gridSettings != null && gridSettings.length > 0) {
            var gridSetting = gridSettings.split('%');
            var gridHiddenColumns = JSON.parse(gridSetting[0]);
            var gridvisibleColumns = JSON.parse(gridSetting[1]);
            restoreGridColumns(grid, gridHiddenColumns, gridvisibleColumns);
        }
    }

    //restores grid columns
    function restoreGridColumns(grid, gridHiddenColumns, gridvisibleColumns) {
        if (gridHiddenColumns != null && gridHiddenColumns.length > 0) {
            var length = gridHiddenColumns.length;
            for (var i = 0; i < length; i++) {
                grid.hideColumn(gridHiddenColumns[i]);
            }
        }

        if (gridvisibleColumns != null && gridvisibleColumns.length > 0) {
            var length = gridvisibleColumns.length;
            for (var i = 0; i < length; i++) {
                grid.showColumn(gridvisibleColumns[i]);
            }
        }
    }


    //saves transaction grid state on log off
    function saveTransactionGridStateOnlogOff() {
        stoSaas.flags.isLogOffClicked = true;
        var transactionGridSettings = sessionStorage.getItem("transactionGridSettings");
        if (transactionGridSettings != null) {
            var promise = $.ajax({
                url: stoSaas.urls.transaction.saveGridSettings,
                type: 'POST',
                async:false,
                data: transactionGridSettings,
                contentType: 'application/json; charset=utf-8',
                success: function (response) {
                    sessionStorage.removeItem("transactionGridSettings");
                },
                error: function (a, b, c) {

                }
            });
        }
        else
        {
            transactionGridSettings = retrieveGridState('transactionGrid');
            if (isTransactionPage) {
                var promise = $.ajax({
                    url: stoSaas.urls.transaction.saveGridSettings,
                    type: 'POST',
                    async: false,
                    data: transactionGridSettings,
                    contentType: 'application/json; charset=utf-8',
                    success: function (response) {
                        sessionStorage.removeItem("transactionGridSettings");
                    },
                    error: function (a, b, c) {

                    }
                });
            
            }
        }
    }

    //Disable header row interactions
    function DisableGridSortingInEditModes() {
        $(".k-grid-header .k-link").click(function (e) {
            if (isAnyUnSavedData) {
                e.stopPropagation();
                return false;
            }
        });
    }

    function DisableGridSorting(context) {
        $(".k-grid-header .k-link", context).click(function (e) {
            e.stopPropagation();
            return false;
        });
    }

    function EnableGridSorting(context) {
        $(".k-grid-header .k-link", context).unbind();
    }

    function EnableGridFilterRows(context) {
        $(".k-filter-row", context).removeAttr("style").unbind();
        $(".k-filter-row", context).find(".k-input").removeAttr("disabled", "true");
        $("tr.k-filter-row input[data-role=datepicker]", context).each(function () { $(this).data("kendoDatePicker").enable(true); });
        $(".k-filter-row button[data-bind='visible:operatorVisible']", context).removeAttr("disabled");
    }

    function DisableGridFilterRows(context) {
        $(".k-filter-row", context).css({ opacity: 0.3, cursor: 'default' }).click(function (e) { e.stopImmediatePropagation(); return false; });
        $(".k-filter-row", context).find(".k-input").attr("disabled", "true");
        $("tr.k-filter-row input[data-role=datepicker]", context).each(function () { $(this).data("kendoDatePicker").enable(false); });
        $(".k-filter-row button[data-bind='visible:operatorVisible']", context).attr("disabled", true);
    }

    //End--Grid state management------------------------------------------------------


    function formatMoney(number, places, symbol, thousand, decimal) {
        number = number || 0;
        places = !isNaN(places = Math.abs(places)) ? places : 2;
        symbol = symbol !== undefined ? symbol : "$";
        thousand = thousand || ",";
        decimal = decimal || ".";
        var negative = number < 0 ? "-" : "",
            i = parseInt(number = Math.abs(+number || 0).toFixed(places), 10) + "",
            j = (j = i.length) > 3 ? j % 3 : 0;
        return symbol + negative + (j ? i.substr(0, j) + thousand : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + thousand) + (places ? decimal + Math.abs(number - i).toFixed(places).slice(2) : "");
    }

    function PadStringLeft(sourceStr,padChar,resultLen) {
        var retStr = sourceStr;
        for (var i = retStr.length; i < resultLen; i++) {
            retStr = padChar + retStr;
        }
        return retStr;
    }

    /*----Region Location Lookup ----*/

    var locationFillInSection;
    var locationDefaultSelectionFlag = false;

    //Function for Location Lookup Pop Up
    function openLocationLookupPopup(sectionId) {
        showLoader();
        locationFillInSection = $("#" + sectionId);
    
        //Load view after required script is loaded
        $.getScript(stoSaas.urls.lookup.location.ScriptUrl).done(function (e) { GetLocationsView(); });
    }

    //Function for fetching locations
    function GetLocationsView() {
        $.ajax({
            url: stoSaas.urls.lookup.location.LookupUrl,
            type: 'GET',
            contentType: 'application/html; charset=utf-8',
            success: function (response) {
                hideLoader();
                openModalDialog(response, CloseLocationPopUp);
                DisplayKendoTooltip();
            },
            error: function () {
                hideLoader();
                //TO DO
            }
        });
    }

    //Function executed on pop up close
    function CloseLocationPopUp(e) {
        $("#ModalDialog").hide();
        hideOverlay();
        locationFillInSection = null;
    }

    /*----EndRegion Location Lookup ----*/

    /*----Region Customer Lookup ----*/

    //Method to load Customer details screen
    function loadCustomerLookup(id) {
        showLoader();
        fieldId = $("#" + id);
        //Load view after required script is loaded
        $.getScript(stoSaas.urls.lookup.customer.ScriptUrl).done(
            function (e) {
                $.ajax({
                    url: stoSaas.urls.lookup.customer.LookupUrl,
                    type: 'GET',
                    contentType: 'application/html; charset=utf-8',
                    success: function (response) {
                        hideLoader();
                        openModalDialog(response);
                        DisplayKendoTooltip();
                    },
                    error: function () {
                        hideLoader();
                        var e = {};
                        e.errors = 'Request not formed properly.';
                        showCustomError("pCustomerLookupCustomError", e, 10000);
                    }
                });
            });    
    }

    /*----EndRegion Customer Lookup ----*/

    /*----Region Geocode Lookup ----*/

    var geocodeFillInSection;

    //Function for Location Lookup Pop Up
    function OpenGeocodeLookupPopup(sectionId) {
        showLoader();
        geocodeFillInSection = $("#" + sectionId);

        //Load view after required script is loaded
        $.getScript(stoSaas.urls.lookup.geocode.ScriptUrl).done(function (e) { GetGeocodeLookupView(); });
    }

    //Function for fetching locations
    function GetGeocodeLookupView() {
        $.ajax({
            url: stoSaas.urls.lookup.geocode.LookupUrl,
            type: 'GET',
            contentType: 'application/html; charset=utf-8',
            success: function (response) {
                hideLoader();
                openModalDialog(response, CloseGeocodeLookupPopUp);
                keepOpenCascadeDropDownListOnSelection();
                DisplayKendoTooltip();
            },
            error: function () {
                hideLoader();
                //TO DO
            }
        });
    }

    //Function executed on pop up close
    function CloseGeocodeLookupPopUp(e) {
        $("#ModalDialog").hide();
        hideOverlay();
        geocodeFillInSection = null;
    }

    /*----EndRegion Geocode Lookup ----*/


    /*----Region SKU/Category Lookup ----*/

    //Method to load SKU/Category details screen
    function loadSKUCategoryLookup(id) {
        showLoader();
        fieldId = $("#" + id);
        //Load view after required script is loaded
        $.getScript(stoSaas.urls.lookup.skuCategory.ScriptUrl).done(
            function (e) {
                $.ajax({
                    url: stoSaas.urls.lookup.skuCategory.LookupUrl,
                    type: 'GET',
                    contentType: 'application/html; charset=utf-8',
                    success: function (response) {
                        hideLoader();
                        openModalDialog(response);
                        DisplayKendoTooltip();
                    },
                    error: function () {
                        hideLoader();
                        var e = {};
                        e.errors = 'Request not formed properly.';
                        showCustomError("pSKUCategoryLookupCustomError", e, 10000);
                    }
                });
            });
    }

    /*----EndRegion SKU/Category Lookup ----*/

    /*----Region Tax AuthorityLookup  */

    function LoadTaxAuthorityLookup(taxId, taxAuthorityId, defaultCompany, authSrchEffDate) {
        showLoader();
        $.getScript(stoSaas.urls.lookup.TaxAuthorityLookup.ScriptUrl).done(function (e) {
            $.getScript(stoSaas.urls.lookup.TaxAuthorityLookupRateDetails.ScriptUrl);
            GetTaxAuthorityLookupView(taxId, taxAuthorityId, defaultCompany, authSrchEffDate);
        });
    }
    function GetTaxAuthorityLookupView(taxId, taxAuthorityId, companyId, authSrchEffDate) {
        var promise = $.ajax({
            url: stoSaas.urls.lookup.TaxAuthorityLookup.LookupUrl,
            type: 'GET',
            contentType: 'application/html; charset=utf-8',
            success: function (response) {            
                openModalDialog(response);
                $("tr.k-filter-row input[data-role=numerictextbox]", "#TalGeocodesGrid").each(function () {
                        $(this).data("kendoNumericTextBox").options.format = "n3";
                        $(this).data("kendoNumericTextBox").options.decimals = "3";
                    });
                DisplayKendoTooltip();
            },
            error: function () {
                hideLoader();
            },
            complete: function () {
                hideLoader();
            }
        });
    }

    function LoadTaxAuthRateDetailsLookup(taxId, taxAuthorityId, defaultCompany, authSrchEffDate) {
        showLoader();
        $.getScript(stoSaas.urls.lookup.TaxAuthorityLookupRateDetails.ScriptUrl).done(function (e) {
            GetTaxAuthorityLookupViewPage3(taxId, taxAuthorityId, defaultCompany, authSrchEffDate);
        });
    }

    function GetTaxAuthorityLookupViewPage3(taxId, taxAuthorityId, companyId, authSrchEffDate) {
        var promise = $.ajax({
            url: stoSaas.urls.lookup.TaxAuthorityLookupRateDetails.LookupUrl,
            type: 'GET',
            contentType: 'application/html; charset=utf-8',
            success: function (response) {
                openModalDialog(response);
                if (taxAuthorityId != undefined && taxAuthorityId > 0) {
                    curerntSelectedtaxAuthority = taxAuthorityId;
                    curerntSelectedtaxId = taxId;
                    $("#btnSearchByEffDate").hide();
                    $("#DtPckrAuthorityEffDate").data("kendoDatePicker").value(authSrchEffDate);
                    $("#DtPckrAuthorityEffDate").data("kendoDatePicker").enable(false);
                    $('#treeviewAuthorityHierarchy').data('kendoTreeView').dataSource.read();
                }
                DisplayKendoTooltip();
            },
            error: function () {
                hideLoader();
            },
            complete: function () {
                hideLoader();
            }
        });
    }
    /*----Region Tax AuthorityLookup  */


    /*----Region Spinner ----*/
    function showLoader() {
        $("#divLoadingMask").show();
        //scrollTop();
    }

    function hideLoader() {
        $("#divLoadingMask").hide();
    }
    /*----EndRegion Spinner ----*/

    /*----Region FormatAmountLabel ----*/

    function FormatAmountLabel(element, container, amount) {
        var el = container.find(element);
        el.text(amount);
        if (amount < 0 || amount.indexOf('(') > -1 ) {
            el.css("color", "red");
        }
        else {
            el.css("color", "");
        }
    }

    /*----EndRegion FormatAmountLabel ----*/

    /*----Region Format amount in Grid column ----*/
    function FormatAmountInGridColumn(amount) {
        if (amount == null) {
            amount = "0";
        }
        if(amount.indexOf('(') > -1){
            return "<span style=\"color:red;\">" + amount + "</span>";
        }else{
            return "<span>" + amount + "</span>";
        }
    }

    /*----Region Format amount in Grid column ----*/

    //Glabal ajax setting to handle errors
    //$(function () {
    //    $(document).ajaxError(function (e, xhr, settings) {
    //        if (xhr.status == 401) {
    //            window.location.href = "/Authorize/Unauthorize";
    //        }
    //    });
    //});

    //$(function () {
    //    $.ajaxSetup({
    //        statusCode: {
    //            401: function () {
    //                window.location.href = "/Authorize/Unauthorize";
    //            }
    //        }
    //    });
    //});

//Global method gets fired on completion of each and every ajax request
    $(function () {
        $(document).ajaxComplete(function (e, xhr, settings) {
            if (xhr.status == 401) {
                //To suppress unsaved data warning message
                isAnyUnSavedData = false;
                if (e.currentTarget.location.pathname != undefined)
                    window.location.href = stoSaas.urls.account.loginUrl + "?ReturnUrl=" + e.currentTarget.location.pathname;
                else
                    window.location.href = stoSaas.urls.account.loginUrl;
            }
            if (xhr.status == 403) {
                //To suppress unsaved data warning message
                isAnyUnSavedData = false;
                window.location.href = stoSaas.urls.authorization.unAuthorizeUrl;
            }
            if (settings.url.indexOf("api/reports/") > 0 && xhr.status == 0 && xhr.statusText == "error")
            {
                isAnyUnSavedData = false;
                window.location.href = stoSaas.urls.account.loginUrl;
            }
            {
                var successMessage = xhr.getResponseHeader('SuccessMessage');
                var errorMessage = xhr.getResponseHeader('TimeOutError');
                if (successMessage != null) {
                    showToasterMessage("divToaster", successMessage, 10000);
                }
                if (errorMessage != null && errorMessage.indexOf('Timeout') == 0) {
                    showToasterMessage("divToasterDanger", errorMessage, 10000);
                }
            }
        });
    });

    function updateUserAccessMatrix() {
        var promise= $.ajax({
            url: stoSaas.urls.authorization.getUserAccessMatrixUrl,
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            async:false,
            success: function (response) {

            },
        });
        return promise;
    }
 

    function reloadPage()
    {
        window.location.reload();
    }

    function updateLegalFilingEntityIdInSession(lfeId) {
        $.ajax({
            url: stoSaas.urls.lfe.updateLfeIdUrl,
            type: 'POST',
            async: false,
            data:{lfeSurrogate:lfeId},
            //contentType: 'application/json; charset=utf-8',
            success: function (response) {
                if(isReloadPage)
                    reloadPage();
            },
        });
    }

    function updateCompanyIdInSession(companyId) {
        var promise= $.ajax({
            url: stoSaas.urls.company.updateCompanyIdUrl,
            type: 'POST',
            async: false,
            data: { companySurrogate: companyId },
            //contentType: 'application/json; charset=utf-8',
            success: function (response) {
                if (isReloadPage)
                    reloadPage();
            },
        });
        return promise;
    }

    function disableCompanyDropdown() {
        $("#DdlBRCompany").data("kendoDropDownList").enable(false);
    }

    function enableCompanyDropdown() {
        $("#DdlBRCompany").data("kendoDropDownList").enable(true);
    }

    function disableLfeDropdown() {
        $("#DdlBRLFE").data("kendoDropDownList").enable(false);
    }

    function enableLfeDropdown() {
        $("#DdlBRLFE").data("kendoDropDownList").enable(true);
    }

    function disableControlForIE10()
    {
        $(".readonly").click(function (e) { e.stopImmediatePropagation(); return false; });
        $(".RO").click(function (e) { e.stopImmediatePropagation(); return false; });
        $(".button-disable").click(function (e) { e.stopImmediatePropagation(); return false; });
    }

    function DisableInputControlForIE10(id) {
        $("#" + id).blur(function () {
            $(this).prop('disabled', false);
        });
        $("#" + id).focus(function () {
            $(this).prop('disabled', true);

            //TODO: Verify
            $(this).trigger('blur');
        });
    }
    //Method used to do post request
    function post(path, params, method) {
        method = method || "post"; // Set method to post by default if not specified.

        // The rest of this code assumes you are not using a library.
        // It can be made less wordy if you use one.
        var form = document.createElement("form");
        form.setAttribute("method", method);
        form.setAttribute("action", path);

        for (var key in params) {
            if (params.hasOwnProperty(key)) {
                var hiddenField = document.createElement("input");
                hiddenField.setAttribute("type", "hidden");
                hiddenField.setAttribute("name", key);
                hiddenField.setAttribute("value", params[key]);

                form.appendChild(hiddenField);
            }
        }

        document.body.appendChild(form);
        form.submit();
    }

    //Select Indexed Item of Kendo TabStrip. Index starts from 0
    function SelectTabStripIndex(tabStripId, targetIndex) {
        var _tabStripObj = $("#" + tabStripId).data("kendoTabStrip");
        _tabStripObj.select(GetTabStripIndexItem(_tabStripObj, targetIndex));
    }

    //Get Kendo TabStrip Index Item
    function GetTabStripIndexItem(tabStripObj, targetIndex) {    
        return tabStripObj.tabGroup.children("li").eq(targetIndex);
    }


    //$.ajaxPrefilter(function (options, originalOptions) {
    //    var token = $('input[name="__RequestVerificationToken"]').val();
    //    if (options.type.toUpperCase() == "POST") {
    //        options.data = $.param($.extend(originalOptions.data, { __RequestVerificationToken: token }));
    //    }
    //});

    function downloadInvoice(transactionId) {
        showLoader();
        var url = stoSaas.urls.transaction.downloadInvoice;
        window.location.href = url + '?transactionId=' + transactionId;
        setTimeout(function (e) { hideLoader(); }, 3000);
    }