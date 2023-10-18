using KlusterG.ArgumentModel.Attributes;
using System.ComponentModel;
using System.Reflection;

namespace KlusterG.ArgumentModel
{
    public static class ConvertArgument
    {
        public static T DefineModel<T>(string[] args) where T : new()
        {
            T model = new T();

            var properties = typeof(T).GetProperties();
            var allArguments = properties.Select(x => x.GetCustomAttributes(typeof(Argument), false).FirstOrDefault()).Where(x => x != null).ToList();
            
            List<Argument> arguments = new List<Argument>();

            allArguments.ForEach(argument =>
            {
                arguments.Add((Argument)argument);
            });


            foreach (var property in properties)
            {
                var argument = (Argument)property.GetCustomAttributes(typeof(Argument), false).FirstOrDefault();

                if (argument == default)
                    continue;

                if (args.Contains(argument.Name) && args.Contains(argument.Abbreviation))
                    throw new ArgumentException($"The command line cannot have the same name and abbreviation. {argument.Name}, {argument.Abbreviation}");

                if (arguments.Count(x => x.Name == argument.Name) > 1)
                    throw new ArgumentException($"Repeating arguments is not allowed. {argument.Name}");

                if (arguments.Count(x => x.Abbreviation == argument.Abbreviation) > 1)
                    throw new ArgumentException($"Repeating arguments is not allowed. {argument.Abbreviation}");

                if (args.Contains(argument.Name))
                {
                    DefineProperty<T>(args.ToList(), property, model, argument.Name);

                    continue;
                }

                if (!string.IsNullOrEmpty(argument.Abbreviation) && args.Contains(argument.Abbreviation))
                {
                    DefineProperty<T>(args.ToList(), property, model, argument.Abbreviation);

                    continue;
                }

                if (argument.IsRequired)
                    throw new ArgumentException($"The {argument.Name} or {argument.Abbreviation} argument is mandatory.");
            }

            return model;
        }

        private static void DefineProperty<T>(List<string> args, PropertyInfo property, T model, string argumentName)
        {
            var value = args[args.IndexOf(argumentName) + 1];

            if (value.Contains("-") && property.PropertyType != typeof(bool))
                throw new ArgumentException($"No value was passed to {argumentName}");

            if (property.PropertyType == typeof(bool))
            {
                property.SetValue(model, true);

                return;
            }

            var type = property.PropertyType;
            var converter = TypeDescriptor.GetConverter(type);

            if (converter != null && converter.CanConvertFrom(typeof(string)))
            {
                var convertedValue = converter.ConvertFrom(value);

                property.SetValue(model, convertedValue);
            }

            return;

            if (property.PropertyType == typeof(bool)) property.SetValue(model, true);
            else if (property.PropertyType == typeof(sbyte)) property.SetValue(model, sbyte.Parse(value));
            else if (property.PropertyType == typeof(byte)) property.SetValue(model, byte.Parse(value));
            else if (property.PropertyType == typeof(short)) property.SetValue(model, short.Parse(value));
            else if (property.PropertyType == typeof(ushort)) property.SetValue(model, ushort.Parse(value));
            else if (property.PropertyType == typeof(int)) property.SetValue(model, int.Parse(value));
            else if (property.PropertyType == typeof(uint)) property.SetValue(model, uint.Parse(value));
            else if (property.PropertyType == typeof(long)) property.SetValue(model, long.Parse(value));
            else if (property.PropertyType == typeof(ulong)) property.SetValue(model, ulong.Parse(value));
            else if (property.PropertyType == typeof(float)) property.SetValue(model, float.Parse(value));
            else if (property.PropertyType == typeof(double)) property.SetValue(model, double.Parse(value));
            else if (property.PropertyType == typeof(decimal)) property.SetValue(model, decimal.Parse(value));
            else if (property.PropertyType == typeof(char)) property.SetValue(model, char.Parse(value));
            else if (property.PropertyType == typeof(DateTime)) property.SetValue(model, DateTime.Parse(value));
            else if (property.PropertyType == typeof(IntPtr)) property.SetValue(model, IntPtr.Parse(value));
            else if (property.PropertyType == typeof(UIntPtr)) property.SetValue(model, UIntPtr.Parse(value));
            else property.SetValue(model, value);
        }
    }
}