function deleteTodo(id) 
{
    $.ajax({
        url: 'Home/Delete',
        type: 'POST',
        data: {
            id: id
        },
        success: function() {
            window.location.reload();
            console.log('del', id);
        }
    });
}

function updateTodo(id) {

    $.ajax({
        url: 'Home/UpdateForm',
        type: 'GET',
        data: {
            id: id
        },
        dataType: 'json',
        success: function (response) {
            $("#ToDo_Name").val(response.name);
            $("#ToDo_Id").val(response.id);
            $("#form-button").val("Update Todo");
            $("#form-action").attr("action", "/Home/Update");
        },
        error: function(error) {
            console.error('Error:', error);
        }
    });
}