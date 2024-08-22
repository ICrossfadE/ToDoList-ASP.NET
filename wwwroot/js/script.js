const addButtont = document.getElementById('add-button');
const cancelButtont = document.getElementById('cancel-button');
const createButtont = document.getElementById('create-button');
const dialog = document.getElementById('default-modal');
const input = document.getElementById('form-input');
const textarea = document.getElementById('form-input');

addButtont.addEventListener('click', (event) => {
    event.preventDefault();

    dialog.showModal();
});


cancelButtont.addEventListener('click', (event) => {
    event.preventDefault();

    dialog.close();
    input.value = '';
    textarea.value = '';
    createButtont.textContent = "Create Todo";
    createButtont.classList.remove("bg-cyan-600");
    createButtont.classList.remove("hover:bg-cyan-700");
    createButtont.classList.add("bg-green-500");
    createButtont.classList.add("hover:bg-green-600");
    
});

createButtont.addEventListener('click', (event) => {
    event.preventDefault();

    dialog.close();
    input.value = '';
    textarea.value = '';
    createButtont.textContent = "Create Todo";
    createButtont.classList.remove("bg-cyan-600");
    createButtont.classList.remove("hover:bg-cyan-700");
    createButtont.classList.add("bg-green-500");
    createButtont.classList.add("hover:bg-green-600");
})
