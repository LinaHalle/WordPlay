namespace Brainfart.Services;

public static class LetterGenerator
{
  private static readonly Random _rnd = new();
  private static readonly string Letters = "ABCDEFGHIJKLMNOPQRSTUVW";

  public static string RandomLetter() =>
      Letters[_rnd.Next(Letters.Length)].ToString();
}
