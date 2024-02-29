using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using Newtonsoft.Json;
using ProjetDevSys;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ProjetDevSysGraphical
{
    public static class AppConstants
    {
        public static string OpenFolderDialog()
        {
            using (CommonOpenFileDialog dialog = new CommonOpenFileDialog())
            {
                dialog.IsFolderPicker = true;
                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    return dialog.FileName;
                }
            }
            App.Current.MainWindow.Activate();
            return "";
        }
        public static string OpenFileDialog()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                return openFileDialog.FileName;
            }
            return "";
        }

        public static Style GridButtonStyle()
        {
            Style navigationButtonStyle = new Style(typeof(Button));

            //Property setters
            navigationButtonStyle.Setters.Add(new Setter(Button.BackgroundProperty, Brushes.Transparent));
            navigationButtonStyle.Setters.Add(new Setter(Button.ForegroundProperty, Application.Current.Resources["Brush3"]));
            navigationButtonStyle.Setters.Add(new Setter(Button.FontWeightProperty, FontWeights.Bold));
            navigationButtonStyle.Setters.Add(new Setter(Button.FontSizeProperty, Application.Current.Resources["FontSizeNavigationButton"]));
            navigationButtonStyle.Setters.Add(new Setter(Button.FontFamilyProperty, Application.Current.Resources["FontButton"])); 
            navigationButtonStyle.Setters.Add(new Setter(Button.PaddingProperty, new Thickness(12, 10, 12, 10)));
            navigationButtonStyle.Setters.Add(new Setter(Button.BorderThicknessProperty, new Thickness(2)));
            navigationButtonStyle.Setters.Add(new Setter(Button.CursorProperty, Cursors.Hand));
            navigationButtonStyle.Setters.Add(new Setter(Button.OpacityProperty, 0.8));

            //Template for sub-border and content presenter
            ControlTemplate template = new ControlTemplate(typeof(Button));
            FrameworkElementFactory border = new FrameworkElementFactory(typeof(Border));
            border.SetValue(Border.CornerRadiusProperty, new CornerRadius(20));
            border.SetValue(Border.BorderThicknessProperty, new Thickness(2));
            border.SetValue(Border.BorderBrushProperty, Application.Current.Resources["Brush4"]);
            border.SetValue(Border.BackgroundProperty, new TemplateBindingExtension(Button.BackgroundProperty));
            FrameworkElementFactory contentPresenter = new FrameworkElementFactory(typeof(ContentPresenter));
            contentPresenter.SetValue(ContentPresenter.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            contentPresenter.SetValue(ContentPresenter.VerticalAlignmentProperty, VerticalAlignment.Center);
            border.AppendChild(contentPresenter);
            template.VisualTree = border;
            navigationButtonStyle.Setters.Add(new Setter(Button.TemplateProperty, template));

            return navigationButtonStyle;
        }
    }
}
