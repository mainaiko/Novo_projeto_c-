import { useEffect, useState } from 'react';

// Tipos de notificação suportados.
// success: operação concluída (verde). error: falha ou violação de regra (vermelho).
type NotificationType = 'success' | 'error';

// Props do componente Notification.
// message: texto exibido. type: estilo visual. onClose: callback de fechamento.
interface NotificationProps {
  message: string;
  type: NotificationType;
  onClose: () => void;
}

// Componente de notificação toast com auto-dismiss (5s) e animação de entrada/saída.
// Usado para feedback de operações CRUD e erros de validação.
export default function Notification({ message, type, onClose }: NotificationProps) {
  const [isExiting, setIsExiting] = useState(false);

  useEffect(() => {
    // Auto-dismiss após 5 segundos
    const timer = setTimeout(() => {
      setIsExiting(true);
      setTimeout(onClose, 300); // Espera a animação de saída terminar
    }, 5000);

    return () => clearTimeout(timer);
  }, [onClose]);

  // Fecha a notificação manualmente ao clicar no botão X.
  const handleClose = () => {
    setIsExiting(true);
    setTimeout(onClose, 300);
  };

  return (
    <div className={`notification notification--${type} ${isExiting ? 'notification--exit' : ''}`}>
      <div className="notification__icon">
        {type === 'success' ? '✓' : '✕'}
      </div>
      <p className="notification__message">{message}</p>
      <button
        className="notification__close"
        onClick={handleClose}
        aria-label="Fechar notificação"
      >
        ×
      </button>
    </div>
  );
}
