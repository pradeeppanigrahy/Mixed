
//Start-- Transaction details line items
function getRequiredDataForLineItem() {
    return {
        transactionId: $("#hdnTransactionId").val(),
        companySurrogate: -1,
        lfeSurrogate: -1
    };
}


function TransactionLineItemGrid_error_handler(e) {
    showCustomError("maintainTransactionLineItemsCustomError", e, 10000);
    return false;
}

function TransactionLineItemTaxGrid_error_handler(e) {
    showCustomError("maintainTransactionLineItemsCustomError", e, 10000);
    return false;
}


//gets called on databound
function TransactionLineItemTaxGrid_On_DataBound(e) {
    var grid = $("#TransactionLineItemGrid").data("kendoGrid");
    FixFilterScroll(grid);
    ResetLineItemTaxesBottomInfo();
}

//gets called on databound
function TransactionLineItemGrid_On_DataBound(e) {
    //var grid = $("#transactionGrid").data("kendoGrid");
    //FixFilterScroll(grid);
}

//change event handler
function TransactionLineItemTaxGrid_On_Change(e) {
    var tds = $.map(this.select(), function (item) {
        return $(item).find('td');
    });
    updateLineItemTaxesBottomInfo(this.select().context, tds[0]);
}


//updates transaction line items taxes bottom information
function updateLineItemTaxesBottomInfo(selectedRow, tds) {
    var fieldset = $(selectedRow).nextAll("#divTransactionLineItemTaxes")
    //var fieldset = $("#divTransactionLineItemTaxes");

    FormatAmountLabel("label#lblTransactionTaxAppliedOnTaxesVal", fieldset, tds[24].textContent);
    FormatAmountLabel("label#lblTransactionTaxableAmountOnTaxesVal", fieldset, tds[0].textContent);
    //fieldset.find("label#lblTransactionTaxAppliedOnTaxesVal").text(tds[20].textContent);
    //fieldset.find("label#lblTransactionTaxableAmountOnTaxesVal").text(tds[0].textContent);
    fieldset.find("label#lblTransactionTaxableQuantityOnTaxesVal").text(tds[1].textContent);
    FormatAmountLabel("label#lblTransactionFeeAppliedOnTaxesVal", fieldset, tds[2].textContent);


    fieldset.find("label#lblTransactionTaxRateAppliedOnTaxesVal").text(tds[23].textContent);
    FormatAmountLabel("label#lblTransactionTaxExemptAmountOnTaxesVal", fieldset, tds[3].textContent);
    fieldset.find("label#lblTransactionTaxExemptQuantityOnTaxesVal").text(tds[4].textContent);
    FormatAmountLabel("label#lblTransactionFeeAmountAppliedOnTaxesVal", fieldset, tds[5].textContent);

    FormatAmountLabel("label#lblTransactionTaxableTaxAmountOnTaxesVal", fieldset, tds[6].textContent);
    FormatAmountLabel("label#lblTransactionNonTaxableAmountOnTaxesVal", fieldset, tds[7].textContent);
    fieldset.find("label#lblTransactionNonTaxableQuantityOnTaxesVal").text(tds[8].textContent);
    fieldset.find("label#lblTransactionContributionOnTaxesVal").text(tds[9].textContent);

    fieldset.find("label#lblTransactionTaxationOROnTaxesVal").text(tds[10].textContent);
    fieldset.find("label#lblTransactionTaxabilityOROnTaxesVal").text(tds[11].textContent);
    fieldset.find("label#lblTransactionPercentageTaxableOnTaxesVal").text(tds[12].textContent);

}

function ResetLineItemTaxesBottomInfo() {
    $("label#lblTransactionTaxAppliedOnTaxesVal").text('');
    $("label#lblTransactionTaxableAmountOnTaxesVal").text('');
    $("label#lblTransactionTaxAppliedOnTaxesVal").text('');
    $("label#lblTransactionTaxableAmountOnTaxesVal").text('');
    $("label#lblTransactionTaxableQuantityOnTaxesVal").text('');
    $("label#lblTransactionFeeAppliedOnTaxesVal").text('');

    $("label#lblTransactionTaxRateAppliedOnTaxesVal").text('');
    $("label#lblTransactionTaxExemptAmountOnTaxesVal").text('');
    $("label#lblTransactionTaxExemptQuantityOnTaxesVal").text('');
    $("label#lblTransactionFeeAmountAppliedOnTaxesVal").text('');

    $("label#lblTransactionTaxableTaxAmountOnTaxesVal").text('');
    $("label#lblTransactionNonTaxableAmountOnTaxesVal").text('');
    $("label#lblTransactionNonTaxableQuantityOnTaxesVal").text('');
    $("label#lblTransactionContributionOnTaxesVal").text('');

    $("label#lblTransactionTaxationOROnTaxesVal").text('');
    $("label#lblTransactionTaxabilityOROnTaxesVal").text('');
    $("label#lblTransactionPercentageTaxableOnTaxesVal").text('');
}

