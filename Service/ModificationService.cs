﻿using Quality_Control_EF.Commons;
using Quality_Control_EF.Forms.InputBox;
using Quality_Control_EF.Forms.Modification.Model;
using Quality_Control_EF.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;

namespace Quality_Control_EF.Service
{
    internal class ModificationService
    {
        private readonly QualityFieldsModel _fields = new QualityFieldsModel();

        public List<ModificationModel> Fields => _fields.Fields;

        internal bool Modified => Fields.Any(x => x.Modify);

        internal bool IsAnySet => Fields.Any(x => x.Visible);

        internal bool IsAnyUnSet => Fields.Any(x => x.Visible == false);

        internal void CheckFieldsInList(string fieldsList)
        {
            string[] fields = fieldsList.Split('|');
            foreach (string field in fields)
            {
                ModificationModel model = Fields.FirstOrDefault(x => x.DbName.Equals(field));
                if (model != null)
                    model.Visible = true;
            }
            ModificationModel stdModel = Fields.FirstOrDefault(x => x.DbName.Equals("measure_date"));
            stdModel.Visible = true;
        }

        internal void CancelModify()
        {
            foreach (ModificationModel model in Fields)
            {
                model.Modify = false;
            }
        }

        internal void UnModifiedAll()
        {
            foreach (ModificationModel model in Fields)
            {
                model.Modify = false;
            }
        }

        internal void UnsetFields()
        {
            foreach (ModificationModel model in Fields)
            {
                if (model.DbName.Equals("measure_date"))
                    model.Visible = true;
                else
                    model.Visible = false;
            }
        }

        internal void SetFields()
        {
            foreach (ModificationModel model in Fields)
            {
                model.Visible = true;
            }
        }

        internal void SetStandard()
        {
            CheckFieldsInList(DefaultData.DefaultDataFieldsNoPh);
        }

        internal void SetStandardWithpH()
        {
            CheckFieldsInList(DefaultData.DefaultDataFields);
        }

        internal void Copy()
        {
            LabBookContext contex = new LabBookContext();
            InputBox inputBox = new InputBox("Podaj numer D do kopiowania pól wyboru:", "Numer D");

            if (inputBox.ShowDialog() == true)
            {
                string number = inputBox.Answer;
                string template = "^[0-9]*$";
                Regex pattern = new Regex(template);

                if (!pattern.IsMatch(number) && number.Length > 0)
                {
                    _ = MessageBox.Show("Wprowadzona wartość nie jest liczbą całkowitą'" + number + "'",
                        "Błąd wartości", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                bool exist = contex.QualityControlFields.Any(x => x.LabbookId == long.Parse(number));
                if (!exist)
                {
                    _ = MessageBox.Show("Wprowadzony numer D-" + number + "'" + " nie ma zdefiniowanych aktywnych pól.",
                        "Błąd wartości", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                var activeFields = contex.QualityControlFields
                    .Where(x => x.LabbookId == long.Parse(number))
                    .Select(x => x.ActiveFields)
                    .FirstOrDefault();
                CheckFieldsInList(activeFields);
            }
        }

        internal string RecalculateFields()
        {
            return Fields
                .Where(x => x.Visible)
                .Select(x => x.DbName)
                .Aggregate((x, y) => x + "|" + y);
        }

    }
}
