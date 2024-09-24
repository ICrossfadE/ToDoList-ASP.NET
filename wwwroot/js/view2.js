const addStatus = document.getElementById('add-status');
const dialogStatus = document.getElementById('default-modal-status');
const cancelStatusButtont = document.getElementById('cancel-status-button');
const createStatusButtont = document.getElementById('create-status-button');
const headerModal = document.getElementById(`header-alert`);
const deleteBotton = document.getElementById('delete');
const statusInput = document.getElementById('form-status-name-input');

// Status
addStatus.addEventListener('click', (event) => {
    event.preventDefault();

    headerModal.textContent = "New Status";
    dialogStatus.showModal();
});

// Alert
function showAlert(statusId) {
    const modal = document.getElementById(`alert-modal-todo-${statusId}`);
    modal.showModal();
}

function hideAlert(statusId) {
    const modal = document.getElementById(`alert-modal-todo-${statusId}`);
    modal.close();
}

// Cancel modal
cancelStatusButtont.addEventListener('click', (event) => {
    event.preventDefault();

    dialogStatus.close();
    statusInput.value = '';
    createStatusButtont.textContent = "Create Status";
    createStatusButtont.classList.remove("bg-cyan-600");
    createStatusButtont.classList.remove("hover:bg-cyan-700");
    createStatusButtont.classList.add("bg-green-500");
    createStatusButtont.classList.add("hover:bg-green-600");
});

// Create moodal
createStatusButtont.addEventListener('click', (event) => {
    event.preventDefault();

    dialogStatus.close();
    statusInput.value = '';
    createStatusButtont.textContent = "Create Status";
    createStatusButtont.classList.remove("bg-cyan-600");
    createStatusButtont.classList.remove("hover:bg-cyan-700");
    createStatusButtont.classList.add("bg-green-500");
    createStatusButtont.classList.add("hover:bg-green-600");
});

document.addEventListener("DOMContentLoaded", function () {
    var statusList = document.getElementById('statusList');

    new Sortable(statusList, {
        animation: 150,  // Анімація
        ghostClass: 'drag-placeholder', // Клас для елемента, який переміщується

        // Функція коли відпускаєм кнопку
        onEnd: function (evt) {
            var newOrder = Array.from(statusList.children)
                .filter(child => child.hasAttribute('data-id')) // Фільтруємо лише елементи з атрибутом data-id
                .map(function (child, index) {
                    return {
                        id: parseInt(child.getAttribute('data-id')), // Отримуємо id
                        statusName: child.querySelector('.statusName').innerText, // Отримуємо ім'я статусу
                        priorityId: index + 1 // Встановлюємо пріоритет
                    };
                });

            console.log('New order:', newOrder);


            //Оновлення порядку в SQL базі
            $.ajax({
                url: 'Status/UpdatePosition',
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify(newOrder), 
                success: function (response) {
                    /*location.reload();*/
                    console.log('Order updated successfully', response);
                },
                error: function (error) {
                    console.error('Error updating order:', error);
                }
            });
        }
    });
});
