using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EvaBatch.Web.Helper
{
    public class IdentityAttribute : Attribute
    {

    }

    public class TableNameAttribute : Attribute
    {
        public string Name;

        public TableNameAttribute(string v)
        {
            this.Name = v;
        }
    }

    public class ColumnNameAttribute : Attribute
    {
        public string Name;

        public ColumnNameAttribute(string v)
        {
            this.Name = v;
        }
    }
}