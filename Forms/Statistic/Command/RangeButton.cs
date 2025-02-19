﻿using Quality_Control_EF.Forms.Statistic.ModelView;
using System;
using System.Windows.Input;

namespace Quality_Control_EF.Forms.Statistic.Command
{
    internal class RangeButton : ICommand
    {
        private readonly StatisticMV _modelView;

        public RangeButton(StatisticMV modelView)
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
            return _modelView.RangeDateEnable;
        }

        public void Execute(object parameter)
        {
            _modelView.ShowRange();
        }
    }
}
