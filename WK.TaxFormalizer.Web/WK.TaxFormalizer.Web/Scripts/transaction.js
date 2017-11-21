
var currentSelectedCompanySurrogate;
var currentSelectedLfeSurrogate;
isTransactionPage = true;

//Highlighttab for current page - AM - 30-03-2015
$(document).ready(function () {
    $("#nav li").removeClass("active");
    $("#transmgmt").addClass("active");
    $("#transmgmt a").removeAttr('href');
});

$(document).ajaxSend(function (evt, request, settings) {
    if (settings.url.indexOf("/Transactions/TransactionLineItemAddressDetail") != -1) {
        $("#divLineItemAddressLoader").show();
    }
});

$(document).ajaxComplete(function (evt, request, settings) {
    if (settings.url.indexOf("/Transactions/TransactionLineItemAddressDetail") != -1) {
        $("#divLineItemAddressLoader").hide();
    }
});

//Begin Grid related events---------------------------------------------------------



//fires on data bound of company dropdown
function DdlBRCompany_DataBound(e) {
    $("#DdlBRCompany").data("kendoDropDownList").value(defaultCompany);
    currentSelectedCompanySurrogate = defaultCompany;
    $("#DdlBRLFE").data("kendoDropDownList").dataSource.read();
}

//function DdlBRCompany_Change(e)
//{
//    var yes = function () {
//        isCompanyDropdownChanged = true;
//        currentSelectedCompanySurrogate = $("#DdlBRCompany").data("kendoDropDownList").value();
//        updateCompanyIdInSession(currentSelectedCompanySurrogate);
//    };
//    var no = function () {
//        return false;
//    };
//    var confirmSave = openCustomPopUp('Company Change Confirmation',
//           'Are you sure you want to chnjhbuygvage company ?'
//           , yes, no);
//    return false;
    
//}

function DdlBRCompany_Select(e) {
    //var data = this.dataItem(e.item.index());
    //var current = this.value();  // this will have the old selected value 
    //var selectedType = data.CompanySurrogate; //this will have the new selected value
    //if (current == selectedType) {
    //    e.preventDefault();
    //    return false;
    //}
    //var yes = function () {
    //    $("#DdlBRCompany").data("kendoDropDownList").value(selectedType);
    //    isCompanyDropdownChanged = true;
    //    isAnyUnSavedData = false;
    //    currentSelectedCompanySurrogate = $("#DdlBRCompany").data("kendoDropDownList").value();
    //    updateCompanyIdInSession(currentSelectedCompanySurrogate);
    //    //reloadPage();
    //};
    //var no = function () {
    //    enableCompanyDropdown();
    //    enableLfeDropdown();
    //    e.preventDefault();
    //    return false;
    //};
    //var confirmSave = openCustomPopUp('Change Confirmation',
    //       'Are you sure you want to change Company ?'
    //       , yes, no);
    //disableCompanyDropdown();
    //disableLfeDropdown();
    //return e.preventDefault();

}

function DdlBRCompany_Change(e) {
    var yes = function () {
        isCompanyDropdownChanged = true;
        isAnyUnSavedData = false;
        currentSelectedCompanySurrogate = $("#DdlBRCompany").data("kendoDropDownList").value();
        updateCompanyIdInSession(currentSelectedCompanySurrogate);
    };
    var no = function () {
        enableCompanyDropdown();
        enableLfeDropdown();
        $("#DdlBRCompany").data("kendoDropDownList").value(currentSelectedCompanySurrogate);
        return false;
    };
    var confirmSave = openCustomPopUp('Change Confirmation',
           'Are you sure you want to change Company ?'
           , yes, no);
    disableCompanyDropdown();
    disableLfeDropdown();
}

//Method gets called on select of lfe Dropdown
function DdlBRLFE_Select(e) {
    //var data = this.dataItem(e.item.index());
    //var current = this.value();  // this will have the old selected value 
    //var selectedType = data.DivisionSurrogate; //this will have the new selected value
    //if (current == selectedType) {
    //    e.preventDefault();
    //    return false;
    //}
    //var yes = function () {
    //    $("#DdlBRLFE").data("kendoDropDownList").value(selectedType);
    //    currentSelectedLfeSurrogate = $("#DdlBRLFE").data("kendoDropDownList").value();
    //    isAnyUnSavedData = false;
    //    updateLegalFilingEntityIdInSession(currentSelectedLfeSurrogate);
    //    //reloadPage();
    //    //$('#transactionGrid').data('kendoGrid').dataSource.read();
    //};
    //var no = function () {
    //    enableCompanyDropdown();
    //    enableLfeDropdown();
    //    e.preventDefault();
    //    return false;
    //};
    //var confirmSave = openCustomPopUp('Change Confirmation',
    //       'Are you sure you want to change Legal Filing Entity ?'
    //       , yes, no);
    //disableCompanyDropdown();
    //disableLfeDropdown();
    //return e.preventDefault();
}

