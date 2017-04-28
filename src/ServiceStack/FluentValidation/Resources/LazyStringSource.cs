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
// The latest version of this file can be found at http://fluentvalidation.codeplex.com
#endregion
namespace ServiceStack.FluentValidation.Resources {
	using System;

	/// <summary>
	/// Lazily loads the string
	/// </summary>
	public class LazyStringSource : IStringSource {
		readonly Func<object, string> _stringProvider;

		/// <summary>
		/// Creates a new LazyStringSource
		/// </summary>
		/// <param name="stringProvider"></param>
		[Obsolete("Use constructor that takes a Func<object, string>")]
		public LazyStringSource(Func<string> stringProvider) {
			_stringProvider = x => stringProvider();
		}

		/// <summary>
		/// Creates a LazyStringSource
		/// </summary>
		public LazyStringSource(Func<object, string> stringProvider) {
			_stringProvider = stringProvider;
		}

		/// <summary>
		/// Gets the value
		/// </summary>
		/// <returns></returns>
		public string GetString(object context) {
			try {
				return _stringProvider(context);
			}
			catch (NullReferenceException ex) {
				throw new FluentValidationMessageFormatException("Could not build error message- the message makes use of properties from the containing object, but the containing object was null.", ex);
			}
		}

		/// <summary>
		/// Resource type
		/// </summary>
		public string ResourceName { get { return null; } }

		/// <summary>
		/// Resource name
		/// </summary>
		public Type ResourceType { get { return null; } }

	}

	public class FluentValidationMessageFormatException : Exception {
		public FluentValidationMessageFormatException(string message) : base(message) {
		}

		public FluentValidationMessageFormatException(string message, Exception innerException) : base(message, innerException) {
		}
	}
}