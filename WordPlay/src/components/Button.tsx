type Props = {
  children: React.ReactNode;
  className?: string;
  onClick?: () => void;
  disabled?: boolean;
};

export default function Button({ children, className = "", onClick, disabled }: Props) {
  return (
    <button className={`btn ${className}`} onClick={onClick} disabled={disabled}>
      {children}
    </button>
  );
}
