﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows;
using System.Windows.Controls;

namespace DaxStudio.UI.TemplateSelectors
{
    public class ResultsGridCellTemplateSelector:DataTemplateSelector
    {
        public DataTemplate DefaultTemplate { get; set; }
        public DataTemplate SpidTemplate { get; set; }
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            System.Diagnostics.Debug.WriteLine("Selecting Cell template");
            return base.SelectTemplate(item, container);
        }
    }
}
