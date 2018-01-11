using Windows.UI.Input.Inking;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Behrlo.Helpers
{
    public class InkCanvasBinding
    {
        #region InkStrokes

        public static InkStrokeContainer GetInkStrokes(DependencyObject obj) =>
            obj.GetValue(InkStrokesProperty) as InkStrokeContainer;

        public static void SetInkStrokes(DependencyObject obj, InkStrokeContainer value) =>
            obj.SetValue(InkStrokesProperty, value);

        public static DependencyProperty InkStrokesProperty = DependencyProperty.RegisterAttached(
            "InkStrokes", typeof(InkStrokeContainer), typeof(InkCanvasBinding),
            new PropertyMetadata(null, InkStrokesProperty_PropertyChanged));

        private static void InkStrokesProperty_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is InkCanvas inkCanvas)
                inkCanvas.InkPresenter.StrokeContainer = e.NewValue as InkStrokeContainer;
        }

        #endregion InkStrokes

        #region IsInputEnabled

        public static bool GetIsInputEnabled(DependencyObject obj) =>
            (bool)obj.GetValue(IsInputEnabledProperty);

        public static void SetIsInputEnabled(DependencyObject obj, bool value) =>
            obj.SetValue(IsInputEnabledProperty, value);

        public static DependencyProperty IsInputEnabledProperty = DependencyProperty.RegisterAttached(
            "IsInputEnabled", typeof(bool), typeof(InkCanvasBinding),
            new PropertyMetadata(null, IsInputEnabledProperty_PropertyChanged));

        private static void IsInputEnabledProperty_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is InkCanvas inkCanvas)
                inkCanvas.InkPresenter.IsInputEnabled = (bool)e.NewValue;
        }

        #endregion IsInputEnabled
    }
}
