using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Database
{
    public interface IPossibleConfig
    {
        IElement Element { get; set; }
        IConfig Config { get; set; }

        string Name
        {
            get
            {
                return Element.Name;
            }
            set
            {
                Element.Name = value;
            }
        }

        List<IParametr> GetParametrs()
        {
            return Element.Parametrs;
        }

        List<string> GetConfig()
        {
            return Config.GetConfig();
        }
    }

    public interface IConfig : IValue
    {
        public string Name { get; set; }
        List<IConfigElement> ConfigElements { get; set; }
        List<string> GetConfig()
        {
            var conf = new List<string>();            
            foreach (var el in ConfigElements)
            {
                conf.Add(el.Name);
            }
            return conf;
        }

        string IValue.ValName
        {
            get
            {
                return Name;
            }
            set
            {
                Name = value;
            }
        }
        object IValue.Val
        {
            get
            {
                return GetConfig();
            }
            set
            {
                Debug.Log("Не реализовано");
            }
        }
    }

    public interface IConfigElement
    {
        long Id { get; set; }
        //long IdConfiguration { get; set; }
        long IdElement { get; set; }
        long Number { get; set; }
        string Name { get; set; }

        void CopyTo(IConfigElement configElement)
        {
            //configElement.IdConfiguration   = IdConfiguration;
            configElement.IdElement         = IdElement;
            configElement.Number            = Number;
        }
    }

    public class ConfigElementInstance : IConfigElement
    {
        public long Id { get; set; }
        //public long IdConfiguration { get; set; }
        public long IdElement { get; set; }
        public long Number { get; set; }
        public string Name { get; set; }
    }

    public interface IElement : IValue
    {
        long Id { get; set; }
        string Name { get; set; }
        List<IParametr> Parametrs { get; set; }

        string IValue.ValName
        {
            get
            {
                return "Designation";
            }
            set
            {
                Debug.Log("IElement.IValue.set Не реализовано");
            }
        }
        object IValue.Val
        {
            get
            {
                return Name;
            }
            set
            {
                Name = value.ToString();
            }
        }
    }

    public interface IParametr : IValue
    {
        long IdParametr { get; set; }
        string Designation { get; set; }
        double Value { get; set; }

        void CopyTo(IParametr par)
        {
            par.IdParametr = IdParametr;
            par.Value = Value;
        }

        string IValue.ValName
        {
            get
            {
                return Designation;
            }
            set
            {
                Designation = value;
            }
        }
        object IValue.Val
        {
            get
            {
                return Value;
            }
            set
            {
                Value = (double)value;
            }
        }
    }

    public class ParametrInstance : IParametr
    {
        public long IdParametr { get; set; }
        public string Designation { get; set; }
        public double Value { get; set; }
    }

    public interface IPar
    {
        long Id { get; set; }
        string Designation { get; set; }
    }

    public interface IValue
    {
        string ValName { get; set; }
        object Val { get; set; }
    }

    public class DesignVal : IValue
    {
        public string ValName { get; set; }
        public object Val { get; set; }
    }
}