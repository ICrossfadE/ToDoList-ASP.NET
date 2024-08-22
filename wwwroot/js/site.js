let todoId = null;

function createTodo() 
{
    const name = document.getElementById('form-input').value;
    const description = document.getElementById('form-textarea').value;

    $.ajax({
        url: 'Home/Insert',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({ Id: todoId, Name: name, Description: description }),
        success: function(response) {
            window.location.reload();
            console.log('Todo created:', response);
        },
    });
}

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

            $("#form-input").val(response.name);
            $("#form-textarea").val(response.description);

            // Show modal
            document.getElementById('default-modal').showModal(); // Використання методу showModal() для показу

            //Edit Button
            $("#create-button").text("Edit Todo");
            $("#create-button").removeClass("bg-green-500");
            $("#create-button").removeClass("hover:bg-green-600");
            $("#create-button").addClass("bg-cyan-600");
            $("#create-button").addClass("hover:bg-cyan-700");
           
            // window.location.reload();
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

