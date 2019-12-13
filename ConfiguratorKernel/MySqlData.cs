using System;
using System.Collections.Generic;
using System.Reflection;

using MySql.Data.MySqlClient;
using ConfiguratorKernel.Directory;

namespace ConfiguratorKernel
{
    /// <summary>
    /// Клас для роботи з MySQL
    /// </summary>
    public class MySqlData : IData
    {
        #region CONNECT CLOSE

        /// <summary>
        /// Поточне підключення
        /// </summary>
        private MySqlConnection mConnect { get; set; }

        /// <summary>
        /// Строка підключення
        /// </summary>
        public string ConnectString { get; set; }

        /// <summary>
        /// Підключення до бази даних
        /// </summary>
        /// <returns>true якщо ок</returns>
        public bool Connect()
        {
            mConnect = new MySqlConnection(ConnectString);

            try
            {
                mConnect.Open();

                Console.WriteLine("connect ok");

                return true;
            }
            catch (MySqlException e)
            {
                Console.WriteLine("connect error: " + e.Message);

                mConnect = null;
                return false;
            }
        }

        /// <summary>
        /// Закриття підключення
        /// </summary>
        /// <returns>true якщо ок</returns>
        public bool Close()
        {
            try
            {
                mConnect.Close();
                return true;
            }
            catch
            {
                mConnect = null;
                return false;
            }
        }

        #endregion

        #region DIRECTORY

        //Вибірка ссилок
        public void DirectorySelectLink(List<DirectoryLink> link, DirectoryLink emptyLink, Dictionary<string, string> where = null, Dictionary<string, string> orderBy = null, int limit = 0)
        {
            MySqlCommand myCommand = new MySqlCommand();
            myCommand.Connection = mConnect;

            myCommand.CommandText = "SELECT `ID` FROM `" + emptyLink.Table + "`";

            MySqlDataReader reader = myCommand.ExecuteReader();
            while (reader.Read())
            {
                link.Add((DirectoryLink)Activator.CreateInstance(emptyLink.GetType(), new object[] { reader["ID"].ToString() }));
            }
            reader.Close();

        }

        //Вибірка всіх полів по ссилці
        public void DirectorySelectFieldsByLink(DirectoryLink link, List<DirectoryFieldInfo> fields)
        {
            MySqlCommand myCommand = new MySqlCommand();
            myCommand.Connection = mConnect;

            string _fields_sql = "";

            foreach (DirectoryFieldInfo field in fields)
            {
                _fields_sql += (_fields_sql.Length > 0 ? ", " : "") + "`" + field.FieldName + "`";
            }

            myCommand.CommandText = "SELECT " + _fields_sql + " FROM `" + link.Table + "` WHERE `ID` = " + link.ID;

            MySqlDataReader reader = myCommand.ExecuteReader();
            if (reader.Read())
            {
                foreach (DirectoryFieldInfo field in fields)
                {
                    field.FieldValue = reader[field.FieldName];
                }
            }
            reader.Close();
        }

        public DirectoryLink DirectoryObjectSave(DirectoryLink link, List<DirectoryFieldInfo> fields)
        {
            MySqlCommand myCommand = new MySqlCommand();
            myCommand.Connection = mConnect;

            if (link.IsEmptyLink())
            {
                string _fields_name_sql = "";
                string _fields_value_sql = "";

                foreach (DirectoryFieldInfo field in fields)
                {
                    _fields_name_sql += (_fields_name_sql.Length > 0 ? ", " : "") + "`" + field.FieldName + "`";
                    _fields_value_sql += (_fields_value_sql.Length > 0 ? ", " : "") + "@" + field.FieldName;

                    myCommand.Parameters.AddWithValue("@" + field.FieldName, field.FieldValue);
                }

                myCommand.CommandText = "INSERT INTO `" + link.Table + "` (" + _fields_name_sql + ") VALUES(" + _fields_value_sql + ")";

                myCommand.ExecuteNonQuery();

                return new DirectoryLink(link.Table, myCommand.LastInsertedId.ToString());
            }
            else
            {
                string _fields_name_value_sql = "";
   
                foreach (DirectoryFieldInfo field in fields)
                {
                    _fields_name_value_sql += (_fields_name_value_sql.Length > 0 ? ", " : "") + "`" + field.FieldName + "` = @" + field.FieldName;

                    myCommand.Parameters.AddWithValue("@" + field.FieldName, field.FieldValue);
                }

                myCommand.CommandText = "UPDATE `" + link.Table + "` SET " + _fields_name_value_sql + " WHERE `ID` = '" + link.ID + "'";

                myCommand.ExecuteNonQuery();

                return link;
            }
        }