function ResetLineItemTaxesBottomInfo() {
    $("label#lblTransactionTaxAppliedOnTaxesVal").text('');
    $("label#lblTransactionTaxableAmountOnTaxesVal").text('');
    $("label#lblTransactionTaxAppliedOnTaxesVal").text('');
    $("label#lblTransactionTaxableAmountOnTaxesVal").text('');
    $("label#lblTransactionTaxableQuantityOnTaxesVal").text('');
    $("label#lblTransactionFeeAppliedOnTaxesVal").text('');

    $("label#lblTransactionTaxRateAppliedOnTaxesVal").text('');
    $("label#lblTransactionTaxExemptAmountOnTaxesVal").text('');
    $("label#lblTransactionTaxExemptQuantityOnTaxesVal").text('');
    $("label#lblTransactionFeeAmountAppliedOnTaxesVal").text('');

    $("label#lblTransactionTaxableTaxAmountOnTaxesVal").text('');
    $("label#lblTransactionNonTaxableAmountOnTaxesVal").text('');
    $("label#lblTransactionNonTaxableQuantityOnTaxesVal").text('');
    $("label#lblTransactionContributionOnTaxesVal").text('');

    $("label#lblTransactionTaxationOROnTaxesVal").text('');
    $("label#lblTransactionTaxabilityOROnTaxesVal").text('');
    $("label#lblTransactionPercentageTaxableOnTaxesVal").text('');
}

//End--Transaction details line items




//Start- Transaction tax details

function TransactionTaxDetailsGrid_error_handler(e) {
    showCustomError("maintainTransactionTaxesCustomError", e, 10000);
    return false;
}


//gets called on databound
function TransactionTaxDetailsGrid_On_DataBound(e) {
    var grid = $("#transactionTaxDetailsGrid").data("kendoGrid");
    FixFilterScroll(grid);
    ResetTransactionTaxesBottomInfo();
}


//change event handler
function TransactionTaxDetailsGrid_On_Change(e) {
    var tds = $.map(this.select(), function (item) {
        return $(item).find('td');
    });
    updateTransactionTaxesBottomInfo(this.select(), tds[0]);
}

//updates transaction line items taxes bottom information
function updateTransactionTaxesBottomInfo(selectedRow, tds) {
    var fieldset = $("#divTransactionTaxDetails");

    FormatAmountLabel("label#lblTransactionTaxAppliedVal", fieldset, tds[14].textContent);
    FormatAmountLabel("label#lblTransactionTaxableSaleAmountVal", fieldset, tds[0].textContent);
    fieldset.find("label#lblTransactionTaxableQuantityVal").text(tds[1].textContent);
    FormatAmountLabel("label#lblTransactionFeeAppliedVal", fieldset, tds[15].textContent);

    FormatAmountLabel("label#lblTransactionTaxExemptAmountVal", fieldset, tds[2].textContent);
    fieldset.find("label#lblTransactionTaxExemptQuantityVal").text(tds[3].textContent);

    FormatAmountLabel("label#lblTransactionTaxableTaxAmountVal", fieldset, tds[4].textContent);
    FormatAmountLabel("label#lblTransactionNonTaxableAmountVal", fieldset, tds[5].textContent);
    fieldset.find("label#lblTransactionNonTaxableQuantityVal").text(tds[6].textContent);

    fieldset.find("label#lblTransactionPassOnRuleVal").text(tds[7].textContent);
    fieldset.find("label#lblTransactionAppearsOnBillAsVal").text(tds[8].textContent);
    fieldset.find("label#lblTransactionTaxIsImposedOnVal").text(tds[9].textContent);
}

function ResetTransactionTaxesBottomInfo() {
    $("label#lblTransactionTaxAppliedVal").text('');
    $("label#lblTransactionTaxableSaleAmountVal").text('');
    $("label#lblTransactionTaxableQuantityVal").text('');
    $("label#lblTransactionFeeAppliedVal").text('');

    $("label#lblTransactionTaxExemptAmountVal").text('');
    $("label#lblTransactionTaxExemptQuantityVal").text('');

    $("label#lblTransactionTaxableTaxAmountVal").text('');
    $("label#lblTransactionNonTaxableAmountVal").text('');
    $("label#lblTransactionNonTaxableQuantityVal").text('');

    $("label#lblTransactionPassOnRuleVal").text('');
    $("label#lblTransactionAppearsOnBillAsVal").text('');
    $("label#lblTransactionTaxIsImposedOnVal").text('');
}

