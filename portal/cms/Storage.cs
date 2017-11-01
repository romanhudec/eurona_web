using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.Entities;

namespace CMS
{
	public static class Storage<T> where T : Entity, new()
	{
		internal static IStorage<T> Instance
		{
			get
			{
				IStorage<T> instance = new WebStorage<T>().Access();
				if (instance == null)
					throw new InvalidOperationException(string.Format("CMS.Storage.Instance Can not be null! Please add storage definition for {0} int WebStorage",
							typeof(T).ToString()));

				return instance;
			}
		}

		public static int Count()
		{
			return Count(null);
		}

		public static int Count(object contition)
		{
			return Instance.Count(contition);
		}

		public static List<T> Read()
		{
			return Read(null);
		}

		public static List<T> Read(object contition)
		{
			try
			{
				return Instance.Read(contition);
			}
			catch (Exception ex)
			{
				return Log.Error<List<T>>(ex);
			}
		}

		public static T ReadFirst(object condition)
		{
			List<T> list = Read(condition);
			return list != null && list.Count > 0 ? list[0] : null;
		}

		public static T ReadFirst()
		{
			List<T> list = Read();
			return list != null && list.Count > 0 ? list[0] : null;
		}

		public static T Create(T entity)
		{
			try
			{
				Instance.Create(entity);
				return entity;
			}
			catch (Exception ex)
			{
				return Log.Error<T>(ex);
			}
		}

		public static T Update(T entity)
		{
			try
			{
				Instance.Update(entity);
				return entity;
			}
			catch (Exception ex)
			{
				return Log.Error<T>(ex);
			}
		}

		public static T Delete(T entity)
		{
			try
			{
				Instance.Delete(entity);
				return entity;
			}
			catch (Exception ex)
			{
				return Log.Error<T>(ex);
			}
		}

		public static R Execute<R>(R command) where R : class, new()
		{
			try
			{
				return Instance.Execute<R>(command);
			}
			catch (Exception ex)
			{
				Log.Error<T>(ex);
				return default(R);
			}
		}
	}
}
