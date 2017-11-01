using System;

namespace System
{
	/// <summary>
	/// ConvertNullable class.
	/// </summary>
	public static class ConvertNullable
	{
		/// <summary>
		/// Converts the System.String representation of a number in a specified base to an equivalent 8-bit unsigned integer, or null.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static Byte? ToByte(object value)
		{
			if (value == null || value == DBNull.Value) return null;
			byte returnValue;
			if (Byte.TryParse(value.ToString(), out returnValue))
				return returnValue;
			return null;
		}

		/// <summary>
		/// Converts a specified value to an equivalent Boolean value, or null.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static bool? ToBool(object value)
		{
			if (value == null || value == DBNull.Value) return null;
			bool returnValue;
			if (bool.TryParse(value.ToString(), out returnValue))
				return returnValue;
			return null;
		}

		/// <summary>
		/// Converts a specified value to a DateTime, or null.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static DateTime? ToDateTime(object value)
		{
			if (value == null || value == DBNull.Value) return null;
			// if null is passed in here, it will convert 
			// to 01/01/0001 which is definately not expected behavior
			if (value != null) {
				DateTime returnValue;
				if (DateTime.TryParse(value.ToString(), out returnValue))
					return returnValue;
			}
			return null;
		}

		/// <summary>
		/// Converts a specified value to a decimal number, or null.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static Decimal? ToDecimal(object value)
		{
			if (value == null || value == DBNull.Value) return null;
			Decimal returnValue;
			if (Decimal.TryParse(value.ToString(), out returnValue))
				return returnValue;
			return null;
		}

		/// <summary>
		/// Converts a specified value to a double-precision floating point number, or null.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static Double? ToDouble(object value)
		{
			if (value == null || value == DBNull.Value) return null;
			Double returnValue;
			if (Double.TryParse(value.ToString(), out returnValue))
				return returnValue;
			return null;
		}

		/// <summary>
		/// Converts the System.String representation of a number in a specified base to an equivalent 16-bit signed integer, or null.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static Int16? ToInt16(object value)
		{
			if (value == null || value == DBNull.Value) return null;
			Int16 returnValue;
			if (Int16.TryParse(value.ToString(), out returnValue))
				return returnValue;
			return null;
		}

		/// <summary>
		/// Converts the System.String representation of a number in a specified base to an equivalent 32-bit signed integer, or null.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static Int32? ToInt32(object value)
		{
			if (value == null || value == DBNull.Value) return null;
			Int32 returnValue;
			if (Int32.TryParse(value.ToString(), out returnValue))
				return returnValue;
			return null;
		}

		/// <summary>
		/// Converts the System.String representation of a number in a specified base to an equivalent 64-bit signed integer, or null.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static Int64? ToInt64(object value)
		{
			if (value == null || value == DBNull.Value) return null;
			Int64 returnValue;
			if (Int64.TryParse(value.ToString(), out returnValue))
				return returnValue;
			return null;
		}

		/// <summary>
		/// Converts the System.String representation of a number in a specified base to an equivalent 8-bit signed integer, or null.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static SByte? ToSByte(object value)
		{
			if (value == null || value == DBNull.Value) return null;
			SByte returnValue;
			if (SByte.TryParse(value.ToString(), out returnValue))
				return returnValue;
			return null;
		}

		/// <summary>
		/// Converts a specified value to a single-precision floating point number, or null.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static Single? ToSingle(object value)
		{
			if (value == null || value == DBNull.Value) return null;
			Single returnValue;
			if (Single.TryParse(value.ToString(), out returnValue))
				return returnValue;
			return null;
		}

		/// <summary>
		/// Converts the System.String representation of a number in a specified base to an equivalent 16-bit unsigned integer, or null.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static UInt16? ToUInt16(object value)
		{
			if (value == null || value == DBNull.Value) return null;
			UInt16 returnValue;
			if (UInt16.TryParse(value.ToString(), out returnValue))
				return returnValue;
			return null;
		}

		/// <summary>
		/// Converts the System.String representation of a number in a specified base to an equivalent 32-bit unsigned integer, or null.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static UInt32? ToUInt32(object value)
		{
			if (value == null || value == DBNull.Value) return null;
			UInt32 returnValue;
			if (UInt32.TryParse(value.ToString(), out returnValue))
				return returnValue;
			return null;
		}

		/// <summary>
		/// Converts the System.String representation of a number in a specified base to an equivalent 64-bit unsigned integer, or null.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static UInt64? ToUInt64(object value)
		{
			if (value == null || value == DBNull.Value) return null;
			UInt64 returnValue;
			if (UInt64.TryParse(value.ToString(), out returnValue))
				return returnValue;
			return null;

		}

		/// <summary>
		/// Converts the System.String to value or null.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static String ToString(object value)
		{
			if (value == null || value == DBNull.Value) return null;
			return value.ToString();
		}

		/// <summary>
		/// Converts object to byte array value or null.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static byte[] ToByteArray(object value)
		{
			if (value == null || value == DBNull.Value) return null;
			return (byte[])value;
		}

	}
}