using System;
using System.Collections.Generic;
using System.Linq;
using Oracle.ManagedDataAccess.Client;
using Dapper;


namespace EvaBatch.Web.Helper
{
    public class SessionFactory : IDisposable
    {
        public string ConnectionString { get; set; }

        public SessionFactory(string _connectionString)
        {
            ConnectionString = _connectionString;
        }

        public void Persist<T>(T model)
        {
            var db_model = Get(model);
            if (db_model == null)
                Add(model);
            else
                Update(model);
        }

        OracleConnection _Conn;
        public OracleConnection Conn
        {
            get
            {
                if (_Conn == null)
                {
                    _Conn = new OracleConnection(ConnectionString);
                }
                return _Conn;
            }
            set
            {
                _Conn = value;
            }
        }

        public SessionFactory CreateQuery(string sql)
        {
            this.CurrentSql = sql;
            return this;
        }

        public SessionFactory SetParameter(string key, object value)
        {
            this.CurrentParams.Add(key, value);
            return this;
        }

        public string CurrentSql { get; set; }

        DynamicParameters _CurrentParams;
        public DynamicParameters CurrentParams
        {
            get
            {
                if (_CurrentParams == null)
                {
                    _CurrentParams = new DynamicParameters();
                }
                return _CurrentParams;
            }
            set
            {
                _CurrentParams = value;
            }
        }

        public List<T> List<T>(string condition = "", string fields = null)
        {
            var from_table = GetTableName<T>();// ?? typeof(T).Name;
            fields = fields ?? ListDbColumns<T>();
            var sql = string.Empty;
            if (string.IsNullOrEmpty(CurrentSql))
            {
                sql = string.Format(" select {0} from {1} {2}"
                       , fields, from_table, condition);
                return Exec<T>(sql, CurrentParams).ToList();
            }
            else
            {
                return Exec<T>($" select {fields} {CurrentSql} ", CurrentParams).ToList();
            }
        }

        public string ListDbColumns<T>()
        {
            var property_list = typeof(T).GetProperties();
            var column_list = new List<string>();
            foreach (var item in property_list)
            {
                var type_name = item.PropertyType.Name;
                if (!type_name.ToLower().Contains("list"))
                {
                    var foo = item
                        .GetCustomAttributes(false);
                    var cust_name = foo.FirstOrDefault() as ColumnNameAttribute;
                    if (cust_name != null)
                    {
                        column_list.Add(cust_name.Name);
                    }
                    else
                    {
                        column_list.Add(item.Name);
                    }
                }
            }
            return string.Join(",", column_list.ToArray());
        }

        private string GetTableName<T>()
        {
            var type = typeof(T);
            var custom_attr = type.GetCustomAttributes(
                                    typeof(TableNameAttribute), true
                                ).FirstOrDefault() as TableNameAttribute;
            return custom_attr == null ? type.Name : custom_attr.Name;
        }

