let todoId = 0;
let statusId = 0;

// Toodo
function createTodo() 
{
    const name = document.getElementById('form-input').value;
    const description = document.getElementById('form-textarea').value;
    const status = document.getElementById('todo-status')?.value;

    $.ajax({
        url: 'Home/Insert',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({ Id: todoId, Name: name, Description: description, StatusId: status }),
        success: function(response) {
            window.location.reload();
            console.log('Todo created:', response);
        },
    });
}

// Status
function createStatus() 
{
    const name = document.getElementById('form-status-input').value;

    $.ajax({
        url: 'Status/Insert',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({ Id: statusId, StatusName: name }),
        success: function(response) {
            window.location.reload();
            console.log('Status created:', response);
        },
    });
}

// Todo
function updateTodo(id) 
{

    $.ajax({
        url: 'Home/UpdateTodo',
        type: 'GET',
        data: {
            id: id
        },
        dataType: 'json',
        success: function (response) {
             todoId = response.id;
             console.log('Response:', response);
            $("#form-input").val(response.name);
            $("#form-textarea").val(response.description);

            // Show modal
            document.getElementById('default-modal-todo').showModal();

            //Edit Button
            $("#create-button").text("Edit Todo");
            $("#create-button").removeClass("bg-green-500");
            $("#create-button").removeClass("hover:bg-green-600");
            $("#create-button").addClass("bg-cyan-500");
            $("#create-button").addClass("hover:bg-cyan-600");
        },
        error: function(error) {
            console.error('Error:', error);
        }
    });
}

// Status
function updateStatus(id) 
{
    $.ajax({
        url: 'Status/UpdateStatus',
        type: 'GET',
        data: {
            id: id
        },
        dataType: 'json',
        success: function (response) {
            statusId = response.id;
            console.log('Response:', response);
            $("#form-status-input").val(response.statusName);

            // Show modal
            document.getElementById('default-modal-status').showModal();

            //Edit Button
            $("#header-alert").text("Edit Status");
            $("#create-status-button").text("Edit Status");
            $("#create-status-button").removeClass("bg-green-500");
            $("#create-status-button").removeClass("hover:bg-green-600");
            $("#create-status-button").addClass("bg-cyan-500");
            $("#create-status-button").addClass("hover:bg-cyan-600");
        },
        error: function(error) {
            console.error('Error:', error);
        }
    });
}

function setStatus(id) 
{
    var status = $("#todo-status").val();

    $.ajax({
        url: 'Home/SetStatusId',
        type: 'POST',
        contentType: 'application/json', // Встановіть тип контенту як JSON
        data: JSON.stringify({
            Id: id,
            StatusId: parseInt(status)
        }),
        dataType: 'json',
        success: function (response) {  
            console.log(`Status updated successfully ${response}` );
        },
    });
}

// Todo
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

// Status
function deleteStatus(id) 
{
    $.ajax({
        url: 'Status/Delete',
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

