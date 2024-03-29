﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CSGitCack
{
    public class ColumnAttribute : Attribute
    {
        private string columnName;

        public ColumnAttribute(string s)
        {
            columnName = s;
        }

        public string GetColumnName()
        {
            return columnName;
        }
    }

    public class IdentityAttribute : Attribute
    {
    }

    public class TableNameAttribute : Attribute
    {
        private string tableName;

        public TableNameAttribute(string s)
        {
            tableName = s;
        }

        public string GetTableName()
        {
            return tableName;
        }
    }

    public enum SqlTypes { SELECT, INSERT, UPDATE };

    // Because I don't want SQLWriter to be static or in a static class
    public class Holder
    {
        public string SelectWriter<T>()
        {
            var columns = new List<string>();
            var getty = typeof(T).GetProperties();
            foreach (var f in getty)
            {
                // Take the column name from T unless it has a ColAttr
                string column = f.Name;
                var aca = Attribute.GetCustomAttributes(f);
                foreach (Attribute a in aca)
                {
                    if (a is ColumnAttribute col)
                    {
                        // Should only be one
                        column = $"{col.GetColumnName()} {f.Name}";
                    }
                }
                columns.Add(column);
            }
            string sql = $"SELECT {string.Join(", ", columns)} FROM {typeof(T).Name}";
            return sql;
        }

        public string InsertWriter<T>()
        {
            var columns = new List<string>();
            var values = new List<string>();
            foreach (var f in typeof(T).GetProperties())
            {
                // Take the column name from T unless it has a ColAttr
                string column = f.Name;
                // Value is the T name regardless of the attributes
                values.Add("@" + f.Name);

                var aca = Attribute.GetCustomAttributes(f);
                foreach (Attribute a in aca)
                {
                    if (a is ColumnAttribute col)
                    {
                        // Should only be one
                        column = $"{col.GetColumnName()}";
                    }
                    else if (a is IdentityAttribute id)
                    {
                        // NOP. Exclude the [Identity] field cos that does its own thing
                    }
                }
                columns.Add(column);
            }
            string sql = $"INSERT INTO table({string.Join(", ", columns)}) VALUES ({string.Join(", ", values)})";
            return sql;
        }

        // UPDATE must have a WHERE clause otherwise it'll update all rows of the table.
        // If you really want that you can supply 1=1 and that will do the trick.
        public string UpdateWriter<T>(string whereClause)
        {
            var columns = new List<string>();
            string autoWhere = "";
            foreach (var f in typeof(T).GetProperties())
            {
                string column = $"{f.Name}=@{f.Name}";
                var aca = Attribute.GetCustomAttributes(f);
                bool isIdentity = false;
                foreach (Attribute a in aca)
                {
                    if (a is ColumnAttribute col)
                    {
                        // Should only be one
                        column = $"{col.GetColumnName()}=@{f.Name}";
                    }
                    else if (a is IdentityAttribute id)
                    {
                        // Not yet supporting "WHERE vfuvibvdiasoid=@FriendlyName"; we'd need to take both attributes into account then
                        autoWhere = $"{f.Name}=@{f.Name}";
                        isIdentity = true;
                    }
                }
                if (!isIdentity)
                    columns.Add(column);
            }
            if (!string.IsNullOrEmpty(whereClause))
                autoWhere = whereClause;
            string sql = $"UPDATE {typeof(T).Name} SET {string.Join(", ", columns)} WHERE {autoWhere}";
            return sql;
        }

        // WHERE clause is optional for SELECTs and is added if present.
        // It is ignored for INSERT statements, and mandatory for UPDATE.
        // If none is supplied for UPDATE then an autogenerated one is used. You can override this by supplying 1=1 if you really want that behaviour.
        public string SqlWriter<T>(SqlTypes sqlType, string whereClause)
        {
            var SelectNames = new List<string>();
            var InsertNames = new List<string>();
            var Values = new List<string>();
            var UpdateExprs = new List<string>();
            string autoWhere = "";

            var objInfo = typeof(T).GetTypeInfo();
            foreach (Attribute attr in objInfo.GetCustomAttributes())
            {
                if (attr is TableNameAttribute tna)
                {
                    Console.WriteLine($"Table name: [{tna.GetTableName()}]");
                }
            }

            foreach (var f in typeof(T).GetProperties())
            {
                bool isIdentity = false;
                var aca = Attribute.GetCustomAttributes(f);
                string colName = f.Name;
                string colExpr = $"{f.Name}=@{f.Name}";

                foreach (Attribute a in aca)
                {
                    if (a is ColumnAttribute col)
                    {
                        colName = $"{col.GetColumnName()}";
                        colExpr = $"{col.GetColumnName()}=@{f.Name}";
                    }
                    else if (a is IdentityAttribute id)
                    {
                        // Currently only supporting WHERE idCol=@idCol
                        // May support WHERE vjjqxn=@FriendlyIdName later but we only use "id" for IDENTITY columns afaik
                        autoWhere = $"{f.Name}=@{f.Name}";
                        isIdentity = true;
                    }
                }
                if (isIdentity)
                {
                    SelectNames.Add(colName);
                }
                else
                {
                    SelectNames.Add(colName);
                    InsertNames.Add(colName);
                    Values.Add("@" + f.Name);
                    UpdateExprs.Add(colExpr);
                }
            }
            string sql = "";
            switch (sqlType)
            {
                case SqlTypes.SELECT:
                    // SelectNames can include identities
                    sql = $"SELECT {string.Join(", ", SelectNames)} FROM {typeof(T).Name}";
                    if (!string.IsNullOrEmpty(whereClause))
                    {
                        sql += $" WHERE {whereClause}";
                    }
                    break;

                case SqlTypes.INSERT:
                    // InsertNames must exclude identities cos they do their own thing (so don't label a column [Identity] if it's not an IDENTITY type in the DB)
                    sql = $"INSERT INTO table({string.Join(", ", InsertNames)}) VALUES ({string.Join(", ", Values)})";
                    break;

                case SqlTypes.UPDATE:
                    if (!string.IsNullOrEmpty(whereClause))
                        autoWhere = whereClause;
                    sql = $"UPDATE {typeof(T).Name} SET {string.Join(", ", UpdateExprs)} WHERE {autoWhere}";
                    break;
            }
            return sql;
        }
    }

    [TableName("MyThingTable")]
    public class Thing
    {
        //[Column("id")]
        //public int _id;
        //public int id
        //{
        //    get => _id;
        //    set => SetProperty(ref _id, value);
        //}

        [Identity]
        public int id { get; set; }

        [Column("fnzcd")]
        public string ThingCode { get; set; }

        [Column("wjkrqnm")]
        public string ThingName { get; set; }

        // Unusually the DB column name for this value doesn't look like random junk
        public string NotLineNoise { get; set; }
    }

    public class SimpleListThing
    {
        public int id { get; set; }
        public string Name { get; set; }
        public bool Flag { get; set; }
    }
}
