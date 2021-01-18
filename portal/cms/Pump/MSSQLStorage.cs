using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace CMS.Pump {
    public sealed class MSSQLStorage : DataStorage {
        const int default_timeout = 300;//5minut
        public MSSQLStorage(string connectionString) :
            base(connectionString) {
        }

        public SqlConnection Connect() {
            this.connection = new SqlConnection((string)this.DataSource);
            if (connection.State != ConnectionState.Open) this.connection.Open();
            return connection;
        }

        private SqlConnection connection = null;
        public SqlConnection Connection {
            get { return this.connection; }
        }

        public int Exec(SqlConnection connection, string sql) {
            try {
                using (SqlCommand cmd = new SqlCommand(sql, connection)) {
                    cmd.CommandTimeout = default_timeout;
                    cmd.CommandType = CommandType.Text;
                    return cmd.ExecuteNonQuery();
                }
            } catch (Exception ex) {
                throw new MSSQLStorageException(ex, sql);
            }
        }

        public int Exec(SqlConnection connection, string sql, params SqlParameter[] parameters) {
            try {
                using (SqlCommand cmd = new SqlCommand(sql, connection)) {
                    cmd.CommandTimeout = default_timeout;
                    cmd.CommandType = CommandType.Text;
                    foreach (SqlParameter op in parameters) cmd.Parameters.Add(op);
                    return cmd.ExecuteNonQuery();
                }
            } catch (Exception ex) {
                throw new MSSQLStorageException(ex, sql, parameters);
            }
        }

        public object Scalar(SqlConnection connection, string sql) {
            try {
                using (SqlCommand cmd = new SqlCommand(sql, connection)) {
                    cmd.CommandTimeout = default_timeout;
                    cmd.CommandType = CommandType.Text;
                    return cmd.ExecuteScalar();
                }
            } catch (Exception ex) {
                throw new MSSQLStorageException(ex, sql);
            }
        }

        public object Scalar(SqlConnection connection, string sql, params SqlParameter[] parameters) {
            try {
                using (SqlCommand cmd = new SqlCommand(sql, connection)) {
                    cmd.CommandTimeout = default_timeout;
                    cmd.CommandType = CommandType.Text;
                    foreach (SqlParameter op in parameters)
                        cmd.Parameters.Add(op);
                    return cmd.ExecuteScalar();
                }
            } catch (Exception ex) {
                throw new MSSQLStorageException(ex, sql);
            }
        }

        public DataTable Query(SqlConnection connection, string sql) {
            try {
                using (SqlCommand cmd = new SqlCommand(sql, connection)) {
                    cmd.CommandTimeout = default_timeout;
                    cmd.CommandType = CommandType.Text;
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd)) {
                        DataTable d = new DataTable();
                        da.Fill(d);
                        return d;
                    }
                }
            } catch (Exception ex) {
                throw new MSSQLStorageException(ex, sql);
            }
        }

        public DataTable Query(SqlConnection connection, string sql, params SqlParameter[] parameters) {
            try {
                using (SqlCommand cmd = new SqlCommand(sql, connection)) {
                    cmd.CommandTimeout = default_timeout;
                    cmd.CommandType = CommandType.Text;
                    foreach (SqlParameter op in parameters)
                        cmd.Parameters.Add(op);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd)) {
                        DataTable d = new DataTable();
                        da.Fill(d);
                        return d;
                    }
                }
            } catch (Exception ex) {
                throw new MSSQLStorageException(ex, sql, parameters);
            }
        }

        public int ExecProc(SqlConnection connection, string storedProcedure, params SqlParameter[] parameters) {
            try {
                using (SqlCommand cmd = new SqlCommand(storedProcedure, connection)) {
                    cmd.CommandTimeout = default_timeout;
                    cmd.CommandType = CommandType.StoredProcedure;
                    foreach (SqlParameter op in parameters) cmd.Parameters.Add(op);
                    return cmd.ExecuteNonQuery();
                }
            } catch (Exception ex) {
                throw new MSSQLStorageException(ex, storedProcedure, parameters);
            }
        }

        public DataTable QueryProc(SqlConnection connection, string storedProcedure, params SqlParameter[] parameters) {
            try {
                using (SqlCommand command = new SqlCommand(storedProcedure, connection)) {
                    command.CommandTimeout = default_timeout;
                    command.CommandType = CommandType.StoredProcedure;
                    if (parameters != null && parameters.Length > 0) {
                        foreach (SqlParameter param in parameters)
                            command.Parameters.Add(param);
                    }

                    DataSet dataSet = new DataSet();
                    using (SqlDataAdapter da = new SqlDataAdapter(command)) {
                        da.Fill(dataSet);
                    }
                    command.Dispose();
                    if (dataSet.Tables.Count == 0)
                        return null;

                    return dataSet.Tables[dataSet.Tables.Count - 1];
                }
            } catch (Exception ex) {
                throw new MSSQLStorageException(ex, storedProcedure, parameters);
            }
        }

        public DataSet QueryProcDataSet(SqlConnection connection, string storedProcedure, params SqlParameter[] parameters) {
            try {
                using (SqlCommand command = new SqlCommand(storedProcedure, connection)) {
                    command.CommandTimeout = default_timeout;
                    command.CommandType = CommandType.StoredProcedure;
                    if (parameters != null && parameters.Length > 0) {
                        foreach (SqlParameter param in parameters)
                            command.Parameters.Add(param);
                    }

                    DataSet dataSet = new DataSet();
                    using (SqlDataAdapter da = new SqlDataAdapter(command)) {
                        da.Fill(dataSet);
                    }
                    command.Dispose();
                    return dataSet;
                }
            } catch (Exception ex) {
                throw new MSSQLStorageException(ex, storedProcedure, parameters);
            }
        }
    }
}
