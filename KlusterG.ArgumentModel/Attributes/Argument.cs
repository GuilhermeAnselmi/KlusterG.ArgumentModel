namespace KlusterG.ArgumentModel.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class Argument : Attribute
    {
        public string Name { get; }
        public string Abbreviation { get; }
        public bool IsRequired { get; }

        public Argument(bool isRequired, string name, string abbreviation = null)
        {
            IsRequired = isRequired;
            Name = $"--{name.Replace("-", "")}";
            Abbreviation = abbreviation != null ? $"-{abbreviation.Replace("-", "")}" : null;
        }
    }
}
