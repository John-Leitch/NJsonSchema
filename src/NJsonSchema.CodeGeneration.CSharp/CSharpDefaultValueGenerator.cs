//-----------------------------------------------------------------------
// <copyright file="CSharpDefaultValueGenerator.cs" company="NJsonSchema">
//     Copyright (c) Rico Suter. All rights reserved.
// </copyright>
// <license>https://github.com/rsuter/NJsonSchema/blob/master/LICENSE.md</license>
// <author>Rico Suter, mail@rsuter.com</author>
//-----------------------------------------------------------------------

using System.Globalization;

namespace NJsonSchema.CodeGeneration.CSharp
{
    /// <summary>Converts the default value to a TypeScript identifier.</summary>
    public class CSharpDefaultValueGenerator : DefaultValueGenerator
    {
        private readonly CSharpGeneratorSettings _settings;

        /// <summary>Initializes a new instance of the <see cref="CSharpDefaultValueGenerator" /> class.</summary>
        /// <param name="typeResolver">The type resolver.</param>
        /// <param name="settings">The settings.</param>
        public CSharpDefaultValueGenerator(ITypeResolver typeResolver, CSharpGeneratorSettings settings) 
            : base(typeResolver, settings.EnumNameGenerator)
        {
            _settings = settings;
        }

        /// <summary>Gets the default value code.</summary>
        /// <param name="schema">The schema.</param>
        /// <param name="allowsNull">Specifies whether the default value assignment also allows null.</param>
        /// <param name="targetType">The type of the target.</param>
        /// <param name="typeNameHint">The type name hint to use when generating the type and the type name is missing.</param>
        /// <param name="useSchemaDefault">if set to <c>true</c> uses the default value from the schema if available.</param>
        /// <returns>The code.</returns>
        public override string GetDefaultValue(JsonSchema4 schema, bool allowsNull, string targetType, string typeNameHint, bool useSchemaDefault)
        {
            var value = base.GetDefaultValue(schema, allowsNull, targetType, typeNameHint, useSchemaDefault);
            if (value == null)
            {
                schema = schema.ActualSchema;
                if (schema != null && allowsNull == false)
                {
                    if (schema.Type.HasFlag(JsonObjectType.Array) ||
                        schema.Type.HasFlag(JsonObjectType.Object))
                        return "new " + targetType + "()";
                }
            }
            return value;
        }

        /// <summary>Converts the default value to a C# number literal. </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The C# number literal.</returns>
        protected override string ConvertNumericValue(object value)
        {
            if (value is byte) return "(byte)" + ((byte)value).ToString(CultureInfo.InvariantCulture);
            if (value is sbyte) return "(sbyte)" + ((sbyte)value).ToString(CultureInfo.InvariantCulture);
            if (value is short) return "(short)" + ((short)value).ToString(CultureInfo.InvariantCulture);
            if (value is ushort) return "(ushort)" + ((ushort)value).ToString(CultureInfo.InvariantCulture);
            if (value is int) return ((int)value).ToString(CultureInfo.InvariantCulture);
            if (value is uint) return ((uint)value).ToString(CultureInfo.InvariantCulture) + "U";
            if (value is long) return ((long)value).ToString(CultureInfo.InvariantCulture) + "L";
            if (value is ulong) return ((ulong)value).ToString(CultureInfo.InvariantCulture) + "UL";
            if (value is float) return ((float)value).ToString("r", CultureInfo.InvariantCulture) + "F";
            if (value is double) return ((double)value).ToString("r", CultureInfo.InvariantCulture) + "D";
            if (value is decimal) return ((decimal)value).ToString(CultureInfo.InvariantCulture) + "M";
            return null;
        }

        /// <summary>Gets the enum default value.</summary>
        /// <param name="schema">The schema.</param>
        /// <param name="actualSchema">The actual schema.</param>
        /// <param name="typeNameHint">The type name hint.</param>
        /// <returns>The enum default value.</returns>
        protected override string GetEnumDefaultValue(JsonSchema4 schema, JsonSchema4 actualSchema, string typeNameHint)
        {
            return _settings.Namespace + "." + base.GetEnumDefaultValue(schema, actualSchema, typeNameHint);
        }
    }
}