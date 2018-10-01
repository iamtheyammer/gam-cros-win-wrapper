using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using WpfTextBox = System.Windows.Controls.TextBox;

namespace ChromebookGUI
{
    /// <summary>
    /// A class for holding information about an Organizational Unit, specifically OrgUnitPath, OrgUnitName and OrgUnitDescription.
    /// </summary>
    public class OrgUnit
    {
        public string OrgUnitPath { get; set; }
        public string OrgUnitName { get; set; }
        public string OrgUnitDescription { get; set; }
    }

    /// <summary>
    /// A class for holding information (specifically DeviceId, LastSync, SerialNumber, Status and Notes, plus Error and ErrorText) about a Chrome device.
    /// </summary>
    public class BasicDeviceInfo
    {
        public string DeviceId { get; set; }
        public string LastSync { get; set; }
        public string SerialNumber { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }
        public string AssetId { get; set; }
        public bool Error { get; set; }
        public string ErrorText { get; set; }
    }

    /// <summary>
    /// This, from the link below, allows us to add placeholder text
    /// https://c-sharp-snippets.blogspot.com/2007/12/textbox-with-placeholder-text.html
    /// </summary>
    public class TextBox : WpfTextBox
    {
        /// <summary>
        ///   Keeps track of whether placeholder text is visible to know when to call InvalidateVisual to show or hide it.
        /// </summary>
        private bool _isPlaceholderVisible;

        /// <summary>
        ///   Identifies the PlaceholderText dependency property.
        /// </summary>
        public static readonly DependencyProperty PlaceholderTextProperty = DependencyProperty.Register(
            "PlaceholderText",
            typeof(string),
            typeof(TextBox),
            new FrameworkPropertyMetadata(
                string.Empty,
                FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>
        ///   Gets or sets the placeholder text to be shown when text box has no text and is not in focus. This is a dependency property.
        /// </summary>
        public string PlaceholderText
        {
            get { return (string)GetValue(PlaceholderTextProperty); }
            set { SetValue(PlaceholderTextProperty, value); }
        }

        // Shadowed BackgroundProperty to disassociate base.Backgrouond and this.Background.
        /// <summary>
        ///   Identifies the Background dependency property.
        /// </summary>
        public new static readonly DependencyProperty BackgroundProperty = DependencyProperty.Register(
            "Background",
            typeof(Brush),
            typeof(TextBox),
            new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.AffectsRender));

        // Shadowed Background property to keep base.Background null.
        /// <summary>
        ///   Gets or sets a brush that describes the background of a control. This is a  dependency property.
        /// </summary>
        public new Brush Background
        {
            get { return GetValue(BackgroundProperty) as Brush; }
            set { SetValue(BackgroundProperty, value); }
        }

        // Sets the base.Background to null to make it transparent. New background is painted in OnRender.
        /// <summary>
        ///   Raises the Initialized event. This method is invoked whenever IsInitialized is set to true internally.
        /// </summary>
        /// <param name="e">
        ///   The EventArgs that contains the event data.
        /// </param>
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            if (Background == null)
                Background = base.Background;
            base.Background = null;
        }

        // Listen to changes in IsFocusedProperty and TextProperty and invalidates visual when placeholder text needs to be shown or hidden.
        /// <summary>
        ///   Called when one or more of the dependency properties that exist on the element have had their effective values changed.
        ///   (Overrides FrameworkElement.OnPropertyChanged(DependencyPropertyChangedEventArgs).)
        /// </summary>
        /// <param name="e">
        ///   The DependencyPropertyChangedEventArgs that contains the event data.
        /// </param>
        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if ((e.Property == IsFocusedProperty || e.Property == TextProperty) && !string.IsNullOrEmpty(PlaceholderText))
                if (!IsFocused && string.IsNullOrEmpty(Text))
                {
                    // Need to show placeholder
                    if (!_isPlaceholderVisible)
                        InvalidateVisual();
                }
                else if (_isPlaceholderVisible)
                    // Need to hide placeholder
                    InvalidateVisual();
            base.OnPropertyChanged(e);
        }

