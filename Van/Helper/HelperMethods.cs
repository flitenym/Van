using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Van.AbstractClasses;
using Van.Helper.Attributes;
using Van.Helper.StaticInfo;
using Van.ViewModel.Provider;
using Van.Windows.ViewModel;

namespace Van.Helper
{
    public static class HelperMethods
    {
        #region SnackBar

        /// <summary>
        /// Отправить сообщение через SnackBar
        /// </summary>
        /// <param name="content">Сообщение</param>
        /// <param name="isNoDuplicateConsider">Если true и будет дубликаты сообщений, то они каждый все равно вызовет новое уведомление, если false то выйдет повторное сообщение 1 раз</param>
        public static async Task Message(string content, bool isNoDuplicateConsider = false)
        {
            await Task.Run(
                () =>
                {
                    if (SharedProvider.GetFromDictionaryByKeyAsync(nameof(MainWindowViewModel)) is MainWindowViewModel mainWindowViewModel)
                    {
                        mainWindowViewModel.IsMessagePanelContent.Enqueue(
                        content,
                        "OK",
                        param => Trace.WriteLine("Actioned: " + param),
                        null,
                        false,
                        isNoDuplicateConsider);
                    }
                });
        }

        #endregion

        #region DataGrid and DataTable

        public static void DataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            // В случае если референсы или сами ID то будем скрывать, они не нужны пользователю 
            if (!(sender is DataGrid dGrid)) return;
            if (!(dGrid.ItemsSource is DataView view)) return;
            var table = view.Table;
            var column = table.Columns[e.Column.Header as string];
            e.Column.Header = table.Columns[e.Column.Header as string].Caption;

            if (GetProperty(column, InfoKeys.ExtendedPropertiesKey)) {
                e.Column.Visibility = Visibility.Collapsed;
            }
        }

        public static void SetProperty(DataColumn column, string key)
        {
            column.ExtendedProperties.Add(key, true);
        }

        public static bool GetProperty(DataColumn column, string key)
        {
            if ((bool?)column.ExtendedProperties[key] is bool)
                return true;
            return false;
        }

        public static DataTable ToDataTable<T>(this IList<T> data, Type type)
        {
            DataTable table = new DataTable();

            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(type);

            foreach (PropertyDescriptor prop in properties)
            {
                DataColumn column = new DataColumn(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType)
                {
                    Caption = string.IsNullOrEmpty(prop.Description) ? prop.Name : prop.Description
                };

                for (int i = 0; i < prop.Attributes.Count; i++)
                {
                    if (prop.Attributes[i].GetType() == typeof(ColumnDataAttribute))
                    {
                        SetProperty(column, InfoKeys.ExtendedPropertiesKey);
                    }
                }

                table.Columns.Add(column);
            }

            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;
        }

        public static object ToObject(this DataRow row, Type type)
        {
            var expandoDict = new ExpandoObject() as IDictionary<string, object>;
            foreach (PropertyInfo property in type.GetProperties())
            {
                if (row.Table.Columns.Contains(property.Name) && property.DeclaringType != typeof(ModelClass))
                {
                    expandoDict.Add(property.Name.ToString(), row[property.Name] == DBNull.Value ? null : row[property.Name]);
                }
            }

            return expandoDict;
        }

        public static object ToObjectLoad(this DataRow row, Type type)
        {
            var expandoDict = new ExpandoObject() as IDictionary<string, object>;
            var properties = type.GetProperties();

            int j = 0;
            for (int i = 0; i < properties.Length; i++)
            {
                var columnDataAttributes = (ColumnDataAttribute[])properties[i].GetCustomAttributes(typeof(ColumnDataAttribute), true);
                if (columnDataAttributes.Length == 0 && 
                    properties[i].DeclaringType != typeof(ModelClass) &&
                    properties[i].CanWrite)
                {
                    var columnName = row.Table.Columns[j].ColumnName;
                    expandoDict.Add(properties[i].Name.ToString(), row[columnName] == DBNull.Value ? null : row[columnName]);
                    j++;
                }
            }

            return expandoDict;
        }

        public static bool ClearDataTable(DataTable dataTable, object value)
        {
            if (value == null) return false;

            dataTable?.Clear();
            dataTable?.Columns.Clear();
            dataTable?.Rows.Clear();

            return true;
        }

        #endregion

        #region Clone

        /// <summary>
        /// Клонирование Списка
        /// </summary>
        public static List<T> Clone<T>(this List<T> listToClone) where T : ICloneable
        {
            return listToClone.Select(item => (T)item.Clone()).ToList();
        }

        #endregion
    }
}
