const addStatus = document.getElementById('add-status');
const dialogStatus = document.getElementById('default-modal-status');
const cancelStatusButtont = document.getElementById('cancel-status-button');
const createStatusButtont = document.getElementById('create-status-button');
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