        public void Insert<T>(T model, string[] exception = null)
        {
            try
            {
                exception = exception ?? new string[] { };
                var type = typeof(T);
                var column_list = ListDbColumns<T>();// type.GetProperties()
                                                     //.Where(x => !exception.Contains(x.Name))
                                                     //.ToArray();
                var column_name_list_raw = column_list.Split(',');
                var column_name_list = column_list; // string.Join(",", column_name_list_row);
                var column_param_list = string.Join(",", column_name_list_raw.Select(x => $":{x}"));
                string tableName = GetTableName<T>();
                var sql = $"Insert into {tableName} ( {column_name_list} ) VALUES ( {column_param_list} ) ";

                DynamicParameters param = new DynamicParameters();
                foreach (var item in column_name_list_raw)
                {
                    var _name = item;
                    var _value = type.GetProperty(item).GetValue(model, null);
                    param.Add($":{item}", _value);
                }

                Conn.Execute(sql, param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Add<T>(T model, string[] exception = null)
        {
            Insert(model, exception);
        }

        public void Update<T>(T model, string[] exception = null)
        {
            exception = exception ?? new string[] { };
            bool has_key = false;
            using (var conn = new OracleConnection(ConnectionString)) /*new OracleConnection(ConnString)*/
            {
                conn.Open();
                var type = typeof(T);
                string tableName = GetTableName<T>();// ?? nameof(T);
                var property_list = type.GetProperties().Where(x => !exception.Contains(x.Name)).ToList();
                var column_list = type.GetProperties().Select(x => x.Name).ToArray();
                var update_field = string.Join(" , ", column_list.Select(x => $"{x} =:{x}"));
                DynamicParameters param_list = new DynamicParameters();
                var where_sql = string.Empty;
                foreach (var item in property_list)
                {
                    var is_keys = item.GetCustomAttributes(true).Any(x => x as IdentityAttribute != null);
                    var var_name = $":{item.Name}";
                    var var_value = item.GetValue(model, null);
                    param_list.Add(var_name, var_value);
                    if (is_keys)
                    {
                        has_key = true;
                        where_sql += $" {item.Name} = {var_name} ";
                    }
                }

                if (!has_key)
                    throw new Exception($"Error, object [{type.Name}] doesn't any identity key defined.");

                var sql = $"update {tableName} set {update_field} where {where_sql}";
                try
                {
                    Conn.Execute(sql, param_list);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public void UpdateByKey<T>(T model, string[] update_by, string[] column_list = null)
        {
            using (var conn = new OracleConnection(ConnectionString)) /*new OracleConnection(ConnString)*/
            {
                conn.Open();
                var type = typeof(T);
                string tableName = GetTableName<T>();// ?? nameof(T);
                //var property_list = type.GetProperties().Where(x => !exception.Contains(x.Name)).ToList();
                //var column_list = type.GetProperties().Select(x => x.Name).ToArray();
                var update_field = string.Join(" , ", column_list.Select(x => $"{x} =:{x}"));
                DynamicParameters param_list = new DynamicParameters();
                var where_sql = string.Empty;

                foreach (var item in update_by)
                {
                    var var_value = type.GetProperty(item).GetValue(model, null);
                    param_list.Add($":{item}", var_value);
                    where_sql += $" {item} =:{item} ";
                }

                foreach (var item in column_list)
                {
                    var var_value = type.GetProperty(item).GetValue(model, null);
                    param_list.Add($":{item}", var_value);
                }

                var sql = $"update {tableName} set {update_field} where {where_sql}";
                Conn.Execute(sql, param_list);
            }
        }

        public void Clear<T>(T model)
        {
            var sql_params = GetParamByAttribute(model);
            if (!sql_params.ParameterNames.Any())
                throw new Exception($"Error, object [{typeof(T).Name}] doesn't any identity key defined.");

            DeleteEntity<T>(sql_params);
        }


        public T Get<T>(T model)
        {
            var sql_params = GetParamByAttribute(model);
            return GetEntity<T>(sql_params);
        }

        private DynamicParameters GetParamByAttribute<T>(T model)
        {
            var type = typeof(T);
            string tableName = GetTableName<T>();// ?? nameof(T);
            var property_list = type.GetProperties().ToList();
            //var key_list = (IdentityAttribute[])property_list.Where(x=>x.GetCustomAttributes(true));

            DynamicParameters output = new DynamicParameters();
            var where_sql = string.Empty;
            foreach (var item in property_list)
            {
                var is_keys = item.GetCustomAttributes(true).Any(x => x as IdentityAttribute != null);
                if (is_keys)
                {
                    var var_name = $":{item.Name}";
                    var var_value = item.GetValue(model, null);
                    where_sql += $" {item.Name} = {var_name} ";
                    output.Add(var_name, var_value);
                }
            }
            return output;
        }

        public T GetEntity<T>(DynamicParameters param)
        {
            var type = typeof(T);
            string tableName = GetTableName<T>();// ?? nameof(T);
            var column_list = ListDbColumns<T>();// string.Join(",", type.GetProperties().Select(x => x.Name).ToArray());

            var sql = $"select {column_list} from {tableName} ";
            if (param.ParameterNames.Count() > 0)
                sql += $" where " + string.Join(" and ", param.ParameterNames.Select(x => string.Format(" {0}=:{0} ", x)));
            return Conn.Query<T>(sql, param).FirstOrDefault();
        }

        public void DeleteEntity<T>(DynamicParameters param)
        {
            var type = typeof(T);
            string tableName = GetTableName<T>();// ?? nameof(T);
            //var column_list = type.GetProperties().Select(x => x.Name).ToArray();

            var sql = $"delete from {tableName} where " + string.Join(" and ", param.ParameterNames.Select(x => string.Format(" {0}=:{0} ", x)));
            Conn.Query(sql, param).FirstOrDefault();
        }

        public void Exec(string sql = "", DynamicParameters paramters = null)
        {
            sql = string.IsNullOrEmpty(sql) ? CurrentSql : sql;
            Conn.Execute(sql, paramters);
        }

        public IEnumerable<T> Exec<T>(string sql = "", DynamicParameters paramters = null)
        {
            try
            {
                using (OracleCommand cmd = new OracleCommand())
                {
                    sql = string.IsNullOrEmpty(sql) ? CurrentSql : sql;
                    var result = Conn.Query<T>(sql, paramters);
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string ExecConcat(string sql = "")
        {
            try
            {
                sql = string.IsNullOrEmpty(sql) ? CurrentSql : sql;
                using (OracleCommand cmd = new OracleCommand())
                {
                    string result = "";
                    var reader = Conn.ExecuteReader(sql);
                    while (reader.Read())
                    {
                        result += "," + reader.GetString(0);
                    }
                    if (!string.IsNullOrEmpty(result))
                    {
                        return result.Substring(1);
                    }
                    else
                    {
                        return result;
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Dispose()
        {
            if (Conn != null)
                Conn.Dispose();
        }
    }
}