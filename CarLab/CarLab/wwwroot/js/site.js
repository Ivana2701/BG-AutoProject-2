

function checkIfStringIsEmtpy(str) {
    if (str == null || str == undefined) {
        str = '';
    }
    return /\S+/.test(str);
}


function showDeleteModal(primarykeyValue, primaryKeyColumn, tableName) {

    //--first empty existing fields
    $('#primarykeyValue').val('');
    $('#primaryKeyColumn').val('');
    $('#tableName').val('');

    //--now assign new values
    $('#primarykeyValue').val(primarykeyValue);
    $('#primaryKeyColumn').val(primaryKeyColumn);
    $('#tableName').val(tableName);


    $('#confirmation-modal-delete').modal('show');

}



function deleteRecordDynamic() {

    let primarykeyValue = $('#primarykeyValue').val();
    let primaryKeyColumn = $('#primaryKeyColumn').val();
    let tableName = $('#tableName').val();

    if (!checkIfStringIsEmtpy(primarykeyValue) || !checkIfStringIsEmtpy(primaryKeyColumn) || !checkIfStringIsEmtpy(tableName)
        || primarykeyValue == 0) {

        $('#confirmation-modal-delete').modal('hide');
        toastr.error('Table not exits or primary key value is null.');
        return false;
    }

    //let deleteURL = '@Url.Action("DeleteAnyRecord","BasicData")';
    let deleteURL = '/BasicData/DeleteAnyRecord';

    $.ajax({
        type: "POST",
        url: deleteURL,
        data: { primarykeyValue: primarykeyValue, primaryKeyColumn: primaryKeyColumn, tableName: tableName },
        cache: false,
        async: false,

        success: function (data) {
         
            console.log(data);
            if (data.success) {
                toastr.success('Deleted Successfully!');
                setTimeout(function () {
                    location.reload();
                }, 1500);
            } else {
                $('#confirmation-modal-delete').modal('hide');
                toastr.error(data.message);
            }


        },
        error: function (xhr, ajaxOptions, thrownError) {
            $('#confirmation-modal-delete').modal('hide');
            toastr.error('An error occured. Please try again.');
        }
    });

}

function clearDeleteModalData() {
    event.preventDefault();
    $('#primarykeyValue').val('');
    $('#primaryKeyColumn').val('');
    $('#tableName').val('');
}

function createDataTable(tableID, isPaginationAllowed) {

    let pagination = isPaginationAllowed == false ? false : true;

    if (!checkIfStringIsEmtpy(tableID)) {
        return false;
    }

    $('#' + tableID).DataTable({
        paging: pagination,
        searching: false,
        'sorting': false,
        info: false,
        lengthChange: false,
      
        lengthMenu: [15, 50, 75, 100],
        columnDefs: [{
            targets: 'no-sort', // column or columns numbers
            orderable: false // set orderable for selected columns
        }],
       
    });
}


function destroyDataTable(tableID) {
    if (!checkIfStringIsEmtpy(tableID)) {
        return false;
    }
    if ($.fn.DataTable.isDataTable('#' + tableID)) {
        $('#' + tableID).DataTable().destroy();
    }
}