function DdlBRLFE_Change(e) {
    var yes = function () {
        currentSelectedLfeSurrogate = $("#DdlBRLFE").data("kendoDropDownList").value();
        isAnyUnSavedData = false;
        updateLegalFilingEntityIdInSession(currentSelectedLfeSurrogate);
    };
    var no = function () {
        enableCompanyDropdown();
        enableLfeDropdown();
        $("#DdlBRLFE").data("kendoDropDownList").value(currentSelectedLfeSurrogate);
        return false;
    };
    var confirmSave = openCustomPopUp('Change Confirmation',
           'Are you sure you want to change Legal Filing Entity ?'
           , yes, no);
    disableCompanyDropdown();
    disableLfeDropdown();
}

//Method gets called on change of lfe Dropdown
//function DdlBRLFE_Change(e) {
//    var yes = function () {
//        currentSelectedLfeSurrogate = $("#DdlBRLFE").data("kendoDropDownList").value();
//        updateLegalFilingEntityIdInSession(currentSelectedLfeSurrogate);
//        //reloadPage();
//        //$('#transactionGrid').data('kendoGrid').dataSource.read();
//    };
//    var no = function () {
//        return false;
//    };
//    var confirmSave = openCustomPopUp('Lfe Change Confirmation',
//           'Are you sure you want to change Legal Filing Entity ?'
//           , yes, no);
//    return false;
//}

//Method gets called after data is bound to lfe dropdown
function DdlBRLFE_DataBound(e) {
    if (isCompanyDropdownChanged) {
        //reloadPage();
    }
    else {
        $("#DdlBRLFE").data("kendoDropDownList").value(defaultLfe);
        currentSelectedLfeSurrogate = $("#DdlBRLFE").data("kendoDropDownList").value();
        $('#transactionGrid').data('kendoGrid').dataSource.read();
    }
}

//Function returns company and LFE Id
function getCompanyAndLFEId(e) {
    //hideCustomError("maintainTransactionCustomError");
    return {
        lfeSurrogate: 1,
        companySurrogate: 1,
    };
}

function filterDivisions(e) {
    return {
        companyid: $("#DdlBRCompany").data("kendoDropDownList").value()
    };
}

//Method gets called if any grid operation fails
function transactionGrid_error_handler(e) {
    if (e.errorThrown == "Unauthorized") {
        window.location.href = stoSaas.urls.account.loginUrl;
        return;
    }
    if (e.errorThrown == "Forbidden") {
        window.location.href = stoSaas.urls.authorization.unAuthorizeUrl;
        return;
    }
    showCustomError("maintainTransactionCustomError", e, 10000);
    return false;
}

//gets called on databound
function transactionGrid_On_DataBound(e) {
    //disableControlForIE10();
    var grid = $("#transactionGrid").data("kendoGrid");
    //FixFilterScroll(grid);
}

function readTransactionGrid() {
    $('#transactionGrid').data('kendoGrid').dataSource.read();
}


//End Grid related events---------------------------------------------------------


//Method to load transaction details screen
function loadTransactionDetails(transactionId, source) {
    showLoader();
    var tran = {};
    tran.transactionID = transactionId;
    tran.origin = source;
    var promise = $.ajax({
        url: transactionUrl.loadTransaction,
        type: 'GET',
        data: { transactionID: transactionId, origin: source },
        contentType: 'application/html; charset=utf-8',
        success: function (response) {
            hideLoader();
            openModalDialog(response);
            keepOpenCascadeDropDownListOnSelection();
            $("tr.k-filter-row input[data-role=numerictextbox]", "#transactionTaxDetailsGrid,#TransactionLineItemGrid").each(function () {
                $(this).data("kendoNumericTextBox").options.format = "n3";
                $(this).data("kendoNumericTextBox").options.decimals = "3";
            }); 
            DisplayKendoTooltip();
        },
        error: function () {
            hideLoader();
            var e = {};
            e.errors = 'Request not formed properly.';
            showCustomError("maintainTransactionCustomError", e, 10000);
        }
    });
}

