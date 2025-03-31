namespace Orbital7.Extensions.Data;

public class LetteredNames
{
    public class NamesLetter
    {
        public string Letter { get; set; }
        public List<object> Names { get; set; }

        public bool HasNames
        {
            get { return Names.Count > 0; }
        }

        public NamesLetter()
        {
            Names = new List<object>();
        }

        public NamesLetter(string letter)
            : this()
        {
            Letter = letter;
        }

        public override string ToString()
        {
            return Letter + " (" + Names.Count + ")";
        }
    }

    public List<NamesLetter> NamesLetterCollection { get; set; }

    public LetteredNames()
    {
        // Create.
        NamesLetterCollection = new List<NamesLetter>();
        NamesLetterCollection.Add(new NamesLetter("A"));
        NamesLetterCollection.Add(new NamesLetter("B"));
        NamesLetterCollection.Add(new NamesLetter("C"));
        NamesLetterCollection.Add(new NamesLetter("D"));
        NamesLetterCollection.Add(new NamesLetter("E"));
        NamesLetterCollection.Add(new NamesLetter("F"));
        NamesLetterCollection.Add(new NamesLetter("G"));
        NamesLetterCollection.Add(new NamesLetter("H"));
        NamesLetterCollection.Add(new NamesLetter("I"));
        NamesLetterCollection.Add(new NamesLetter("J"));
        NamesLetterCollection.Add(new NamesLetter("K"));
        NamesLetterCollection.Add(new NamesLetter("L"));
        NamesLetterCollection.Add(new NamesLetter("M"));
        NamesLetterCollection.Add(new NamesLetter("N"));
        NamesLetterCollection.Add(new NamesLetter("O"));
        NamesLetterCollection.Add(new NamesLetter("P"));
        NamesLetterCollection.Add(new NamesLetter("Q"));
        NamesLetterCollection.Add(new NamesLetter("R"));
        NamesLetterCollection.Add(new NamesLetter("S"));
        NamesLetterCollection.Add(new NamesLetter("T"));
        NamesLetterCollection.Add(new NamesLetter("U"));
        NamesLetterCollection.Add(new NamesLetter("V"));
        NamesLetterCollection.Add(new NamesLetter("W"));
        NamesLetterCollection.Add(new NamesLetter("X"));
        NamesLetterCollection.Add(new NamesLetter("Y"));
        NamesLetterCollection.Add(new NamesLetter("Z"));
        NamesLetterCollection.Add(new NamesLetter("#"));
    }

    public LetteredNames(List<object> items)
        : this()
    {
        foreach (object item in items)
            Find(item).Names.Add(item);
    }

    private NamesLetter Find(object item)
    {
        NamesLetter target = null;

        // Search.
        string name = item.ToString();
        if (!string.IsNullOrEmpty(name))
        {
            string firstLetter = name.Substring(0, 1).ToUpper();
            foreach (NamesLetter namesLetter in NamesLetterCollection)
            {
                if (namesLetter.Letter.Equals(firstLetter))
                {
                    target = namesLetter;
                    break;
                }
            }
        }

        // If not found, use the last one.
        if (target == null)
            target = NamesLetterCollection[NamesLetterCollection.Count - 1];

        return target;
    }
}
