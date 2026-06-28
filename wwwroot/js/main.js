/*AddressManager — main.js*/

'use strict';

// ── CEP auto-fill ───────────────────────────────────────────────────────────

function initCepLookup() {
  const cepInput = document.getElementById('Cep');
  if (!cepInput) return;

  const btn = document.getElementById('btnBuscarCep');
  const loader = document.getElementById('cepLoader');

  async function buscarCep() {
    const cep = cepInput.value.replace(/\D/g, '');
    if (cep.length !== 8) {
      showToast('Digite um CEP com 8 dígitos.', 'warning');
      return;
    }

    if (loader) loader.style.display = 'inline-block';
    if (btn) btn.disabled = true;

    try {
      const res = await fetch(`/Address/BuscarCep?cep=${cep}`);
      if (!res.ok) {
        showToast('CEP não encontrado. Preencha manualmente.', 'danger');
        return;
      }

      const data = await res.json();
      setField('Logradouro', data.logradouro);
      setField('Bairro', data.bairro);
      setField('Cidade', data.cidade);
      setField('Uf', data.uf.toUpperCase());
      if (data.complemento) setField('Complemento', data.complemento);

      // Focus numero for user to fill in
      const numInput = document.getElementById('Numero');
      if (numInput) numInput.focus();

      showToast('Endereço preenchido com sucesso!', 'success');
    } catch {
      showToast('Erro ao consultar CEP. Verifique sua conexão.', 'danger');
    } finally {
      if (loader) loader.style.display = 'none';
      if (btn) btn.disabled = false;
    }
  }

  if (btn) btn.addEventListener('click', buscarCep);

  // Also trigger on Enter key inside CEP input
  cepInput.addEventListener('keydown', (e) => {
    if (e.key === 'Enter') {
      e.preventDefault();
      buscarCep();
    }
  });

  // Auto-format CEP mask
  cepInput.addEventListener('input', () => {
    cepInput.value = cepInput.value.replace(/\D/g, '').slice(0, 8);
  });
}

function setField(id, value) {
  const el = document.getElementById(id);
  if (el && value) el.value = value;
}

// ── Delete confirmation ─────────────────────────────────────────────────────

function initDeleteConfirm() {
  document.querySelectorAll('[data-confirm]').forEach(btn => {
    btn.addEventListener('click', function (e) {
      const msg = this.dataset.confirm || 'Confirma esta ação?';
      if (!confirm(msg)) e.preventDefault();
    });
  });
}

// ── Auto-dismiss alerts ─────────────────────────────────────────────────────

function initAlerts() {
  document.querySelectorAll('.alert[data-auto-dismiss]').forEach(el => {
    const delay = parseInt(el.dataset.autoDismiss, 10) || 4000;
    setTimeout(() => {
      el.style.transition = 'opacity 0.5s';
      el.style.opacity = '0';
      setTimeout(() => el.remove(), 500);
    }, delay);
  });
}

// ── Toast notification ──────────────────────────────────────────────────────

function showToast(message, type = 'info') {
  const existing = document.getElementById('toast-container');
  if (existing) existing.remove();

  const colors = {
    success: '#00e676',
    danger:  '#ff3d6b',
    warning: '#ffab00',
    info:    '#00e5ff',
  };

  const container = document.createElement('div');
  container.id = 'toast-container';
  container.style.cssText = `
    position: fixed;
    bottom: 1.5rem;
    right: 1.5rem;
    z-index: 9999;
    display: flex;
    flex-direction: column;
    gap: 0.5rem;
  `;

  const toast = document.createElement('div');
  toast.style.cssText = `
    background: #0a1628;
    border: 1px solid ${colors[type]};
    color: ${colors[type]};
    padding: 0.75rem 1.2rem;
    border-radius: 6px;
    font-size: 0.875rem;
    font-family: Inter, sans-serif;
    box-shadow: 0 4px 24px rgba(0,0,0,0.5), 0 0 12px ${colors[type]}33;
    animation: slideIn 0.25s ease;
    max-width: 320px;
  `;
  toast.textContent = message;

  const style = document.createElement('style');
  style.textContent = `
    @keyframes slideIn {
      from { transform: translateX(100%); opacity: 0; }
      to   { transform: translateX(0);   opacity: 1; }
    }
  `;
  document.head.appendChild(style);

  container.appendChild(toast);
  document.body.appendChild(container);

  setTimeout(() => {
    toast.style.transition = 'opacity 0.4s';
    toast.style.opacity = '0';
    setTimeout(() => container.remove(), 400);
  }, 3500);
}

// ── UF uppercase ────────────────────────────────────────────────────────────

function initUfInput() {
  const uf = document.getElementById('Uf');
  if (uf) {
    uf.addEventListener('input', () => {
      uf.value = uf.value.toUpperCase().slice(0, 2);
    });
  }
}

// ── Init ────────────────────────────────────────────────────────────────────

document.addEventListener('DOMContentLoaded', () => {
  initCepLookup();
  initDeleteConfirm();
  initAlerts();
  initUfInput();
});

// ── Modal for delete confirmation ───────────────────────────────────────────

function abrirModal(id) {
  const modal = document.getElementById('modalExcluir');
  const form  = document.getElementById('formExcluir');
  form.action = `/Address/Delete/${id}`;
  modal.style.display = 'flex';
}

function fecharModal() {
  document.getElementById('modalExcluir').style.display = 'none';
}

// Fecha ao clicar fora do modal
document.getElementById('modalExcluir')?.addEventListener('click', function(e) {
  if (e.target === this) fecharModal();
});