const addStatus = document.getElementById('add-status');
const dialogStatus = document.getElementById('default-modal-status');
const cancelStatusButtont = document.getElementById('cancel-status-button');
const createStatusButtont = document.getElementById('create-button');
const statusInput = document.getElementById('form-status-input');

// Status
addStatus.addEventListener('click', (event) => {
  event.preventDefault();

  dialogStatus.showModal();
});

// Cancel modal
cancelStatusButtont.addEventListener('click', (event) => {
  event.preventDefault();

  dialogStatus.close();
  statusInput.value = '';
  // createButtont.textContent = "Create Todo";
  // createButtont.classList.remove("bg-cyan-600");
  // createButtont.classList.remove("hover:bg-cyan-700");
  // createButtont.classList.add("bg-green-500");
  // createButtont.classList.add("hover:bg-green-600");
});