using System;

namespace KBE.Shared.Exceptions
{
	public class ConfigurationMissingException<T> : Exception
	{
		public ConfigurationMissingException()
			: base($"The configuration for the type '{typeof(T).Name}' was not found in a same named section or has the wrong datatype!")
		{
		}
	}
}