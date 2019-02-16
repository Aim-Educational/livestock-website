using System;

namespace Jaster.Json
{
    public class JsonException : Exception
    {
        public JsonException(string message) : base(message)
        {
        }

        public static void FromInvalidType(string message, Json.Type got, Json.Type actualType)
        {
            throw new JsonException($"{message}: type wanted '{got}' however current type is '{Convert.ToString(actualType)}'");
        }
    }
}