//Method to save transaction grid settings into database
function saveTransactionGridSettings() {
    var transactionGridSettings = retrieveGridState('transactionGrid');
    sessionStorage.setItem("transactionGridSettings", transactionGridSettings);
}


//Method to get transaction grid settings from database
function getTransactionGridSettings() {
    var gridSettings = sessionStorage.getItem("transactionGridSettings");
    if (gridSettings == null) {
        var promise = $.ajax({
            url: transactionUrl.getGridSettings,
            type: 'GET',
            data: { gridID: 'transactionGrid' },
            contentType: 'application/json; charset=utf-8',
            success: function (response) {
                var transactionGridSettings = response;
                setGridState('transactionGrid', transactionGridSettings);
            },
            error: function (a, b, c) {

            }
        });
    }
    else {
        setTimeout(function () {
            setGridState('transactionGrid', gridSettings);
        }, 2000);
    }
}


//deletes a transaction 
function deleteTransaction(transactionID) {
    var no = function () {
        $("#customModal").hide();
    };
    var yes = function () {
        var tran = {};
        tran.transactionID = transactionID;
        var promise = $.ajax({
            url: transactionUrl.deleteTransaction,
            type: 'POST',
            data: JSON.stringify(tran),
            contentType: 'application/json; charset=utf-8',
            success: function (response, status, request) {
                if (response) {
                    var successMessage = request.getResponseHeader('SuccessMessage');
                    if (successMessage != null) {
                        showToasterMessage("divToaster", successMessage, 10000);
                        $('#transactionGrid').data('kendoGrid').dataSource.read();
                    }
                }
                else {
                    var errorMessage = request.getResponseHeader('ErrorMessage');
                    if (errorMessage != "Request is not formed properly") {
                        var ok = function () {
                            readTransactionGrid();
                        };
                        openCustomPopUp('Delete Notification',
                          errorMessage
                          , null, null, ok, null, 'alert', null);
                    }
                    else {
                        var e = {};
                        e.errors = errorMessage;
                        showCustomError("maintainTransactionCustomError", e, 10000);
                    }
                }
            },
            error: function (a, b, c) {
                var e = {};
                e.errors = 'Request not formed properly.';
                showCustomError("transactionDetailCustomError", e, 10000);
            }
        });
    };
    var confirmSave = openCustomPopUp('Delete Confirmation',
           'Are you sure you want to delete this transaction ?'
           , yes, no);
}

//finalizes a transaction 
function finalizeTransaction(transactionID) {
    var no = function () {
        $("#customModal").hide();
    };
    var yes = function () {
        var tran = {};
        tran.transactionID = transactionID;
        var promise = $.ajax({
            url: transactionUrl.finalizeTransaction,
            type: 'POST',
            data: JSON.stringify(tran),
            contentType: 'application/json; charset=utf-8',
            success: function (response, status, request) {
                if (response) {
                    var successMessage = request.getResponseHeader('SuccessMessage');
                    if (successMessage != null) {
                        showToasterMessage("divToaster", successMessage, 10000);
                        $('#transactionGrid').data('kendoGrid').dataSource.read();
                    }
                }
                else {
                    var errorMessage = request.getResponseHeader('ErrorMessage');
                    if (errorMessage != "Request is not formed properly") {
                        var ok = function () {
                            readTransactionGrid();
                        };
                        openCustomPopUp('Finalize Notification',
                          errorMessage
                          , null, null, ok, null, 'alert', null);
                    }
                    else {
                        var e = {};
                        e.errors = errorMessage;
                        showCustomError("maintainTransactionCustomError", e, 10000);
                    }
                }
            },
            error: function (a, b, c) {
                var e = {};
                e.errors = 'Request not formed properly.';
                showCustomError("maintainTransactionCustomError", e, 10000);
            }
        });
    };
    var confirmSave = openCustomPopUp('Finalize Confirmation',
           'Are you sure you want to finalize this transaction ?'
           , yes, no);

}

