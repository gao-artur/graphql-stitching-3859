#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using HotChocolate.Language;
using HotChocolate.Types;
using Newtonsoft.Json;

namespace Remote
{
    public sealed class PropertiesDictionaryType : ScalarType
    {
        public PropertiesDictionaryType() : base("PropertiesDictionary")
        {
        }

        // define which .NET type represents your type
        public override Type RuntimeType => typeof(IDictionary<string, object>);

        // define which literals this type can be parsed from.
        public override bool IsInstanceOfType(IValueNode literal)
        {
            if (literal == null)
            {
                throw new ArgumentNullException(nameof(literal));
            }

            return literal is NullValueNode
                   || literal is ObjectValueNode
                   || literal is ListValueNode;
        }

        // define how a literal is parsed to the native .NET type.
        public override object ParseLiteral(IValueNode valueSyntax)
        {
            if (valueSyntax == null)
            {
                throw new ArgumentNullException(nameof(valueSyntax));
            }

            if (valueSyntax is ObjectValueNode objectLiteral)
            {
                var dictionary = objectLiteral.Fields.ToDictionary(x => x.Name.Value, x => x.Value.Value);
                return dictionary;
            }

            if (valueSyntax is NullValueNode)
            {
                return null;
            }

            throw new ArgumentException(
                "The string type can only parse string literals.",
                nameof(valueSyntax));
        }

        public override IValueNode ParseResult(object? resultValue)
        {
            if (resultValue == null)
            {
                return new NullValueNode(null);
            }

            if (resultValue is IDictionary<string, object> dictionary)
            {
                return new StringValueNode(null, JsonConvert.SerializeObject(dictionary), false);
            }

            throw new ArgumentException(
                "The specified value has to be a string or char in order " +
                "to be parsed by the string type.");
        }

        // define how a native type is parsed into a literal,

        public override IValueNode ParseValue(object? runtimeValue)
        {
            if (runtimeValue == null)
            {
                return new NullValueNode(null);
            }

            if (runtimeValue is IDictionary<string, object> dictionary)
            {
                return new StringValueNode(null, JsonConvert.SerializeObject(dictionary), false);
            }

            throw new ArgumentException(
                "The specified value has to be a string or char in order " +
                "to be parsed by the string type.");
        }

        // define the result serialization. A valid output must be of the following .NET types:
        // System.String, System.Char, System.Int16, System.Int32, System.Int64,
        // System.Float, System.Double, System.Decimal and System.Boolean
        public override bool TrySerialize(object? runtimeValue, out object? resultValue)
        {
            if (runtimeValue == null)
            {
                resultValue = null;
                return true;
            }

            if (runtimeValue is IDictionary<string, object> dictionary)
            {
                resultValue = runtimeValue;
                return true;
            }

            resultValue = null;
            return false;
        }

        public override bool TryDeserialize(object? resultValue, out object? runtimeValue)
        {
            if (resultValue is null)
            {
                runtimeValue = null;
                return true;
            }

            if (resultValue is IDictionary<string, object> dictionary)
            {
                runtimeValue = dictionary;
                return true;
            }

            runtimeValue = null;
            return false;
        }
    }
}