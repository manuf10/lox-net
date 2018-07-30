using System.Collections.Generic;
using LoxNet.Interpreter.Extensions;

namespace LoxNet.Interpreter.Components
{
    class Scanner
    {
        private readonly string _source;
        private int _start = 0;
        private int _current = 0;
        private int _line = 1;
        private readonly List<Token> _tokens = new List<Token>();
        private bool IsAtEnd() => _current == _source.Length;
        
        public Scanner(string source)
        {
            _source = source;
        }

        public List<Token> ScanTokens()
        {
            while (!IsAtEnd())
            {
                _start = _current;
                ScanToken();
            }
            _tokens.Add(new Token(TokenType.EOF, "", null, _line));
            return _tokens;
        }

        private void ScanToken()
        {
            var character = Advance();
            switch (character) 
            {
                case '(': AddToken(TokenType.LEFT_PAREN); break;
                case ')': AddToken(TokenType.RIGHT_PAREN); break;
                case '{': AddToken(TokenType.LEFT_BRACE); break;
                case '}': AddToken(TokenType.RIGHT_BRACE); break;
                case ',': AddToken(TokenType.COMMA); break;
                case '.': AddToken(TokenType.DOT); break;
                case '-': AddToken(TokenType.MINUS); break;
                case '+': AddToken(TokenType.PLUS); break;
                case ';': AddToken(TokenType.SEMICOLON); break;
                case '*': AddToken(TokenType.STAR); break; 
                case '!': AddToken(Match('=') ? TokenType.BANG_EQUAL : TokenType.BANG); break;
                case '=': AddToken(Match('=') ? TokenType.EQUAL_EQUAL : TokenType.EQUAL); break;
                case '<': AddToken(Match('=') ? TokenType.LESS_EQUAL : TokenType.LESS); break;
                case '>': AddToken(Match('=') ? TokenType.GREATER_EQUAL : TokenType.GREATER); break;
                case '/':
                    //Handle commented lines
                    if (Match('/'))
                        while (Peek() != '\n' && !IsAtEnd()) Advance();
                    else 
                        AddToken(TokenType.SLASH);
                    break;
                //Ignore whitespace
                case ' ':
                case '\r':
                case '\t':
                    break;
                case '\n':
                    _line++;
                    break;
                //String literals
                case '"': MatchString(); break;
                case 'o':
                    if (Peek() == 'r')
                        AddToken(TokenType.OR);
                    break;
                default: 
                    if (char.IsDigit(character)) 
                        MatchNumber();
                    else if (char.IsLetterOrDigit(character))
                        MatchIdentifier();
                    else
                        Lox.ReportError(_line, "Unexpected character.");
                    break;
            }
        }

        private char Advance() 
        {
            _current++;
            return _source[_current - 1];
        }

        private void AddToken(TokenType type)
        {
            AddToken(type, null);
        }

        private void AddToken(TokenType type, object literal) 
        {
            var text = _source.Slice(_start, _current);
            _tokens.Add(new Token(type, text, literal, _line));
        }

        //Lookahead next character
        private bool Match(char expected) 
        {
            if (IsAtEnd()) 
                return false;
            
            if (_source[_current] != expected) 
                return false;
            
            _current++;
            return true;
        }

        private char Peek()
        {
            if (IsAtEnd()) return '\0';
            return _source[_current];
        }

        private char PeekNext()
        {
            if (_current + 1 >= _source.Length) return '\0';
            return _source[_current + 1];
        }

        //Note: Lox supports multi-line strings
        private void MatchString()
        {
            while (Peek() != '"' && !IsAtEnd())
            {
                if (Peek() == '\n')
                    _line++;
                Advance();
            }

            if (IsAtEnd())
                Lox.ReportError(_line, "Unterminated string.");

            Advance();

            var value = _source.Slice(_start + 1, _current - 1);
            AddToken(TokenType.STRING, value);
        }

        private void MatchNumber()
        {
            while (char.IsDigit(Peek())) Advance();
            
            if (Peek() == '.' && char.IsDigit(PeekNext()))
            {
                Advance();
                while(char.IsDigit(Peek())) 
                    Advance();
            }

            var value = _source.Slice(_start - 1, _current - 1);
            AddToken(TokenType.NUMBER, value);
        }

        private void MatchIdentifier()
        {
            while (char.IsLetterOrDigit(Peek()))
                Advance();

            var text = _source.Slice(_start, _current);
            if (_keywords.ContainsKey(text))
                AddToken(_keywords[text]);
            else
                AddToken(TokenType.IDENTIFIER);
        }

        private static readonly Dictionary<string, TokenType> _keywords = new Dictionary<string, TokenType>()
        {
            { "and",    TokenType.AND },
            { "class",  TokenType.CLASS },
            { "else",   TokenType.ELSE },
            { "false",  TokenType.FALSE },
            { "for",    TokenType.FOR },
            { "fun",    TokenType.FUN },
            { "if",     TokenType.IF },
            { "nil",    TokenType.NIL },
            { "or",     TokenType.OR },
            { "print",  TokenType.PRINT },
            { "return", TokenType.RETURN },
            { "super",  TokenType.SUPER },
            { "this",   TokenType.THIS },
            { "true",   TokenType.TRUE },
            { "var",    TokenType.VAR },
            { "while",  TokenType.WHILE }
        };
    }
}