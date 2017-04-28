#region License
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

namespace ServiceStack.FluentValidation.Internal
{
	using System;
	using System.Collections.Generic;
	using System.Linq.Expressions;
	using System.Reflection;
	using System.Text;
	using System.Threading.Tasks;

	/// <summary>
	/// Useful extensions
	/// </summary>
	public static class Extensions {
		internal static void Guard(this object obj, string message) {
			if (obj == null) {
				throw new ArgumentNullException(message);
			}
		}

		internal static void Guard(this string str, string message) {
			if (string.IsNullOrEmpty(str)) {
				throw new ArgumentNullException(message);
			}
		}

		/// <summary>
		/// Checks if the expression is a parameter expression
		/// </summary>
		/// <param name="expression"></param>
		/// <returns></returns>
		public static bool IsParameterExpression(this LambdaExpression expression) {
			return expression.Body.NodeType == ExpressionType.Parameter;
		}

		/// <summary>
		/// Gets a MemberInfo from a member expression.
		/// </summary>
		public static MemberInfo GetMember(this LambdaExpression expression) {
			var memberExp = RemoveUnary(expression.Body);

			if (memberExp == null) {
				return null;
			}

			return memberExp.Member;
		}

		/// <summary>
		/// Gets a MemberInfo from a member expression.
		/// </summary>
		public static MemberInfo GetMember<T, TProperty>(this Expression<Func<T, TProperty>> expression) {
			var memberExp = RemoveUnary(expression.Body);

			if (memberExp == null) {
				return null;
			}

			return memberExp.Member;
		}

		private static MemberExpression RemoveUnary(Expression toUnwrap) {
			if (toUnwrap is UnaryExpression) {
				return ((UnaryExpression)toUnwrap).Operand as MemberExpression;
			}

			return toUnwrap as MemberExpression;
		}

		/// <summary>
		/// Splits pascal case, so "FooBar" would become "Foo Bar"
		/// </summary>
		public static string SplitPascalCase(this string input)
		{
			if (string.IsNullOrEmpty(input)) {
				return input;
			}

			var retVal = new StringBuilder(input.Length + 5);

			foreach (var currentChar in input)
			{
				if (char.IsUpper(currentChar))
				{
					retVal.Append(' ');
					retVal.Append(currentChar);				
				}
				else
				{
					retVal.Append(currentChar);
				}
			}

			return retVal.ToString().Trim();
		}

		/// <summary>
		/// Helper method to construct a constant expression from a constant.
		/// </summary>
		/// <typeparam name="T">Type of object being validated</typeparam>
		/// <typeparam name="TProperty">Type of property being validated</typeparam>
		/// <param name="valueToCompare">The value being compared</param>
		/// <returns></returns>
		internal static Expression<Func<T, TProperty>> GetConstantExpresionFromConstant<T, TProperty>(TProperty valueToCompare) {
			Expression constant = Expression.Constant(valueToCompare, typeof(TProperty));
			ParameterExpression parameter = Expression.Parameter(typeof(T), "t");
			return Expression.Lambda<Func<T, TProperty>>(constant, parameter);
		}

		internal static void ForEach<T>(this IEnumerable<T> source, Action<T> action) {
			foreach(var item in source) {
				action(item);
			}
		}

#pragma warning disable 1591
		public static Func<object, object> CoerceToNonGeneric<T, TProperty>(this Func<T, TProperty> func) {
			return x => func((T)x);
		} 

		public static Func<object, bool> CoerceToNonGeneric<T>(this Func<T, bool> func) {
			return x => func((T)x);
		}

		public static Func<object, Task<bool>> CoerceToNonGeneric<T>(this Func<T, Task<bool>> func)
		{
			return x => func((T)x);
		}

		public static Func<object, int> CoerceToNonGeneric<T>(this Func<T, int> func)
		{
			return x => func((T)x);
		}

		public static Func<object, long> CoerceToNonGeneric<T>(this Func<T, long> func)
		{
			return x => func((T)x);
		}

		public static Func<object, string> CoerceToNonGeneric<T>(this Func<T, string> func)
		{
			return x => func((T)x);
		}

		public static Func<object, System.Text.RegularExpressions.Regex> CoerceToNonGeneric<T>(this Func<T, System.Text.RegularExpressions.Regex> func)
		{
			return x => func((T)x);
		}

		public static Action<object> CoerceToNonGeneric<T>(this Action<T> action) {
			return x => action((T)x);
		}
#pragma warning restore 1591
	}
}