//cancel a transaction 
function cancelTransaction(transactionID) {
    var no = function () {
        $("#customModal").hide();
    };
    var yes = function () {
        var tran = {};
        tran.transactionID = transactionID;
        var promise = $.ajax({
            url: transactionUrl.cancelTransaction,
            type: 'POST',
            data: JSON.stringify(tran),
            contentType: 'application/json; charset=utf-8',
            success: function (response, status, request) {
                if (response) {
                    var successMessage = request.getResponseHeader('SuccessMessage');
                    if (successMessage != null) {
                        showToasterMessage("divToaster", successMessage, 10000);
                        $('#transactionGrid').data('kendoGrid').dataSource.read();
                    }
                }
                else {
                    var errorMessage = request.getResponseHeader('ErrorMessage');
                    if (errorMessage != "Request is not formed properly") {
                        var ok = function () {
                            readTransactionGrid();
                        };
                        openCustomPopUp('Cancel Notification',
                          errorMessage
                          , null, null, ok, null, 'alert', null);
                    }
                    else {
                        var e = {};
                        e.errors = errorMessage;
                        showCustomError("maintainTransactionCustomError", e, 10000);
                    }
                }
            },
            error: function (a, b, c) {
                var e = {};
                e.errors = 'Request not formed properly.';
                showCustomError("maintainTransactionCustomError", e, 10000);
            }
        });
    };
    var confirmSave = openCustomPopUp('Cancel Confirmation',
           'Are you sure you want to cancel this transaction ?'
           , yes, no);

}


//Load Returns UI
var loadedAmount = 0;
function LoadReturnTransaction(transactionId) {
    showLoader();
    //checkAndLoadReturn(transactionId);
    loadTransactionReturnView(transactionId);
}

//function checkAndLoadReturn(transactionId) {
//    //var allowReturn = false;
//    var promise = $.ajax({
//        url: stoSaas.urls.returnTransaction.CheckFullReturnUrl,
//        type: 'GET',
//        data: { transactionId: transactionId },
//        contentType: 'application/html; charset=utf-8',
//        success: function (response) {
//            if (response == 'True') {
//                hideLoader();
//                openCustomPopUp("Maintain Transactions", "This transaction has been fully returned!", null, null, "closeModalDialog();", null, "alert");  
//            }
//            else {
//                //allowReturn = true;
//                loadTransactionReturnView(transactionId)
//            }
//        },
//        error: function () {
//            hideLoader();
//            var e = {};
//            e.errors = 'Request not formed properly.';
//            showCustomError("pTransactionReturnCustomError", e, 10000);
//        }
//    });
//    //promise.done(function () { if (allowReturn) { loadTransactionReturnView(transactionId) } });
//}

//Enable and disable return type selection checkboxes
function HandleReturnsRadioBoxes(transactionId) {
    var currentRow = $("#btnReturnTransaction_" + transactionId).closest("tr");
    var currentGrid = $('#transactionGrid').data('kendoGrid');
    var rowDataItem = currentGrid.dataItem(currentRow);
    if (rowDataItem.TransactionReturnStatus == "Partial") {
        //Tranasction has been partially returned earlier, thus we only enable PartialReturn
        $("#PartialReturn").click();
        $("#FullReturn").attr('disabled', true);
    }
}

function loadTransactionReturnView(transactionId) {
    $.getScript(stoSaas.urls.returnTransaction.ScriptUrl).done(function (e) {
        var promise = $.ajax({
            url: transactionUrl.returnTransaction,
            type: 'GET',
            data: { transactionId: transactionId },
            contentType: 'application/html; charset=utf-8',
            success: function (response) {
                hideLoader();
                openModalDialog(response);
                DisplayKendoTooltip();
                HandleReturnsRadioBoxes(transactionId);
                $("#btnModalDialogClose").on('click', function (e) { isAnyUnSavedData = false; });
                $("tr.k-filter-row input[data-role=numerictextbox]", "#fullReturnLineItemGrid,#partialReturnLineItemGrid").each(function () {
                    $(this).data("kendoNumericTextBox").options.format = "n3";
                    $(this).data("kendoNumericTextBox").options.decimals = "3";
                });
            },
            error: function () {
                hideLoader();
                var e = {};
                e.errors = 'Request not formed properly.';
                showCustomError("maintainTransactionCustomError", e, 10000);
            }
        });
    });
}

function closeModalDialog() {
    $("#ModalDialog").hide();
    hideOverlay();
}

function ToggleButton(id, flag) {
    if ($("#" + id) != undefined) {
        if (flag) {
            $("#" + id).prop('disabled', false);
            $("#" + id).toggleClass("button-disable");
        }
        else {
            $("#" + id).prop('disabled', true);
            $("#" + id).toggleClass("button-disable");
        }
    }
}

