using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace ClientDetails.App_Code
{
    public class clsConnection
    {
        string connString = ConfigurationManager.ConnectionStrings["MyConnectionString"].ToString();
        SqlConnection objSqlConnection;


        public DataSet ExecuteDataSet(SqlCommand objSqlCommand)
        {
            SqlDataAdapter objSqlDataAdapter = new SqlDataAdapter();
            DataSet ds = new DataSet();

            objSqlConnection = new SqlConnection(connString);
            objSqlConnection.Open();
            try
            {
                objSqlCommand.Connection = objSqlConnection;
                objSqlDataAdapter.SelectCommand = objSqlCommand;
                objSqlDataAdapter.Fill(ds);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                objSqlCommand.Dispose();
                objSqlConnection.Close();
            }
            return ds;
        }

        public void ExecuteNonQuery(SqlCommand objSqlCommand)
        {
            objSqlConnection = new SqlConnection(connString);
            objSqlConnection.Open();
            try
            {
                objSqlCommand.Connection = objSqlConnection;
                objSqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                objSqlCommand.Dispose();
                objSqlConnection.Close();
            }
        }

        public void SQLBulkCopy(DataTable oDataTable, string TableName)
        {
            SqlBulkCopy BC = new SqlBulkCopy(connString, SqlBulkCopyOptions.TableLock);
            BC.DestinationTableName = TableName.Trim();
            BC.BatchSize = oDataTable.Rows.Count;
            BC.BulkCopyTimeout = 0;
            BC.WriteToServer(oDataTable);
            BC.Close();
        }


        public DataSet ExecuteDataSet(SqlCommand objSqlCommand, string ConnectionString)
        {
            SqlDataAdapter objSqlDataAdapter = new SqlDataAdapter();
            DataSet ds = new DataSet();

            objSqlConnection = new SqlConnection(ConnectionString);
            objSqlConnection.Open();
            try
            {
                objSqlCommand.Connection = objSqlConnection;
                objSqlDataAdapter.SelectCommand = objSqlCommand;
                objSqlDataAdapter.Fill(ds);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                objSqlCommand.Dispose();
                objSqlConnection.Close();
            }
            return ds;
        }

        public void ExecuteNonQuery(SqlCommand objSqlCommand, string ConnectionString)
        {
            objSqlConnection = new SqlConnection(ConnectionString);
            objSqlConnection.Open();
            try
            {
                objSqlCommand.Connection = objSqlConnection;
                objSqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                objSqlCommand.Dispose();
                objSqlConnection.Close();
            }
        }

    }
}