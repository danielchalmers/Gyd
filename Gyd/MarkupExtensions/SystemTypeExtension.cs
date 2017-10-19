using System;
using System.Windows.Markup;

namespace Gyd.MarkupExtensions
{
    public class SystemTypeExtension : MarkupExtension
    {
        private object _value;

        public bool Bool
        {
            set => _value = value;
        }

        public double Double
        {
            set => _value = value;
        }

        public int Int
        {
            set => _value = value;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _value;
        }
    }
}