        // Draws background and placeholder text of the TextBox.
        /// <summary>
        ///   When overridden in a derived class, participates in rendering operations that are directed by the layout system.
        ///   The rendering instructions for this element are not used directly when this method is invoked, and are instead
        ///   preserved for later asynchronous use by layout and drawing.
        /// </summary>
        /// <param name="drawingContext">
        ///   The drawing instructions for a specific element. This context is provided to the layout system.
        /// </param>
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            _isPlaceholderVisible = false;
            drawingContext.DrawRectangle(Background, null, new Rect(RenderSize));

            if (!IsFocused && string.IsNullOrEmpty(Text) && !string.IsNullOrEmpty(PlaceholderText))
            {
                // Draw placeholder

                _isPlaceholderVisible = true;
                TextAlignment computedTextAlignment = ComputedTextAlignment();
                // foreground brush does not need to be dynamic. OnRender called when SystemColors changes.
                Brush foreground = SystemColors.GrayTextBrush.Clone();
                foreground.Opacity = Foreground.Opacity;
                Typeface typeface = new Typeface(FontFamily, /*FontStyles.Italic*/ this.FontStyle, FontWeight, FontStretch);
                FormattedText formattedText = new FormattedText(PlaceholderText,
                                                    CultureInfo.CurrentCulture,
                                                    FlowDirection,
                                                    typeface,
                                                    FontSize,
                                                    foreground);
                formattedText.TextAlignment = computedTextAlignment;
                formattedText.MaxTextHeight = RenderSize.Height - BorderThickness.Top - BorderThickness.Bottom - Padding.Top - Padding.Bottom;
                formattedText.MaxTextWidth = RenderSize.Width - BorderThickness.Left - BorderThickness.Right - Padding.Left - Padding.Right - 4.0;

                double left;
                double top = 0.0;
                if (FlowDirection == FlowDirection.RightToLeft)
                    left = BorderThickness.Right + Padding.Right + 2.0;
                else
                    left = BorderThickness.Left + Padding.Left + 2.0;
                switch (VerticalContentAlignment)
                {
                    case VerticalAlignment.Top:
                    case VerticalAlignment.Stretch:
                        top = BorderThickness.Top + Padding.Top;
                        break;
                    case VerticalAlignment.Bottom:
                        top = RenderSize.Height - BorderThickness.Bottom - Padding.Bottom - formattedText.Height;
                        break;
                    case VerticalAlignment.Center:
                        top = (RenderSize.Height + BorderThickness.Top - BorderThickness.Bottom + Padding.Top - Padding.Bottom - formattedText.Height) / 2.0;
                        break;
                }
                if (FlowDirection == FlowDirection.RightToLeft)
                {
                    // Somehow everything got drawn reflected. Add a transform to correct.
                    drawingContext.PushTransform(new ScaleTransform(-1.0, 1.0, RenderSize.Width / 2.0, 0.0));
                    drawingContext.DrawText(formattedText, new Point(left, top));
                    drawingContext.Pop();
                }
                else
                    drawingContext.DrawText(formattedText, new Point(left, top));
            }
        }

        /// <summary>
        ///   Computes changes in text alignment caused by HorizontalContentAlignment. TextAlignment has priority over HorizontalContentAlignment.
        /// </summary>
        /// <returns>
        ///   Returns the effective text alignment.
        /// </returns>
        private TextAlignment ComputedTextAlignment()
        {
            if (DependencyPropertyHelper.GetValueSource(this, TextBox.HorizontalContentAlignmentProperty).BaseValueSource == BaseValueSource.Local
                && DependencyPropertyHelper.GetValueSource(this, TextBox.TextAlignmentProperty).BaseValueSource != BaseValueSource.Local)
            {
                // HorizontalContentAlignment dominates
                switch (HorizontalContentAlignment)
                {
                    case HorizontalAlignment.Left:
                        return TextAlignment.Left;
                    case HorizontalAlignment.Right:
                        return TextAlignment.Right;
                    case HorizontalAlignment.Center:
                        return TextAlignment.Center;
                    case HorizontalAlignment.Stretch:
                        return TextAlignment.Justify;
                }
            }
            return TextAlignment;
        }
    }
}
