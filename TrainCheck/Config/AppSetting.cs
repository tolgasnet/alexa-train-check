using System;
using System.Collections.Generic;
using System.Linq;

namespace TrainCheck.Config
{
	public abstract class AppSetting
	{
		public void ValidateForNulls()
		{
			var properties = GetType().GetProperties();
			var missingProperties = new List<string>();

			foreach (var property in properties)
			{
				var value = property.GetValue(this);
				if (value == null)
				{
					missingProperties.Add(property.Name);
				}
			}

			if (missingProperties.Any())
			{
				var missingNames = string.Join(", ", missingProperties);

				throw new ApplicationException($"Some of the configuration values are null: {missingNames}");
			}
		}
	}
}