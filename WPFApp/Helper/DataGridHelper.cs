using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace WPFApp.Helper
{
    public class DataGridHelper : DataGrid
    {
        public static readonly DependencyProperty SelectedCellsProperty =
        DependencyProperty.Register("SelectedCells", typeof(Dictionary<object, List<int>>), typeof(DataGridHelper), new PropertyMetadata(default(Dictionary<object, List<int>>)));

        public new Dictionary<object, List<int>> SelectedCells
        {
            get { return (Dictionary<object, List<int>>)GetValue(SelectedCellsProperty); }
            set { throw new Exception("This property is read-only. To bind to it you must use 'Mode=OneWayToSource'."); }
        }

        protected override void OnSelectedCellsChanged(SelectedCellsChangedEventArgs e)
        {
            base.OnSelectedCellsChanged(e);
            var dictionary = new Dictionary<object, List<int>>();
            foreach (DataGridCellInfo ci in base.SelectedCells)
            {
                if (!dictionary.ContainsKey(ci.Item)) dictionary.Add(ci.Item, new List<int>());

                dictionary[ci.Item].Add(ci.Column.DisplayIndex);
            }

            SetValue(SelectedCellsProperty, dictionary);
        }
    }
}