var bindPartialReturnLineItemGridFlag = true;

function partialReturnLineItemGrid_Edit(e) {
    $("#Amount,#Quantity,#UnitCharge").focus(function () { removeFormatting(this); });
    $("#Quantity").blur(function () { calculateLineItemAmount(); });
    $("#Amount").blur(function () { calculateLineItemUnitCharge(); });
    $("#UnitCharge").blur(function () { calculateLineItemAmount(); });
    HideAllPartialLineItemsErrors();
    ToggleButton("btnSaveReturn",false);
    var gridNewRow = $('#partialReturnLineItemGrid .k-grid-edit-row');
    $(gridNewRow).find('input[id=TransactionLineItemProductInformationModel_OriginalQuantity]').parent().html(e.model.TransactionLineItemProductInformationModel.OriginalQuantity);
    $(gridNewRow).find('input[id=TransactionLineItemProductInformationModel_OriginalUnitCharge]').parent().html(e.model.TransactionLineItemProductInformationModel.OriginalUnitCharge);
    $(gridNewRow).find('input[id=TransactionLineItemProductInformationModel_OriginalLineItemAmount]').parent().html(e.model.TransactionLineItemProductInformationModel.OriginalLineItemAmount);

    DisablePageUIControls();
}

function partialReturnLineItemGrid_Save(e) {
    HideAllPartialLineItemsErrors();
    var currentGrid = $('#partialReturnLineItemGrid').data('kendoGrid');
    currentGrid.dataItem(e.container).set("Quantity", $("#Quantity").val());
    currentGrid.dataItem(e.container).set("UnitCharge", $("#UnitCharge").val());
    currentGrid.dataItem(e.container).set("Amount", $("#Amount").val());
    ToggleButton("btnSaveReturn", true);
    bindPartialReturnLineItemGridFlag = true;
}

function partialReturnLineItemGrid_SaveChanges(e) {
    var abc = e;
}

function partialReturnLineItemGrid_Cancel(e) {
    e.model.dirty = true;
    HideAllPartialLineItemsErrors();
    ToggleButton("btnSaveReturn", true);
    isAnyUnSavedData = false;
    EnablePageUIControls();
}

function partialReturnLineItemGrid_Remove(e) {
    HideAllPartialLineItemsErrors();
    if ($('#partialReturnLineItemGrid').data('kendoGrid').dataItems().length <= 1) {
        alert("A transaction must have at least one line item");
        $('#partialReturnLineItemGrid').data('kendoGrid').cancelChanges();
    }
}

function partialReturnLineItemGrid_RemoveClick(e) {
    var no = function () {
        //$('#RolesSummaryGrid').data('kendoGrid').cancelChanges();
    };
    var yes = function () {
        var currentGrid = $('#partialReturnLineItemGrid').data('kendoGrid');
        $('#partialReturnLineItemGrid').data('kendoGrid').removeRow($(e.currentTarget).closest("tr"));
    };
    if ($('#partialReturnLineItemGrid').data('kendoGrid').dataItems().length <= 1) {
        openCustomPopUp("Delete Not Allowed", "A transaction must have at least one line item", null, null, null, e, "alert", true);
    }
    else {
        openCustomPopUp('Delete Confirmation',
                    'Are you sure you wish to delete this Line Item ?', yes, no
                    );
    }
}

function partialReturnLineItemGrid_Change(e) {
    if (e.action == "sync") {
        if (bindPartialReturnLineItemGridFlag) {
            EnablePageUIControls();
            //RefreshKendoGridWithIntactConfig("partialReturnLineItemGrid");
        }
    }
    if (e.action == "itemchange") {
        if (e.field == "Quantity") {
            calculateLineItemAmount();
        }
        else if (e.field == "Amount") {
            //calculateLineItemUnitCharge();
        }
        else if (e.field == "UnitCharge") {
            calculateLineItemAmount();
        }
    }
}


