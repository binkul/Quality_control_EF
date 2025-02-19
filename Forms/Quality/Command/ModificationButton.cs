﻿using Quality_Control_EF.Forms.Quality.ModelView;
using System;
using System.Windows.Input;

namespace Quality_Control_EF.Forms.Quality.Command
{
    internal class ModificationButton : ICommand
    {
        private readonly QualityMV _modelView;

        public ModificationButton(QualityMV modelView)
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
            return _modelView.IsAnyQuality;
        }

        public void Execute(object parameter)
        {
            _modelView.ModifiyFields();
        }
    }
}
