const addButtont = document.getElementById('add-button');
const cancelButtont = document.getElementById('cancel-button');
const createButtont = document.getElementById('create-button');
const modalWindow = document.getElementById('default-modal')

addButtont.addEventListener('click', (event) => {
    event.preventDefault();

    showModalWindow(modalWindow);
})

cancelButtont.addEventListener('click', (event) => {
    event.preventDefault();

    hideModalWindow(modalWindow);
})

createButtont.addEventListener('click', (event) => {
    event.preventDefault();

    hideModalWindow(modalWindow);
})


function showModalWindow(elem) {
    elem.classList.remove('hidden');
    elem.classList.add('active');
    console.log("show");
}
function hideModalWindow(elem) {
    elem.classList.remove('active');
    elem.classList.add('hidden');
    console.log("hide");
}