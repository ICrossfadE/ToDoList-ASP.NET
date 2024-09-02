const addTodo = document.getElementById('add-todo');
const cancelButtont = document.getElementById('cancel-button');
const createButtont = document.getElementById('create-button');
const dialogTodo = document.getElementById('default-modal-todo');
const input = document.getElementById('form-input');
const textarea = document.getElementById('form-input');

// Todo
addTodo.addEventListener('click', (event) => {
    event.preventDefault();
    dialogTodo.showModal();
});

// Cancel Todo
cancelButtont.addEventListener('click', (event) => {
    event.preventDefault();
    dialogTodo.close();
    input.value = '';
    textarea.value = '';
    createButtont.textContent = "Create Todo";
    createButtont.classList.remove("bg-cyan-600");
    createButtont.classList.remove("hover:bg-cyan-700");
    createButtont.classList.add("bg-green-500");
    createButtont.classList.add("hover:bg-green-600");
});

// Create Todo
createButtont.addEventListener('click', (event) => {
    event.preventDefault();
    dialogTodo.close();
    input.value = '';
    textarea.value = '';
    createButtont.textContent = "Create Todo";
    createButtont.classList.remove("bg-cyan-600");
    createButtont.classList.remove("hover:bg-cyan-700");
    createButtont.classList.add("bg-green-500");
    createButtont.classList.add("hover:bg-green-600");
});
