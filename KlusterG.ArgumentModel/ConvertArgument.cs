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

            if (((value.Count() > 0 && value.Substring(0, 1).Equals("-")) || 
                (value.Count() > 1 && value.Substring(0, 2).Equals("--"))) && 
                property.PropertyType != typeof(bool))
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
        }
    }
}