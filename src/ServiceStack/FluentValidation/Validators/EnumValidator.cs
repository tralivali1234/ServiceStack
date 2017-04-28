﻿#region License
// Copyright (c) Jeremy Skinner (http://www.jeremyskinner.co.uk)
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// The latest version of this file can be found at https://github.com/jeremyskinner/FluentValidation
#endregion

namespace ServiceStack.FluentValidation.Validators
{
	using System;
	using System.Reflection;
	using Resources;

	public class EnumValidator : PropertyValidator
	{
		private readonly Type enumType;

		public EnumValidator(Type enumType) : base(nameof(Messages.enum_error), typeof(Messages)) {
		    ErrorCodeSource = new StaticStringSource(ValidationErrors.Enum);
			this.enumType = enumType;
		}

		protected override bool IsValid(PropertyValidatorContext context) {
			if (context.PropertyValue == null) return true;

			var underlyingEnumType = Nullable.GetUnderlyingType(enumType) ?? enumType;

			if (!underlyingEnumType.GetTypeInfo().IsEnum) return false;

			if (underlyingEnumType.GetTypeInfo().GetCustomAttribute<FlagsAttribute>() != null)
			{
				return IsFlagsEnumDefined(underlyingEnumType, context.PropertyValue);
			}

			return Enum.IsDefined(underlyingEnumType, context.PropertyValue);
		}

		private static bool IsFlagsEnumDefined(Type enumType, object value) {
			var typeName = Enum.GetUnderlyingType(enumType).Name;

			switch (typeName)
			{
				case "Byte":
					{
						var typedValue = (byte)value;
						return EvaluateFlagEnumValues(typedValue, enumType);
					}

				case "Int16":
					{
						var typedValue = (short)value;

						if (typedValue < 0)
						{
							return false;
						}

						return EvaluateFlagEnumValues(Convert.ToUInt64(typedValue), enumType);
					}

				case "Int32":
					{
						var typedValue = (int)value;

						if (typedValue < 0)
						{
							return false;
						}

						return EvaluateFlagEnumValues(Convert.ToUInt64(typedValue), enumType);
					}

				case "Int64":
					{
						var typedValue = (long)value;

						if (typedValue < 0)
						{
							return false;
						}

						return EvaluateFlagEnumValues(Convert.ToUInt64(typedValue), enumType);
					}

				case "SByte":
					{
						var typedValue = (sbyte)value;

						if (typedValue < 0)
						{
							return false;
						}

						return EvaluateFlagEnumValues(Convert.ToUInt64(typedValue), enumType);
					}

				case "UInt16":
					{
						var typedValue = (ushort)value;
						return EvaluateFlagEnumValues(typedValue, enumType);
					}

				case "UInt32":
					{
						var typedValue = (uint)value;
						return EvaluateFlagEnumValues(typedValue, enumType);
					}

				case "UInt64":
					{
						var typedValue = (ulong)value;
						return EvaluateFlagEnumValues(typedValue, enumType);
					}

				default:
					var message = string.Format("Unexpected typeName of '{0}' during flags enum evaluation.", typeName);
					throw new ArgumentOutOfRangeException("typeName", message);
			}
		}

		private static bool EvaluateFlagEnumValues(ulong value, Type enumType) {
			ulong mask = 0;

			foreach (var enumValue in Enum.GetValues(enumType)) {
				var enumValueAsUInt64 = Convert.ToUInt64(enumValue);
				mask = mask | enumValueAsUInt64;
			}

			return (mask & value) == value;
		}
	}
}