        public void DirectoryObjectTabularPartSave(List<DirectoryTabularPartRecord> records, string ownerID, string tabularPartTable, List<DirectoryFieldInfo> fields)
        {
            MySqlCommand myCommand = new MySqlCommand();
            myCommand.Connection = mConnect;

            myCommand.CommandText = "START TRANSACTION";
            myCommand.ExecuteNonQuery();

            //Очистка записів
            myCommand.CommandText = "DELETE FROM `" + tabularPartTable + "` WHERE `Owner` = @Owner";
            myCommand.Parameters.AddWithValue("@Owner", ownerID);
            myCommand.ExecuteNonQuery();

            foreach (DirectoryTabularPartRecord record in records)
            {
                myCommand.Parameters.Clear();

                Type recordType = record.GetType();

                string _fields_name_sql = "`ID`";
                string _fields_value_sql = "@ID";
                string _fields_value_update_sql = "";

                PropertyInfo PropertyRecordInfo = recordType.GetProperty("ID");
                string field_id_value = PropertyRecordInfo.GetValue(record).ToString();
                myCommand.Parameters.AddWithValue("@ID", (field_id_value == "" ? "0" : field_id_value));

                _fields_name_sql += ",`Owner`";
                _fields_value_sql += ",@Owner";
                _fields_value_update_sql += "`Owner` = @Owner";

                myCommand.Parameters.AddWithValue("@Owner", ownerID);

                foreach (DirectoryFieldInfo field in fields)
                {
                    _fields_name_sql += ",`" + field.FieldName + "`";
                    _fields_value_sql += ",@" + field.FieldName;
                    _fields_value_update_sql += (_fields_value_update_sql.Length > 0 ? "," : "") + "`" + field.FieldName + "` = @" + field.FieldName;

                    PropertyRecordInfo = record.GetType().GetProperty(field.FieldName);
                    myCommand.Parameters.AddWithValue("@"+ field.FieldName, PropertyRecordInfo.GetValue(record));
                }

                myCommand.CommandText = "INSERT INTO `" + tabularPartTable + "` (" + _fields_name_sql + ") VALUES (" + _fields_value_sql + ") ON DUPLICATE KEY UPDATE " + _fields_value_update_sql;

                myCommand.ExecuteNonQuery();
            }

            myCommand.CommandText = "COMMIT";
            myCommand.ExecuteNonQuery();
        }

        public void DirectoryTabularPartSelect(List<DirectoryTabularPartRecord> records, DirectoryTabularPartRecord record, string ownerID, string tabularPartTable, List<DirectoryFieldInfo> fields)
        {
            MySqlCommand myCommand = new MySqlCommand();
            myCommand.Connection = mConnect;

            myCommand.CommandText = "SELECT * FROM `" + tabularPartTable + "` WHERE `owner` = @owner";
            myCommand.Parameters.AddWithValue("@owner", ownerID);

            MySqlDataReader reader = myCommand.ExecuteReader();
            while (reader.Read())
            {
                DirectoryTabularPartRecord newRecord = (DirectoryTabularPartRecord)Activator.CreateInstance(record.GetType());

                Type newRecordType = newRecord.GetType();

                PropertyInfo PropertyRecordInfo = newRecordType.GetProperty("ID");
                PropertyRecordInfo.SetValue(newRecord, reader["ID"].ToString());

                foreach (DirectoryFieldInfo field in fields)
                {
                    PropertyRecordInfo = newRecordType.GetProperty(field.FieldName);
                    field.FieldValue = reader[field.FieldName];
                    PropertyRecordInfo.SetValue(newRecord, field.FieldValue);
                }

                records.Add(newRecord);
            }
            reader.Close();
        }

        #endregion

    }
}
