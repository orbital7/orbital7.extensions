using System;
using System.Collections.Generic;

namespace Orbital7.Extensions
{
    public class LetteredNames
    {
        public class NamesLetter
        {
            public string Letter { get; set; }
            public List<object> Names { get; set; }

            public bool HasNames
            {
                get { return this.Names.Count > 0; }
            }

            public NamesLetter()
            {
                this.Names = new List<object>();
            }

            public NamesLetter(string letter)
                : this()
            {
                this.Letter = letter;
            }

            public override string ToString()
            {
                return this.Letter + " (" + this.Names.Count + ")";
            }
        }

        public List<NamesLetter> NamesLetterCollection { get; set; }

        public LetteredNames()
        {
            // Create.
            this.NamesLetterCollection = new List<NamesLetter>();
            this.NamesLetterCollection.Add(new NamesLetter("A"));
            this.NamesLetterCollection.Add(new NamesLetter("B"));
            this.NamesLetterCollection.Add(new NamesLetter("C"));
            this.NamesLetterCollection.Add(new NamesLetter("D"));
            this.NamesLetterCollection.Add(new NamesLetter("E"));
            this.NamesLetterCollection.Add(new NamesLetter("F"));
            this.NamesLetterCollection.Add(new NamesLetter("G"));
            this.NamesLetterCollection.Add(new NamesLetter("H"));
            this.NamesLetterCollection.Add(new NamesLetter("I"));
            this.NamesLetterCollection.Add(new NamesLetter("J"));
            this.NamesLetterCollection.Add(new NamesLetter("K"));
            this.NamesLetterCollection.Add(new NamesLetter("L"));
            this.NamesLetterCollection.Add(new NamesLetter("M"));
            this.NamesLetterCollection.Add(new NamesLetter("N"));
            this.NamesLetterCollection.Add(new NamesLetter("O"));
            this.NamesLetterCollection.Add(new NamesLetter("P"));
            this.NamesLetterCollection.Add(new NamesLetter("Q"));
            this.NamesLetterCollection.Add(new NamesLetter("R"));
            this.NamesLetterCollection.Add(new NamesLetter("S"));
            this.NamesLetterCollection.Add(new NamesLetter("T"));
            this.NamesLetterCollection.Add(new NamesLetter("U"));
            this.NamesLetterCollection.Add(new NamesLetter("V"));
            this.NamesLetterCollection.Add(new NamesLetter("W"));
            this.NamesLetterCollection.Add(new NamesLetter("X"));
            this.NamesLetterCollection.Add(new NamesLetter("Y"));
            this.NamesLetterCollection.Add(new NamesLetter("Z"));
            this.NamesLetterCollection.Add(new NamesLetter("#"));
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
                foreach (NamesLetter namesLetter in this.NamesLetterCollection)
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
                target = this.NamesLetterCollection[this.NamesLetterCollection.Count - 1];

            return target;
        }
    }
}
