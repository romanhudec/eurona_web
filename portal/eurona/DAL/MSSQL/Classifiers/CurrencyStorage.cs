using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using CMS.Entities;
using CMS.MSSQL;
using SHP.Entities.Classifiers;

namespace Eurona.DAL.MSSQL.Classifiers {
    internal sealed class CurrencyStorage : MSSQLStorage<Currency> {
        public CurrencyStorage(int instanceId, Account account, string connectionString)
            : base(instanceId, account, connectionString) {
        }

        private static Currency GetCurrency(DataRow record) {
            Currency currency = new Currency();
            currency.Id = Convert.ToInt32(record["CurrencyId"]);
            currency.InstanceId = Convert.ToInt32(record["InstanceId"]);
            currency.Name = Convert.ToString(record["Name"]);
            currency.Code = Convert.ToString(record["Code"]);
            currency.Icon = Convert.ToString(record["Icon"]);
            currency.Locale = Convert.ToString(record["Locale"]);
            currency.Notes = Convert.ToString(record["Notes"]);
            currency.Rate = ConvertNullable.ToDecimal(record["Rate"]);
            currency.Symbol = Convert.ToString(record["Symbol"]);

            return currency;
        }

        public override List<Currency> Read(object criteria) {
            string sql = @"SELECT [CurrencyId], InstanceId, [Name], [Code], [Icon], [Locale], [Notes], [Rate], [Symbol] FROM vShpCurrencies";

            List<Currency> list = new List<Currency>();
            using (SqlConnection connection = Connect()) {
                DataTable table;
                if (criteria is Currency.ReadById) {
                    Currency.ReadById by = criteria as Currency.ReadById;
                    sql += " WHERE CurrencyId = @CurrencyId";
                    sql += " ORDER BY [Name] ASC";
                    table = Query<DataTable>(connection, sql, new SqlParameter("@CurrencyId", by.Id));
                } else if (criteria is Currency.ReadByRate) {
                    Currency.ReadByRate by = criteria as Currency.ReadByRate;
                    sql += " WHERE Rate = @Rate AND Locale = @Locale AND InstanceId=@InstanceId";
                    sql += " ORDER BY [Name] ASC";
                    table = Query<DataTable>(connection, sql, new SqlParameter("@Rate", by.Rate), new SqlParameter("@Locale", Locale), new SqlParameter("@InstanceId", InstanceId));
                } else if (criteria is Currency.ReadByLocale) {
                    Currency.ReadByLocale by = criteria as Currency.ReadByLocale;
                    sql += " WHERE Locale = @Locale";
                    sql += " ORDER BY [Name] ASC";
                    table = Query<DataTable>(connection, sql, new SqlParameter("@Locale", by.Locale), new SqlParameter("@InstanceId", InstanceId));
                } else {
                    sql += " WHERE Locale = @Locale-- AND InstanceId=@InstanceId";
                    sql += " ORDER BY [Name] ASC";
                    table = Query<DataTable>(connection, sql, new SqlParameter("@Locale", Locale), new SqlParameter("@InstanceId", InstanceId));
                }

                foreach (DataRow dr in table.Rows)
                    list.Add(GetCurrency(dr));
            }
            return list;
        }

        public override int Count(object criteria) {
            throw new NotImplementedException();
        }

        public override void Create(Currency currency) {
            using (SqlConnection connection = Connect()) {
                ExecProc(connection, "pShpCurrencyCreate",
                        new SqlParameter("@HistoryAccount", AccountId),
                        new SqlParameter("@InstanceId", InstanceId),
                        new SqlParameter("@Name", currency.Name),
                        new SqlParameter("@Code", currency.Code),
                        new SqlParameter("@Icon", currency.Icon),
                        new SqlParameter("@Notes", currency.Notes),
                        new SqlParameter("@Rate", currency.Rate),
                        new SqlParameter("@Symbol", currency.Symbol),
                        new SqlParameter("@Locale", String.IsNullOrEmpty(currency.Locale) ? Locale : currency.Locale));
            }
        }

        public override void Update(Currency currency) {
            using (SqlConnection connection = Connect()) {
                ExecProc(connection, "pShpCurrencyModify",
                        new SqlParameter("@HistoryAccount", AccountId),
                        new SqlParameter("@CurrencyId", currency.Id),
                        new SqlParameter("@Name", currency.Name),
                        new SqlParameter("@Code", currency.Code),
                        new SqlParameter("@Icon", currency.Icon),
                        new SqlParameter("@Notes", currency.Notes),
                        new SqlParameter("@Rate", currency.Rate),
                        new SqlParameter("@Symbol", currency.Symbol),
                        new SqlParameter("@Locale", currency.Locale));
            }
        }

        public override void Delete(Currency currency) {
            using (SqlConnection connection = Connect()) {
                SqlParameter historyAccount = new SqlParameter("@HistoryAccount", AccountId);
                SqlParameter addressId = new SqlParameter("@CurrencyId", currency.Id);
                SqlParameter result = new SqlParameter("@Result", -1);
                result.Direction = ParameterDirection.Output;
                ExecProc(connection, "pShpCurrencyDelete", result, historyAccount, addressId);
            }
        }
    }
}
