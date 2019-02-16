using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jaster.Json
{
    internal class ParserImpl
    {
        ulong line { get; set; }
        ulong column { get; set; }
        int index { get; set; }

        IEnumerable<char> json { get; set; }

        public ParserImpl(IEnumerable<char> json)
        {
            this.json = json;
        }

        char NextChar(bool peek = false)
        {
            if (this.index >= this.json.Count())
                throw new JsonException("Unepected End of Line");

            var next = this.json.ElementAt(this.index);
            if (!peek)
            {
                this.index++;
                this.column++;

                if (next == '\n')
                {
                    this.line++;
                    this.column = 0;
                }
            }

            return next;
        }

        void EatWhitespace()
        {
            while (char.IsWhiteSpace(this.NextChar(peek: true)))
            {
                this.NextChar();
            }
        }

        public Json Parse()
        {
            this.EatWhitespace();

            var next = this.NextChar(peek: true);
            if (char.IsDigit(next) || next == '-')
                return this.ParseNumber();

            switch (next)
            {
                case '{':
                    return this.ParseObject();

                case '[':
                    return this.ParseArray();

                case '"':
                    return this.ParseString();

                case 't':
                case 'f':
                    return this.ParseBoolean();

                case 'n':
                    return this.ParseNull();

                default:
                    throw new JsonException($"Unexpected character '{next}' at line {this.line} column {this.column}");
            }

            throw new Exception("This exception should never be thrown");
        }

        Json ParseObject()
        {
            this.EatWhitespace();
            Json obj = Json.EmptyObject;

            if (this.NextChar() != '{')
                throw new JsonException("Expected '{' when parsing object");

            while (true)
            {
                this.EatWhitespace();
                var next = this.NextChar(peek: true);
                if (next == '}')
                {
                    this.NextChar();
                    break;
                }

                var name = this.ParseString().As<string>();

                this.EatWhitespace();
                if (this.NextChar() != ':')
                    throw new JsonException($"[Line {this.line} column {this.column}]Expected ':' following value name while parsing object");

                var value = this.Parse();
                obj.Set(name, value);

                this.EatWhitespace();
                next = this.NextChar();
                if (next == '}')
                    break;
                else if (next == ',')
                    continue;
                else
                    throw new JsonException($"[Line {this.line} column {this.column}]Expected '}}' or ',', not '{next}' when parsing object");
            }

            return obj;
        }

        Json ParseArray()
        {
            this.EatWhitespace();
            Json array = Json.EmptyArray;

            if (this.NextChar() != '[')
                throw new JsonException("Expected '[' when parsing array");

            while (true)
            {
                this.EatWhitespace();
                var next = this.NextChar(peek: true);
                if (next == ']')
                {
                    this.NextChar();
                    break;
                }

                array.Append(this.Parse());

                this.EatWhitespace();
                next = this.NextChar();
                if (next == ']')
                    break;
                else if (next == ',')
                    continue;
                else
                    throw new JsonException($"Expected ']' or ',', not '{next}' when parsing array of values");
            }

            return array;
        }

        Json ParseString()
        {
            this.EatWhitespace();
            var buffer = new StringBuilder();

            if (this.NextChar() != '"')
                throw new JsonException("Expected '\"' when parsing string");

            while (true)
            {
                var next = this.NextChar();
                if (next == '"')
                    break;

                if (next == '\\')
                {
                    next = this.NextChar();

                    switch (next)
                    {
                        case 'n':
                            next = '\n';
                            break;

                        case 'r':
                            next = '\r';
                            break;

                        case '/':
                            next = '/';
                            break;

                        case '\\':
                            next = '\\';
                            break;

                        default:
                            throw new JsonException($"[Line {this.line} column {this.column}] Unrecognised escape character '{next}'");
                    }
                }

                buffer.Append(next);
            }

            return new Json(buffer.ToString());
        }

        Json ParseBoolean()
        {
            this.EatWhitespace();
            const string stringTrue = "true";
            const string stringFalse = "false";

            var next = this.NextChar(peek: true);
            int length = 0;
            string str = null;

            if (next == 't')
            {
                length = stringTrue.Length;
                str = stringTrue;
            }
            else if (next == 'f')
            {
                length = stringFalse.Length;
                str = stringFalse;
            }
            else throw new JsonException($"[Line {this.line} column {this.column}] Unexpected character '{next}' when parsing boolean");

            for (int i = 0; i < length; i++)
            {
                next = this.NextChar();
                if (next != str[i])
                    throw new JsonException($"[Line {this.line} column {this.column}] Unexpected character '{next}' when parsing boolean");
            }

            return (str == stringTrue) ? new Json(true) : new Json(false);
        }

        Json ParseNumber()
        {
            this.EatWhitespace();
            var buffer = new StringBuilder(capacity: 8);

            while (true)
            {
                var next = this.NextChar(peek: true);
                if (next == ',' || char.IsWhiteSpace(next) || next == '}' || next == ']')
                {
                    if (next != ',')
                        this.NextChar();

                    break;
                }

                if (next != '.' && next != 'e' && !char.IsDigit(next) && next != '-')
                    throw new JsonException($"[Line {this.line} column {this.column}] Unexpected '{next}' when parsing number");

                buffer.Append(this.NextChar());
            }

            try
            {
                return new Json(Convert.ToDouble(buffer.ToString()));
            }
            catch (Exception ex)
            {
                throw new JsonException("[Please report this] Unable to convert properly parsed number to double: " + ex.Message);
            }
        }

        Json ParseNull()
        {
            this.EatWhitespace();
            const string expected = "null";

            for (int i = 0; i < expected.Length; i++)
            {
                var next = this.NextChar();
                if (expected[i] != next)
                    throw new Exception($"[Line {this.line} column {this.column}] Unexpected '{next}' when attempting to read 'null' value");
            }

            return Json.Null;
        }
    }
}
