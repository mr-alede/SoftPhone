using SoftPhone.Core.Core;
using SoftPhone.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftPhone.Core.Helpers
{
	public static class ExceptionsHelper
	{
		public static void GenerateExceptionEvent<T>(Exception e) where T: ErrorEventBase, new()
		{
			Exception exception = e;
			while (exception.InnerException != null)
			{
				exception = exception.InnerException;
			}

			//EventsAggregator.Raise(new T(exception.Message));
		}

	}
}
