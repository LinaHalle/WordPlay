type Props = {
  children: React.ReactNode;
};

export default function Button({ children }: Props) {
  return <button className="btn">{children}</button>;
}