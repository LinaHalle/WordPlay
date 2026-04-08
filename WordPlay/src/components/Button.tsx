type Props = {
  children: React.ReactNode;
  className?: string;
};

export default function Button({ children, className = "" }: Props) {
  return <button className={`btn ${className}`}>{children}</button>;
}