function partialReturnLineItemGrid_errorHandler(e) {
    if (e.errorThrown == "Unauthorized") {
        window.location.href = stoSaas.urls.account.loginUrl;
        return;
    }
    if (e.errorThrown == "Forbidden") {
        window.location.href = stoSaas.urls.authorization.unAuthorizeUrl;
        return;
    }
    var editRowCount = $('#partialReturnLineItemGrid .k-grid-edit-row').length;
    if (editRowCount == 0) {
        bindPartialReturnLineItemGridFlag = true;
        $('#partialReturnLineItemGrid').data('kendoGrid').cancelChanges();
    }
    else {
        bindPartialReturnLineItemGridFlag = false;
        $('#partialReturnLineItemGrid').data('kendoGrid').one("dataBinding", function (e) {
            e.preventDefault();
        });
    }
    if (e.status == "customerror") {
        showCustomError("divPartialReturnGridErrors", e, 10000);
    }
    //showCustomErrorWithNoTimeout("divPartialReturnGridErrors", e);//showCustomError("divPartialReturnGridErrors", e, 10000);
    ToggleButton("btnSaveReturn", false);
}

//calculates line item amount
function calculateLineItemAmount() {
    if ($("#Quantity").val() != "" && $("#UnitCharge").val() != "") {
        var quantity = $("#Quantity").val().replace(/[$-.]/g, '');
        var unitcharge = $("#UnitCharge").val().replace(/[$-.]/g, '');
        var cntQuantityDecimalDigits = getDecimalDigitsCount($("#Quantity").val());
        var cntUnitChargeDecimalDigits = getDecimalDigitsCount($("#UnitCharge").val());

        if (!isNaN(quantity) && !isNaN(unitcharge) && cntQuantityDecimalDigits <= 8 && cntUnitChargeDecimalDigits <= 3) {
            var amount = parseFloat($("#Quantity").val().replace(/[^0-9-.]/g, '')) * parseFloat($("#UnitCharge").val().replace(/[^0-9-.]/g, ''));
            $("#Amount").val(parseFloat(amount).toFixed(2));
            var unitCharge = parseFloat($("#UnitCharge").val().replace(/[^0-9-.]/g, '')).toFixed(3);
            $("#UnitCharge").val(unitCharge);
        }
    }
}

//calculates unit charge of a line item
function calculateLineItemUnitCharge() {
    if ($("#Quantity").val() != "" && $("#Amount").val() != "") {
        var quantity = $("#Quantity").val().replace(/[$-.]/g, '');
        var lineItemAmount = $("#Amount").val().replace(/[$-.]/g, '');
        var cntQuantityDecimalDigits = getDecimalDigitsCount($("#Quantity").val());
        var cntLineItemAmountDecimalDigits = getDecimalDigitsCount($("#Amount").val());

        if (!isNaN(quantity) && !isNaN(lineItemAmount) && cntQuantityDecimalDigits <= 8 && cntLineItemAmountDecimalDigits <= 3) {
            if (quantity > 0 && lineItemAmount > 0) {
                var unitCharge = parseFloat($("#Amount").val().replace(/[^0-9-.]/g, '')) / parseFloat($("#Quantity").val().replace(/[^0-9-.]/g, ''));
                $("#UnitCharge").val(parseFloat(unitCharge).toFixed(3));
                $("#Amount").val(parseFloat($("#Amount").val().replace(/[^0-9-.]/g, '')).toFixed(2));
            }
        }
    }
}

function getDecimalDigitsCount(number) {
    if (number.indexOf('.') > -1) {
        return (number.length - 1) - number.indexOf('.');
    }
    return 0;
}

function removeFormatting(e) {
    var isNonNumeric = isNaN(e.value.replace(/[$-.]/g, ''));
    if (!isNonNumeric) {
        e.value = (e.value.replace(/[^0-9-.]/g, ''));
    }
}


function FormatFullReturnGridTotalLineItem() {
    document.getElementById("divfullReturnLineItemGridTotalLineItemQuantity").parentNode.colSpan = 6;
    document.getElementById("divfullReturnLineItemGridTotalLineItemAmount").parentNode.colSpan = 5;
    var qntyElem = document.getElementById("divfullReturnLineItemGridTotalLineItemQuantity").parentNode.outerHTML;
    var lineItemElem = document.getElementById("divfullReturnLineItemGridTotalLineItemAmount").parentNode.outerHTML;//.replace("Total Line Item Amount: ", "Total Line Item Amount: $");
    $("#fullReturnLineItemGrid tr.k-footer-template").html(qntyElem + lineItemElem);
}


function EnablePageUIControls() {
    enableCompanyDropdown();
    enableLfeDropdown();
    EnableGridOperations("partialReturnLineItemGrid");
}

function DisablePageUIControls() {
    disableCompanyDropdown();
    disableLfeDropdown();
    DisableGridOperations("partialReturnLineItemGrid");
}
