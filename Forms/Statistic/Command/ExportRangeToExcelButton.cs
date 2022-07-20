﻿using Quality_Control_EF.Forms.Statistic.ModelView;
using System;
using System.Windows.Input;

namespace Quality_Control_EF.Forms.Statistic.Command
{
    internal class ExportRangeToExcelButton : ICommand
    {
        private readonly StatisticRangeMV _modelView;

        public ExportRangeToExcelButton(StatisticRangeMV modelView)
        {
            _modelView = modelView ?? throw new ArgumentNullException("Model widoku jest null");
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        public bool CanExecute(object parameter)
        {
            return _modelView.IsAnyQuality;
        }

        public void Execute(object parameter)
        {
            _modelView.ExcelExport();
        }
    }
}
