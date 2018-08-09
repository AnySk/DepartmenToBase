using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace Department2Base
{
    class dataBase
    {
        static string work = @"Data Source=SERVER-OTO\SQLEXPRESS;Initial Catalog=DToB;Persist Security Info=True;User ID=Adm;Password=Analiz2";
        static string test = @"Data Source=SERVER-OTO\SQLEXPRESS;Initial Catalog=TestDToB;User ID=Adm;Password=Analiz2";

        /// <summary>
        /// ОСНОВНАЯ СТРОКА ДОСТУПА К БАЗЕ ДАННЫХ
        /// </summary>
        public static string connectString = work;
        private static SqlConnection sqlCon = new SqlConnection(connectString);
        private static SqlCommandBuilder sqlCommand = null;
        public static SqlDataAdapter sqlAdapter = null;
        /// <summary>
        /// ЗДЕСЬ ХРАНЯТСЯ ТАБЛИЦЫ ВСЕХ DataGridView
        /// </summary>
        public static DataSet dataset = new DataSet();

        /// <summary>
        /// Основной метод для привязки данных DataGridView, создаются CommandBuilder для автоматического обновления данных в базе
        /// при изменении данных в DataGridView (Все таблицы хранятся в dataset)
        /// </summary>
        /// <param name="tableName">
        /// Запрос к базе начиная с названия Таблицы.
        /// </param>
        /// <param name="onlyAdapter">
        /// При значении true - обновляется только Adapter, без обновления данных в dataset
        /// </param>
        /// <param name="dataTableName">
        /// Имя таблицы под которым будут храниться данные tableName
        /// </param>
        /// <param name="howmuch">
        /// Количество первых строк с базы(TOP)
        /// </param>
        /// <param name="requestJustByMyself">
        /// При значении true - можно задать всю строку запроса вручную.
        /// </param>
        public static void ToDisplay(string tableName, bool onlyAdapter = false, string dataTableName = null, string howmuch = null, bool requestJustByMyself = false, bool withoutComBuilder = false)
        {
            try
            {
                string req = null;

                if (!requestJustByMyself)
                    req = howmuch == null ? "SELECT * FROM " + tableName + ";" : "SELECT TOP " + howmuch + " * FROM " + tableName + ";";
                else
                    req = tableName;
                if (dataTableName == null)
                    dataTableName = tableName;
                if (!onlyAdapter)
                {
                    if (dataTableName == null)
                    {
                        if (dataset.Tables[tableName] != null)
                            dataset.Tables[tableName].Clear();
                    }
                    else
                    {
                        if (dataset.Tables[dataTableName] != null)
                            dataset.Tables[dataTableName].Clear();
                    }

                    sqlAdapter = new SqlDataAdapter(req, sqlCon);
                    if (!withoutComBuilder)
                    {
                        sqlCommand = new SqlCommandBuilder(sqlAdapter);
                        sqlAdapter.InsertCommand = sqlCommand.GetInsertCommand();
                        sqlAdapter.UpdateCommand = sqlCommand.GetUpdateCommand();
                        sqlAdapter.DeleteCommand = sqlCommand.GetDeleteCommand();
                    }
                    sqlAdapter.Fill(dataset, dataTableName);
                }
                else
                {
                    sqlAdapter = new SqlDataAdapter(req, sqlCon);
                    if (!withoutComBuilder)
                    {
                        sqlCommand = new SqlCommandBuilder(sqlAdapter);
                        sqlAdapter.InsertCommand = sqlCommand.GetInsertCommand();
                        sqlAdapter.UpdateCommand = sqlCommand.GetUpdateCommand();
                        sqlAdapter.DeleteCommand = sqlCommand.GetDeleteCommand();
                    }
                }
            }
            catch(Exception e)
            {
                //MessageBoxTi.Show("database.ToDisplay " + e.Message);
            }
        }

        /// <summary>
        /// Возвращает сухой Datatable.
        /// </summary>
        /// <param name="tableName">
        /// Строка апроса
        /// </param>
        /// <returns></returns>
        public static DataTable SimpleData(string tableName)
        {
            try
            {
                DataTable dt = new DataTable();
                using (SqlConnection sqlConnection = new SqlConnection(connectString))
                {
                    sqlConnection.Open();
                    using (SqlCommand sqlCommand = new SqlCommand("SELECT * FROM " + tableName + ";", sqlConnection))
                    {
                        SqlDataAdapter da = new SqlDataAdapter(sqlCommand);
                        da.Fill(dt);
                        sqlConnection.Close();
                        da.Dispose();
                        return dt;
                    }
                }
            }
            catch (Exception e)
            {
                MessageBoxTi.Show("database.SimpleData " + e.Message);
                return null;
            }
        }

        /// <summary>
        /// Возвращает List со значениями в строке из базы данных
        /// </summary>
        /// <param nae="action"m>
        /// Строка запроса(предпочтительно чтобы возвращал одну строку)
        /// </param>
        /// <returns></returns>
        public static List<object> SingleRow(string action)
        {
            try
            {
                List<object> columns = new List<object>();
                using (SqlConnection sqlConnection = new SqlConnection(connectString))
                {
                    sqlConnection.Open();
                    using (SqlCommand sqlCommand = new SqlCommand(action, sqlConnection))
                    {
                        var reader = sqlCommand.ExecuteReader();

                        reader.Read();
                        for (var i = 0; i < reader.FieldCount; i++)
                            columns.Add(reader.GetValue(i));
                        sqlConnection.Close();
                        return columns;
                    }
                }
            }
            catch (Exception e)
            {
                MessageBoxTi.Show("dataBase.SingleRow " + e.Message);
                return null;
            }
        }

        /// <summary>
        /// Возвращает любое скалярное(одиночное) значение
        /// </summary>
        /// <param name="action">
        /// Строка запроса
        /// </param>
        /// <returns></returns>
        public static object ToCount(string action)
        {
            try
            {
                object y = null;
                using (SqlConnection sqlConnection = new SqlConnection(connectString))
                {
                    sqlConnection.Open();
                    using (SqlCommand sqlCommand = new SqlCommand(action, sqlConnection))
                    {
                        if ((y = sqlCommand.ExecuteScalar()).ToString() == "")
                            y = null;

                        sqlConnection.Close();
                        return y;
                    }
                }
            }
            catch (Exception e)
            {
                //MessageBoxTi.Show("dataBase.ToCount " + e.Message);
                return null;
            }
        }

        /// <summary>
        /// Просто запрос к базе, ничего не возвращает(ExecuteNonQuery), дейсткия типа UPDATE и в том же духе
        /// </summary>
        /// <param name="action">
        /// Строка запроса
        /// </param>
        public static void SimpleRequest(string action)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectString))
                {
                    sqlConnection.Open();
                    using (SqlCommand sqlCommand = new SqlCommand(action, sqlConnection))
                    {
                        sqlCommand.ExecuteNonQuery();
                        sqlConnection.Close();
                    }
                }
            }
            catch (Exception e)
            {
                //MessageBoxTi.Show("dataBase.SimpleRequest " + e.Message);
            }
        }

        /// <summary>
        /// Выполняет запись в таблицу MainLog действия Пользователя
        /// </summary>
        /// <param name="name">
        /// Имя пользователя
        /// </param>
        /// <param name="asWho">
        /// Принидлежность к группе BP201, NS-2 и т.д.
        /// </param>
        /// <param name="action">
        /// То, что сделал пользователь
        /// </param>
        /// <param name="addSeconds">
        /// если нужно добавить секунды ко времени действия, по умолчанию - время берётся с базы
        /// </param>
        public static void ToUpdate(string name, string action, int addSeconds = 0)
        {
            try
            {
                string tiiime = null;
                if (addSeconds == 0)
                {
                    tiiime = "GETDATE()";
                }
                else
                {
                    tiiime = "'" + Convert.ToDateTime(ToCount("SELECT GETDATE()")).AddSeconds(addSeconds).ToString() + "'";
                }

                using (SqlConnection sqlConnection = new SqlConnection(connectString))
                {
                    sqlConnection.Open();
                    using (SqlCommand sqlCommand = new SqlCommand("INSERT INTO  MainLog ([Who], [What], [WhenItWas]) VALUES(@Who, @What, " + tiiime + ")", sqlConnection))
                    {

                        sqlCommand.Parameters.AddWithValue("@Who", name);
                        sqlCommand.Parameters.AddWithValue("@What", action);
                        sqlCommand.ExecuteNonQuery();

                    }
                    sqlConnection.Close();
                }
            }
            catch (Exception e)
            {
                MessageBoxTi.Show("dataBase.ToUpdate1 " + e.Message);
            }
        }

        /// <summary>
        /// Выгрузка новой версии базы в Админке
        /// </summary>
        /// <param name="st">
        /// Сами данные exe
        /// </param>
        /// <param name="ver">
        /// Версия базы
        /// </param>
        public static void ToUpdate(Stream st, string ver)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectString))
                {
                    sqlConnection.Open();
                    using (SqlCommand sqlCommand = new SqlCommand("INSERT INTO  ProgVer ([Data], [Version]) VALUES(@Data, @Version)", sqlConnection))
                    {
                        sqlCommand.Parameters.Add(new SqlParameter("@Data", SqlDbType.VarBinary, -1) { Value = st });
                        sqlCommand.Parameters.Add(new SqlParameter("@Version", SqlDbType.NVarChar, -1) { Value = ver });
                        sqlCommand.ExecuteNonQuery();
                    }
                    sqlConnection.Close();
                }
            }
            catch (Exception e)
            {
                MessageBoxTi.Show("dataBase.ToUpdate2 " + e.Message);
            }
        }

        /// <summary>
        /// Чек-сумма нужного куска базы данных (используется для обнаружения изменений базы внесенных извне)
        /// </summary>
        /// <param name="stringForChecking">
        /// Строка запроса, для которой нужно расчитать чек-сумму
        /// </param>
        /// <returns></returns>
        public static int CheckSum(string stringForChecking, bool justRej = false)
        {
            try
            {
                string rec = null;
                if (justRej) rec = stringForChecking;
                else rec = "SELECT CHECKSUM_AGG(BINARY_CHECKSUM(*)) FROM " + stringForChecking + ";";

                using (SqlConnection sqlConnection = new SqlConnection(connectString))
                {
                    sqlConnection.Open();
                    using (SqlCommand sqlCommand = new SqlCommand(rec, sqlConnection))
                    {
                        sqlCommand.ExecuteNonQuery();
                        object y = sqlCommand.ExecuteScalar();

                        sqlConnection.Close();

                        if (y == DBNull.Value)
                            return -1;
                        else
                            return Convert.ToInt32(y);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBoxTi.Show("dataBase.CheckSum " + e.Message);
                return 0;
            }
        }
    }
}
