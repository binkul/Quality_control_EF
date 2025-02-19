﻿using Quality_Control_EF.Commons;
using Quality_Control_EF.Converters;
using Quality_Control_EF.Forms.Statistic.Model;
using Quality_Control_EF.Forms.Statistic.ModelView;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Input;

namespace Quality_Control_EF.Forms.Statistic
{
    /// <summary>
    /// Logika interakcji dla klasy StatisticRangeForm.xaml
    /// </summary>
    public partial class StatisticRangeForm : RibbonWindow
    {
        public StatisticRangeForm(StatisticDto statisticDto)
        {
            InitializeComponent();

            lblTitle.Content = statisticDto.Title;
            Height = SystemParameters.PrimaryScreenHeight - 100;
            Width = SystemParameters.PrimaryScreenWidth - 200;

            StatisticRangeMV statisticMV = new StatisticRangeMV(statisticDto);
            DataContext = statisticMV;

            SetColumns(statisticMV);
        }

        private void SetColumns(StatisticRangeMV statisticMV)
        {
            Style headerStyle = (Style)Resources["AllignColmnHeaderCenter"];
            Style cellStyleCenter = (Style)Resources["AllignCellCenter"];
            Style cellStyleLeft = (Style)Resources["AllignCellLeft"];


            foreach (QualityDataColumn data in DefaultData.ColumnData)
            {
                if (data.ColumnHeader.Equals("Doba")) continue;

                DataGridTextColumn column = new DataGridTextColumn
                {
                    HeaderStyle = headerStyle,
                    ElementStyle = data.ColumnHeader.Equals("Wyrób") ? cellStyleLeft : cellStyleCenter,
                    Header = data.ColumnHeader,
                    Width = data.ColumnWidth,
                    IsReadOnly = data.IsReadOnly,
                    CanUserSort = data.CanUserSort
                };

                Binding binding = new Binding(data.EFColumnName)
                {
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                };

                if (data.IsValue)
                {
                    binding.ConverterCulture = System.Globalization.CultureInfo.CurrentCulture;
                    binding.StringFormat = data.ValueFormat;
                    binding.UpdateSourceTrigger = UpdateSourceTrigger.LostFocus;
                    binding.Mode = BindingMode.TwoWay;
                    binding.Converter = new EmptyStringToNullConverter();
                    column.EditingElementStyle = (Style)Resources["DoubleErrorStyle"];
                }
                else if (data.ColumnHeader.Equals("Data"))
                {
                    binding.Mode = BindingMode.TwoWay;
                    binding.Converter = new DateTimeConverter();
                    column.IsReadOnly = true;
                }
                column.Binding = binding;

                if (!data.IsAlwaysVisible)
                {
                    Binding visible = new Binding
                    {
                        Source = statisticMV,
                        Path = new PropertyPath("GetActiveFields"),
                        Mode = BindingMode.OneWay,
                        Converter = new ColumnVisibilityConverter(),
                        ConverterParameter = data.DBColumnName
                    };

                    _ = BindingOperations.SetBinding(column, DataGridColumn.VisibilityProperty, visible);
                }

                DgQualityData.Columns.Add(column);
            }
        }

        private void DgQualityData_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Right)
            {
                // Cancel [Enter] key event.
                e.Handled = true;
                // Press [Tab] key programatically.
                KeyEventArgs tabKeyEvent = new KeyEventArgs(
                  e.KeyboardDevice, e.InputSource, e.Timestamp, Key.Tab)
                {
                    RoutedEvent = Keyboard.KeyDownEvent
                };
                _ = InputManager.Current.ProcessInput(tabKeyEvent);
            }
        }

    }
}
