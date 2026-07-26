import { useEffect, useState } from 'react';

//NotificationType -  Define os tipos de notificação suportados.
//success: Ação completada com sucesso (verde).
//error: Erro ou violação de regra de negócio (vermelho).
type NotificationType = 'success' | 'error';

//interface - define as propriedades do componente Notification
//message: string - mensagem a ser exibida
//type: NotificationType - tipo de notificação
//onClose: () => void - função para fechar a notificação
interface NotificationProps {
  message: string;
  type: NotificationType;
  onClose: () => void;
}

//Componente de notificação toast.
//Exibe mensagens temporárias (5s) com animação de entrada/saída.
//Usado para feedback de operações CRUD e erros de regra de negócio.
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

  /** Fecha a notificação manualmente ao clicar no X. */
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
