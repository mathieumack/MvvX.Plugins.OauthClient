using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Expression.Media;

namespace MvvX.Plugins.OAuthClient
{
    /// <summary>
    /// Source code : https://github.com/MrMitch/WPF-Loaders.git​​​​​​​
    /// </summary>
    public class Loader : Control
    {
        #region IsIndeterminate
        public static readonly DependencyProperty IsIndeterminateProperty = DependencyProperty.Register(
            "IsIndeterminate",
            typeof(bool),
            typeof(Loader),
            new PropertyMetadata(default(bool))
        );

        public bool IsIndeterminate
        {
            get { return (bool)GetValue(IsIndeterminateProperty); }
            set { SetValue(IsIndeterminateProperty, value); }
        }
        #endregion

        #region ThicknessUnit
        public static readonly DependencyProperty ThicknessUnitProperty = DependencyProperty.Register(
            "ThicknessUnit",
            typeof(UnitType),
            typeof(Loader),
            new PropertyMetadata(default(UnitType))
        );

        public UnitType ThicknessUnit
        {
            get { return (UnitType)GetValue(ThicknessUnitProperty); }
            set { SetValue(ThicknessUnitProperty, value); }
        }
        #endregion

        #region Thickness
        public static readonly DependencyProperty ThicknessProperty = DependencyProperty.Register(
            "Thickness",
            typeof(double),
            typeof(Loader),
            new PropertyMetadata(default(double))
        );

        public double Thickness
        {
            get { return (double)GetValue(ThicknessProperty); }
            set { SetValue(ThicknessProperty, value); }
        }
        #endregion

        #region Fill
        public static readonly DependencyProperty FillProperty = DependencyProperty.Register(
            "Fill", typeof(Brush),
            typeof(Loader),
            new PropertyMetadata(default(Brush))
        );

        public Brush Fill
        {
            get { return (Brush)GetValue(FillProperty); }
            set { SetValue(FillProperty, value); }
        }
        #endregion


        static Loader()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Loader), new FrameworkPropertyMetadata(typeof(Loader)));
        }
    }
}
