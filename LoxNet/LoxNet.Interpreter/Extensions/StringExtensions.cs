namespace LoxNet.Interpreter.Extensions
{
    public static class Extensions
    {
        public static string Slice(this string source, int start, int end)
        {
            if (end < 0) end = source.Length + end;
            int len = end - start;
            return source.Substring(start, len);
        }
    }
}