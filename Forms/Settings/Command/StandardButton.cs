﻿using Quality_Control_EF.Forms.Settings.ModelView;
using System;
using System.Windows.Input;

namespace Quality_Control_EF.Forms.Setting.Command
{
    internal class StandardButton : ICommand
    {
        private readonly SettingMV _modelView;

        public StandardButton(SettingMV modelView)
        {
            if (modelView == null) throw new ArgumentNullException("Model widoku jest null");
            _modelView = modelView;
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
            return true;
        }

        public void Execute(object parameter)
        {
            _modelView.Standard();
        }
    }
}
