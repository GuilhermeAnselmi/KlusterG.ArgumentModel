using KlusterG.ArgumentModel.Attributes;

namespace KlusterG.ArgumentModel.Sample.Models
{
    public class Person
    {
        public string Temp { get; set; }
        [Argument(isRequired: true, name: "--name", abbreviation: "-n")]
        public string Name { get; set; }
        [Argument(isRequired: true, name: "--age", abbreviation: "-a")]
        public int Age { get; set; }
        [Argument(isRequired: false, name: "--alive", abbreviation: "-l")]
        public bool Alive { get; set; }
        [Argument(isRequired: false, name: "--sbyte", abbreviation: "-s")]
        public sbyte SByte { get; set; }
        [Argument(isRequired: false, name: "--value", abbreviation: "-v")]
        public double Value { get; set; }
        [Argument(isRequired: false, name: "--date", abbreviation: "-d")]
        public DateTime Date { get; set; }
    }
}
