using KlusterG.ArgumentModel.Attributes;

namespace KlusterG.ArgumentModel.Sample.Models
{
    public class Person
    {
        [Argument(isRequired: true, name: "--name", abbreviation: "-n")]
        public string Name { get; set; }
        [Argument(isRequired: true, name: "--age", abbreviation: "-a")]
        public int Age { get; set; }
        [Argument(isRequired: false, name: "--alive", abbreviation: "-l")]
        public bool Alive { get; set; }
    }
}
