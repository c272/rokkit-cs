using System;
using System.Collections.Generic;
using System.Linq;

namespace Rokkit
{
    public class Table
    {
        public int ID;
        public string Name;
        public List<Record> Records;
        private List<ValueTuple<string, Type>> RecordProperties;

        public Table(int ID_, string Name_)
        {
            ID = ID_;
            Name = Name_;
        }

        public void AddProperty(Type t, string name) 
        {
            //Validating name is not already used.
            if (RecordProperties.FindIndex(x => x.Item1==name)!=-1) {
                //Do not add.
                return;
            }

            RecordProperties.Add(new ValueTuple<string, Type>(name, t));
        }
        
    }

    public struct Record
    {
        public int ID;
        public List<object> Properties;
        public List<Type> PropertyTypes;
    }
}

