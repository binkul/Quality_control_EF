﻿using Quality_Control_EF.Forms.Modification.ModelView;
using System;
using System.Windows.Input;

namespace Quality_Control_EF.Forms.Modification.Command
{
    internal class SetButton : ICommand
    {
        private readonly ModificationMV _modelView;

        public SetButton(ModificationMV modelView)
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
            return _modelView.IsAnyUnset;
        }

        public void Execute(object parameter)
        {
            _modelView.Set();
        }
    }
}
