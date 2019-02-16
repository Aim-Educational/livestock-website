using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jaster.Json
{
    /// <summary>
    /// 
    /// </summary>
    public class Json
    {
        /// <summary>
        /// What type of data the json instance stores.
        /// </summary>
        public enum Type
        {
            /// <summary>
            /// A JSON Object, this is essentially the same as a Dictionary, where the key is string, and the value is Json.
            /// </summary>
            Object,

            /// <summary>
            /// A JSON array.
            /// </summary>
            Array,
            
            /// <summary>
            /// A string.
            /// </summary>
            String,

            /// <summary>
            /// A number.
            /// </summary>
            Number,

            /// <summary>
            /// A boolean.
            /// </summary>
            Boolean,

            /// <summary>
            /// A null value.
            /// </summary>
            Null
        }

        #region The different types of data that can be stored.
        internal Dictionary<string, Json> _object;
        internal List<Json>               _array;
        private string                    _string;
        private double                    _number;
        private bool                      _boolean;
        #endregion

        /// <summary>
        /// What type of data this Json instance stores.
        /// </summary>
        public Type type { get; private set; }

        #region Constructors
        private Json(Json.Type type)
        {
            this.type = type;
        }

        /// <summary>
        /// Constructor for strings.
        /// </summary>
        /// <param name="str">The string value</param>
        public Json(string str) : this(Type.String)
        {
            this._string = str;
        }

        /// <summary>
        /// Constructor for objects.
        /// </summary>
        /// <param name="object_">The object to store</param>
        public Json(Dictionary<string, Json> object_) : this(Type.Object)
        {
            this._object = object_;
        }

        /// <summary>
        /// Constructor for arrays.
        /// </summary>
        /// <param name="array">The array to store</param>
        public Json(List<Json> array) : this(Type.Array)
        {
            this._array = array;
        }

        /// <summary>
        /// Constructor for booleans
        /// </summary>
        /// <param name="boolean">The boolean value</param>
        public Json(bool boolean) : this(Type.Boolean)
        {
            this._boolean = boolean;
        }

        /// <summary>
        /// Constructor for numbers
        /// </summary>
        /// <param name="num">The numerical value</param>
        public Json(long num) : this(Type.Number)
        {
            this._number = num;
        }

        /// <summary>
        /// Constructor for numbers
        /// </summary>
        /// <param name="num">The numerical value</param>
        public Json(ulong num) : this(Type.Number)
        {
            this._number = num;
        }

        /// <summary>
        /// Constructor for numbers
        /// </summary>
        /// <param name="num">The numerical value</param>
        public Json(double num) : this(Type.Number)
        {
            this._number = num;
        }
        #endregion

        /// <summary>
        /// Gets a Json value given it's associated key, or a default value if the key isn't found.
        /// </summary>
        /// <exception cref="JsonException">If this Json instance's type isn't Json.Type.Object</exception>
        /// <param name="key">The key of the value to get</param>
        /// <param name="default_">The default value to return if the key isn't found</param>
        /// <returns>If the key is found, then the associated Json value. Otherwise, the default_ value is returned.</returns>
        public Json Get(string key, Json default_ = null)
        {
            this.enforceType(Json.Type.Object);

            Json value;
            return (this._object.TryGetValue(key, out value)) ? value : default_;
        }

        /// <summary>
        /// Gets a Json value given it's index, or a default value if the index is out of bounds.
        /// </summary>
        /// <param name="index">The index of the value to get.</param>
        /// <param name="default_">The default value to return if the index is out of bounds</param>
        /// <returns>If the index is not out of bounds, then the value at that index. Otherwise, the default_ value.</returns>
        public Json Get(int index, Json default_ = null)
        {
            this.enforceType(Json.Type.Array);

            return (index < this._array.Count && index >= 0) ? this._array[index] : default_;
        }

        /// <summary>
        /// Sets a value with a certain key.
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="value">The value</param>
        public void Set(string key, Json value)
        {
            this.enforceType(Json.Type.Object);

            this._object[key] = value;
        }

        /// <summary>
        /// Sets a value at a certain index.
        /// </summary>
        /// <param name="index">The index to set the value at.</param>
        /// <param name="value">The value</param>
        public void Set(int index, Json value)
        {
            this.enforceType(Json.Type.Array);

            this._array[index] = value;
        }

        /// <summary>
        /// Appends a value to the array.
        /// </summary>
        /// <param name="value">The value.</param>
        public void Append(Json value)
        {
            this.enforceType(Json.Type.Array);

            this._array.Add(value);
        }

        /// <summary>
        /// Determines if the Json instance is null.
        /// </summary>
        /// <returns>If the Json instance is null.</returns>
        public bool IsNull()
        {
            return this.type == Json.Type.Null;
        }

        /// <summary>
        /// Parses the given JSON.
        /// </summary>
        /// <param name="json">The JSON to parse</param>
        /// <returns>The parsed JSON as a Json instance</returns>
        public static Json Parse(IEnumerable<char> json)
        {
            // A private class is used so I can easily keep track of state between functions,
            // without having to keep passing them as parameters.
            // Also let's me keep the interface of "JsonParser.Parse"
            var parser = new ParserImpl(json);

            return parser.Parse();
        }

        /// <summary>
        /// Converts the Json instance to a certain value.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the value to convert to.
        /// 
        /// Valid types:
        ///  * Any numerical type.
        ///  * string
        ///  * bool
        /// </typeparam>
        /// <returns>The value as a T</returns>
        public T As<T>()
        {
            dynamic value = null;
            var type = typeof(T);

            if(type == typeof(short))
            {
                this.enforceType(Json.Type.Number);
                value = (short)this._number;
            }
            else if (type == typeof(ushort))
            {
                this.enforceType(Json.Type.Number);
                value = (ushort)this._number;
            }
            else if (type == typeof(int))
            {
                this.enforceType(Json.Type.Number);
                value = (int)this._number;
            }
            else if (type == typeof(uint))
            {
                this.enforceType(Json.Type.Number);
                value = (uint)this._number;
            }
            else if (type == typeof(long))
            {
                this.enforceType(Json.Type.Number);
                value = (long)this._number;
            }
            else if (type == typeof(ulong))
            {
                this.enforceType(Json.Type.Number);
                value = (ulong)this._number;
            }
            else if (type == typeof(float))
            {
                this.enforceType(Json.Type.Number);
                value = (float)this._number;
            }
            else if (type == typeof(double))
            {
                this.enforceType(Json.Type.Number);
                value = (double)this._number;
            }
            else if(type == typeof(bool))
            {
                this.enforceType(Json.Type.Boolean);
                value = this._boolean;
            }
            else if(type == typeof(string))
            {
                this.enforceType(Json.Type.String);
                value = this._string;
            }
            else
            {
                string error;

                if(type == typeof(List<Json>))
                    error = "For Json values that represent arrays (Json.Type.Array), please use Json.Get(int index)";
                else if(type == typeof(Dictionary<string, Json>))
                    error = "For Json values that represent objects (Json.Type.Object), please use Json.Get(string key)";
                else
                    error = $"Unknown type to convert to: '{type.ToString()}'";

                throw new JsonException(error);
            }

            return value;
        }

        /// <returns>An object that can be used in a foreach loop, to go over all of the values in a Json array.</returns>
        public IEnumerable<Json> GetArrayEnumerator()
        {
            this.enforceType(Json.Type.Array);
            return this._array.AsEnumerable();
        }
        
        /// <returns>An object that can be used in a foreach loop to go over all the Key-Value pairs in a Json object.</returns>
        public IEnumerable<KeyValuePair<string, Json>> GetDictionaryEnumerator()
        {
            this.enforceType(Json.Type.Object);
            return this._object.AsEnumerable();
        }

        /// <summary>
        /// Creates an empty Json object
        /// </summary>
        public static Json EmptyObject
        {
            get { return new Json(new Dictionary<string, Json>()); }
        }

        /// <summary>
        /// Creates an empty Json array
        /// </summary>
        public static Json EmptyArray
        {
            get { return new Json(new List<Json>()); }
        }

        /// <summary>
        /// Creates a Json instance representing a null value.
        /// </summary>
        public static Json Null
        {
            get { return new Json(Json.Type.Null); }
        }

        /// <summary>
        /// Converts the Json instance back into a Json string.
        /// </summary>
        /// <param name="compact">If true, then no uneeded whitespace is used, and everything is stored on a single line.</param>
        /// <param name="tabSize">[Only if compact is false] The size of the tabs(made up of spaces) used.</param>
        /// <returns></returns>
        public string ToJsonString(bool compact = false, int tabSize = 4, int _indentCount = 0, bool _skipFirstIndent = false)
        {
            var tabs        = (compact) ? "" : new string(' ', tabSize);
            var indent      = (compact) ? "" : new string(' ', tabSize * _indentCount);
            var newLine     = (compact) ? "" : "\n";
            var space       = (compact) ? "" : " ";
            var firstIndent = (compact || _skipFirstIndent) ? "" : indent;

            switch(this.type)
            {
                case Type.String:
                    return $"{firstIndent}{this._string.ToLiteral()}";

                case Type.Boolean:
                    return $"{firstIndent}{this._boolean.ToString().ToLower()}";

                case Type.Null:
                    return $"{firstIndent}null";

                case Type.Array:
                    var buffer = new StringBuilder();
                    buffer.Append($"{firstIndent}[{newLine}");

                    var values = this.GetArrayEnumerator()
                                     .Select(value => value.ToJsonString(compact, tabSize, _indentCount + 1));
                    var str = string.Join($",{newLine}", values);

                    buffer.Append(str + newLine);
                    buffer.Append($"{indent}]");
                    return buffer.ToString();

                case Type.Number:
                    return $"{firstIndent}{this._number}";

                case Type.Object:
                    var output = new StringBuilder();
                    output.Append($"{firstIndent}{{{newLine}");

                    var formatted = this.GetDictionaryEnumerator()
                                        .Select(kv => $"{indent}{tabs}\"{kv.Key}\":{space}{kv.Value.ToJsonString(compact, tabSize, _indentCount + 1, true)}");
                    output.Append(string.Join($",{newLine}", formatted) + newLine);

                    output.Append($"{indent}}}");
                    return output.ToString();

                default:
                    throw new NotImplementedException();
            }
        }

        private void enforceType(Json.Type type)
        {
            if(this.type != type)
                JsonException.FromInvalidType("Invalid data type", type, this.type);
        }
    }

    internal static class CodeStolenFromStackoverflow
    {
        // I know what it's doing, I was just too lazy to write it myself.
        public static string ToLiteral(this string input)
        {
            StringBuilder literal = new StringBuilder(input.Length + 2);
            literal.Append("\"");
            foreach (var c in input)
            {
                switch (c)
                {
                    case '\'': literal.Append(@"\'"); break;
                    case '\"': literal.Append("\\\""); break;
                    case '\\': literal.Append(@"\\"); break;
                    case '\0': literal.Append(@"\0"); break;
                    case '\a': literal.Append(@"\a"); break;
                    case '\b': literal.Append(@"\b"); break;
                    case '\f': literal.Append(@"\f"); break;
                    case '\n': literal.Append(@"\n"); break;
                    case '\r': literal.Append(@"\r"); break;
                    case '\t': literal.Append(@"\t"); break;
                    case '\v': literal.Append(@"\v"); break;
                    default:
                        // ASCII printable character
                        if (c >= 0x20 && c <= 0x7e)
                        {
                            literal.Append(c);
                            // As UTF16 escaped character
                        }
                        else
                        {
                            literal.Append(@"\u");
                            literal.Append(((int)c).ToString("x4"));
                        }
                        break;
                }
            }
            literal.Append("\"");
            return literal.ToString();
        }
    }
}
