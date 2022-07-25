function getParameterByName(name, url) {
    if (!url) {
        url = window.location.href;
    }
    name = name.replace(/[\[\]]/g, "\\$&");
    var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
        results = regex.exec(url);
    if (!results) return null;
    if (!results[2]) return '';
    return decodeURIComponent(results[2].replace(/\+/g, " "));
}

function MsgboxError(id, Message) {
    ClearMsgbox(id);
    $(id).addClass("alert-danger");
    $(id).html(Message);
    $(id).show(300);
    setTimeout(function () { $(id).hide(500); }, 3000);
}

function MsgboxSuccess(id, Message) {
    ClearMsgbox(id);
    $(id).addClass("alert-success");
    $(id).html(Message);
    $(id).show(300);
    setTimeout(function () { $(id).hide(500); }, 3000);
}

function ClearMsgbox(id) {
    $(id).removeClass("alert-success");
    $(id).removeClass("alert-info");
    $(id).removeClass("alert-warning");
    $(id).removeClass("alert-danger");
    $(id).html("&nbsp;");
}

function chkall(source) {
    checkboxes = document.getElementsByName('id[]');
    for (var i = 0, n = checkboxes.length; i < n; i++) {
        checkboxes[i].checked = source.checked;
    }
}

function to_json(workbook) {
    var result = {};
    workbook.SheetNames.forEach(function (sheetName) {
        var roa = XLSX.utils.sheet_to_row_object_array(workbook.Sheets[sheetName]);
        if (roa.length > 0) {
            result[sheetName] = roa;
        }
    });
    return result;
}

function CheckRequireField(id) {
    var result = true;
    //Clear Old Warning
    $("input.required").each(function () {
        $(this).removeClass("alert-warning");
    });
    //Add new warning
    $("input.required").each(function () {
        if ($(this).val().trim().length === 0) {
            $(this).addClass("alert-warning");
            result = false;
        }
    });
    // Message alert
    if (!result) {
        ClearMsgbox(id);
        $(id).addClass("alert-warning");
        $(id).html("<b>Warning!</b> these field require.");
        $(id).show(300);
        setTimeout(function () { $(id).hide(500); }, 3000);
    }
    return result;
}