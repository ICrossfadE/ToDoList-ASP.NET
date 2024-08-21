function createTodo() 
{
    const name = document.getElementById('form-input').value;
    const description = document.getElementById('form-textarea').value;

    $.ajax({
        url: 'Home/Insert',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({ Name: name, Description: description }),
        success: function(response) {
            window.location.reload();
            console.log('Todo created:', response);
        },
    });
}

function updateTodo(id) {

    $.ajax({
        url: 'Home/UpdateTodo',
        type: 'GET',
        data: {
            id: id
        },
        dataType: 'json',
        success: function (response) {
            $("#ToDo_Name").val(response.name);
            $("#ToDo_Id").val(response.id);
            $("#form-button").val("Update Todo");
            $("#form-action").attr("action", "/Home/UpdateDatabase");
        },
        error: function(error) {
            console.error('Error:', error);
        }
    });
}

function deleteTodo(id) 
{
    $.ajax({
        url: 'Home/Delete',
        type: 'DELETE',
        data: {
            id: id
        },
        success: function() {
            window.location.reload();
            console.log('del', id);
        }
    });
}