function ResetTransactionTaxesBottomInfo() {
    $("label#lblTransactionTaxAppliedVal").text('');
    $("label#lblTransactionTaxableSaleAmountVal").text('');
    $("label#lblTransactionTaxableQuantityVal").text('');
    $("label#lblTransactionFeeAppliedVal").text('');

    $("label#lblTransactionTaxExemptAmountVal").text('');
    $("label#lblTransactionTaxExemptQuantityVal").text('');

    $("label#lblTransactionTaxableTaxAmountVal").text('');
    $("label#lblTransactionNonTaxableAmountVal").text('');
    $("label#lblTransactionNonTaxableQuantityVal").text('');

    $("label#lblTransactionPassOnRuleVal").text('');
    $("label#lblTransactionAppearsOnBillAsVal").text('');
    $("label#lblTransactionTaxIsImposedOnVal").text('');
}



//End- Transaction tax details




//Start-- Load transaction Information section

function loadTransactionInformationSection(transactionId) {
    $("#transactionInfoLoadingMask").show();
    var promise = $.ajax({
        url: transactionUrl.getTransactionInformation,
        type: 'GET',
        data: transactionId,
        contentType: 'application/html; charset=utf-8',
        success: function (response) {
            $("#transactionInfoLoadingMask").hide();
            if (response != null && response != '') {
                $("#divTransactionInformation").html(response);
            }
            else {
                var e = {};
                e.errors = 'Failed to load transaction information.';
                showCustomError("transactionInformationCustomError", e, 10000);
            }
        },
        error: function () {
            $("#transactionInfoLoadingMask").hide();
            var e = {};
            e.errors = 'Failed to load transaction information.';
            showCustomError("transactionInformationCustomError", e, 10000);
        }
    });

}

//End-- Load transaction Information section
function getTransactionId() {
    return {
        transactionId: $("#hdnTransactionId").val()
    };
}

//Start-- Load transaction address detail
function loadTransactionAddressDetails(transactionId) {
    $("#transactionInfoLoadingMask").show();
    var promise = $.ajax({
        url: transactionUrl.getTransactionAddressDetail,
        type: 'GET',
        data: transactionId,
        contentType: 'application/html; charset=utf-8',
        success: function (response) {
            $("#transactionInfoLoadingMask").hide();
            if (response != null && response != '') {
                $("#divTransactionAddressDetail").html(response);
            }
            else {
                var e = {};
                e.errors = 'Failed to load transaction address details.';
                showCustomError("transactionAddressDetailCustomError", e, 10000);
            }
        },
        error: function () {
            $("#transactionInfoLoadingMask").hide();
            var e = {};
            e.errors = 'Failed to load transaction address details.';
            showCustomError("transactionAddressDetailCustomError", e, 10000);
        }
    });

}

//End-- Load transaction address detail

//Start-- Transaction Proces Log

////------------- Transaction proces log grid functions
function getTransactionProcessLog() {
    return {
        transactionId: $("#hdnTransactionId").val(),
        gridType: 'Transaction'
    };
}

function TransactionProcessLogGrid_error_handler(e) {
    showCustomError("maintainTransactionProcessLogCustomError", e, 10000);
    return false;
}

function TransactionProcessLogGrid_On_DataBound(e) {
    var grid = $("#transactionProcessLogGrid").data("kendoGrid");
    EnableProcesLogTab(grid);
    FixFilterScroll(grid);
    $("#ProcessLongMessage_Transaction").text('');
}

function TransactionProcessLogGrid_On_Change(e) {
    var tds = $.map(this.select(), function (item) {
        return $(item).find('td');
    });
    displayProcessLogLongMessage($("#ProcessLongMessage_Transaction"), this.select(), tds[0]);
}

////------------- Transaction line item proces log grid functions
function getTransactionLineItemProcessLog() {
    return {
        transactionId: $("#hdnTransactionId").val(),
        gridType: 'LineItem'
    };
}

function TransactionLineItemProcessLogGrid_error_handler(e) {
    showCustomError("maintainTransactionLineItemProcessLogCustomError", e, 10000);
    return false;
}
function TransactionLineItemProcessLogGrid_On_DataBound(e) {
    var grid = $("#transactionLineItemProcessLogGrid").data("kendoGrid");
    EnableProcesLogTab(grid);
    FixFilterScroll(grid);
    $("#ProcessLongMessage_TransactionLineItem").text('');
}

function TransactionLineItemProcessLogGrid_On_Change(e) {
    var tds = $.map(this.select(), function (item) {
        return $(item).find('td');
    });
    displayProcessLogLongMessage($("#ProcessLongMessage_TransactionLineItem"), this.select(), tds[0]);
}

////---common methods
function displayProcessLogLongMessage(id, selectedRow, tds) {
    $(id).text(tds[0].textContent);
}
function EnableProcesLogTab(grid) {
    //Enable Process log tab if data is present
    if (grid.dataSource.data().length != 0) {
        var tabStrip = $("#tabstrip").kendoTabStrip().data("kendoTabStrip");
        tabStrip.enable(tabStrip.tabGroup.find('li:Contains("Error Log")'), true);
    }
}
//End-- Transaction Proces Log