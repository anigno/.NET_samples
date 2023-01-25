#region

using System;
using System.Windows.Media;
using System.Windows;

#endregion

namespace ConverterExamples.Converters
{
    public class BoolToStringConverter : BoolToValueConverter<String>
    {
    }

    public class BoolToBrushConverter : BoolToValueConverter<Brush>
    {
    }

    public class BoolToVisibilityConverter : BoolToValueConverter<Visibility>
    {
    }

    public class BoolToObjectConverter : BoolToValueConverter<Object>
    {
    }
}

//Usage
//<TextBlock Text="{Binding Path=MyBoolValue, Converter={StaticResource BooleanToStringConverter}}" /> 

//<local:BoolToStringConverter x:Key="BooleanToStringConverter" FalseValue="No" TrueValue="Yes" />
//<local:BoolToBrushConverter x:Key="Highlighter" FalseValue="Transparent" TrueValue="Yellow" />
//<local:BoolToStringConverter x:Key="CYesNo" FalseValue="No" TrueValue="Yes" />
//<local:BoolToVisibilityConverter x:Key="InverseVisibility" TrueValue="Collapsed" FalseValue="Visible" />