using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using CMS.Entities;
using CMS.MSSQL;
using SHP.Entities.Classifiers;

namespace SHP.MSSQL.Classifiers
{
	public sealed class VATStorage : MSSQLStorage<VAT>
	{
		public VATStorage(int instanceId, Account account, string connectionString)
			: base(instanceId, account, connectionString)
		{
		}

		private static VAT GetVAT(DataRow record)
		{
			VAT vat = new VAT();
			vat.Id = Convert.ToInt32(record["VATId"]);
			vat.InstanceId = Convert.ToInt32(record["InstanceId"]);
			vat.Name = Convert.ToString(record["Name"]);
			vat.Code = Convert.ToString(record["Code"]);
			vat.Icon = Convert.ToString(record["Icon"]);
			vat.Locale = Convert.ToString(record["Locale"]);
			vat.Notes = Convert.ToString(record["Notes"]);
			vat.Percent = ConvertNullable.ToDecimal(record["Percent"]);

			return vat;
		}

		public override List<VAT> Read(object criteria)
		{
			string sql = @"SELECT VATId, InstanceId, [Name], [Code], [Icon], [Locale], [Notes], [Percent] FROM vShpVATs";

			List<VAT> list = new List<VAT>();
			using (SqlConnection connection = Connect())
			{
				DataTable table;
				if (criteria is VAT.ReadById)
				{
					VAT.ReadById by = criteria as VAT.ReadById;
					sql += " WHERE VATId = @VATId";
					sql += " ORDER BY [Name] ASC";
					table = Query<DataTable>(connection, sql, new SqlParameter("@VATId", by.Id));
				}
				else
				{
					sql += " WHERE Locale = @Locale AND InstanceId=@InstanceId";
					sql += " ORDER BY [Name] ASC";
					table = Query<DataTable>(connection, sql, new SqlParameter("@Locale", Locale), new SqlParameter("@InstanceId", InstanceId));
				}

				foreach (DataRow dr in table.Rows)
					list.Add(GetVAT(dr));
			}
			return list;
		}

		public override int Count(object criteria)
		{
			throw new NotImplementedException();
		}

		public override void Create(VAT vat)
		{
			using (SqlConnection connection = Connect())
			{
				ExecProc(connection, "pShpVATCreate",
						new SqlParameter("@HistoryAccount", AccountId),
						new SqlParameter("@InstanceId", InstanceId),
						new SqlParameter("@Name", vat.Name),
						new SqlParameter("@Code", vat.Code),
						new SqlParameter("@Icon", vat.Icon),
						new SqlParameter("@Notes", vat.Notes),
						new SqlParameter("@Percent", vat.Percent),
						new SqlParameter("@Locale", String.IsNullOrEmpty(vat.Locale) ? Locale : vat.Locale));
			}
		}

		public override void Update(VAT vat)
		{
			using (SqlConnection connection = Connect())
			{
				ExecProc(connection, "pShpVATModify",
						new SqlParameter("@HistoryAccount", AccountId),
						new SqlParameter("@VATId", vat.Id),
						new SqlParameter("@Name", vat.Name),
						new SqlParameter("@Code", vat.Code),
						new SqlParameter("@Icon", vat.Icon),
						new SqlParameter("@Notes", vat.Notes),
						new SqlParameter("@Percent", vat.Percent),
						new SqlParameter("@Locale", vat.Locale));
			}
		}

		public override void Delete(VAT vat)
		{
			using (SqlConnection connection = Connect())
			{
				SqlParameter historyAccount = new SqlParameter("@HistoryAccount", AccountId);
				SqlParameter addressId = new SqlParameter("@VATId", vat.Id);
				SqlParameter result = new SqlParameter("@Result", -1);
				result.Direction = ParameterDirection.Output;
				ExecProc(connection, "pShpVATDelete", result, historyAccount, addressId);
			}
		}
	}
}
