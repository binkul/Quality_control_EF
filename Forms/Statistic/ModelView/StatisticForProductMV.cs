﻿using GalaSoft.MvvmLight.Command;
using Quality_Control_EF.Commons;
using Quality_Control_EF.Forms.Statistic.Command;
using Quality_Control_EF.Forms.Statistic.Model;
using Quality_Control_EF.Models;
using Quality_Control_EF.Service;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Quality_Control_EF.Forms.Statistic.ModelView
{
    internal class StatisticForProductMV : INotifyPropertyChanged
    {
        private ICommand _saveTodayButton;
        private ICommand _exportToExcelButton;

        public event PropertyChangedEventHandler PropertyChanged;
        private readonly StatisticService _service;
        public QualityControlData ActualControlData { private get; set; }
        public RelayCommand<CancelEventArgs> OnClosingCommand { get; set; }
        public RelayCommand<DataGridCellEditEndingEventArgs> OnCellQualityDataChange { get; set; }


        public StatisticForProductMV(StatisticDto statisticDto)
        {
            _service = new StatisticService(statisticDto);
            OnClosingCommand = new RelayCommand<CancelEventArgs>(OnClosingCommandExecuted);
            OnCellQualityDataChange = new RelayCommand<DataGridCellEditEndingEventArgs>(OnCellQualityDataChangeExecuted);
        }

        public SortableObservableCollection<QualityControlData> ProductData => _service.Statistic;

        public bool IsAnyQuality => ProductData.Count > 0;

        public bool Modified => _service.Statistic.Any(x => x.Modified);

        public List<string> GetActiveFields => _service.GetVisibleColumn;

        private void OnPropertyChanged(params string[] names)
        {
            if (PropertyChanged != null)
            {
                foreach (string name in names)
                    PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        private void OnCellQualityDataChangeExecuted(DataGridCellEditEndingEventArgs e)
        {
            if (ActualControlData != null)
            {
                ActualControlData.Modified = true;
                _service.RecalculateProductData();
                OnPropertyChanged(nameof(ProductData));
            }
        }

        private void OnClosingCommandExecuted(CancelEventArgs e)
        {
            MessageBoxResult ansver = MessageBoxResult.No;

            if (Modified)
            {
                ansver = MessageBox.Show("Dokonano zmian w rekordach. Czy zapisać zmiany?", "Zapis zmian", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            }

            if (ansver == MessageBoxResult.Yes)
            {
                Save();
            }
            else if (ansver == MessageBoxResult.Cancel)
            {
                e.Cancel = true;
            }
        }

        public ICommand TodaySaveButton
        {
            get
            {
                if (_saveTodayButton == null) _saveTodayButton = new ProductSaveButton(this);
                return _saveTodayButton;
            }
        }

        public ICommand ExcelExportButton
        {
            get
            {
                if (_exportToExcelButton == null) _exportToExcelButton = new ExportProductToExcelButton(this);
                return _exportToExcelButton;
            }
        }

        internal void Save()
        {
            _ = _service.Save();
        }

        internal void ExcelExport()
        {
            _service.ExportToExcel();
        }

    }
}
