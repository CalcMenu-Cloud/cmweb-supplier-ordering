using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace OrderingAPI.Utility
{
    public sealed class Util
    {
        // Method to convert DataTable to list of class objects
      public  static List<T> ConvertDataTableToList<T>( DataTable table) where T : new()
        {
            List<T> list = new List<T>();

            foreach (DataRow row in table.Rows)
            {
                T obj = new T();

                foreach (DataColumn col in table.Columns)
                {
                    // Get property matching column name
                    var prop = typeof(T).GetProperty(col.ColumnName);

                    // Check if property exists
                    if (prop != null && row[col] != DBNull.Value)
                    {
                        // Set property value
                        prop.SetValue(obj, row[col]);
                    }
                }

                // Add object to list
                list.Add(obj);
            }

            return list;
        }
    }
}
