using System.Collections.Generic;

namespace Database
{
    public  interface IPossibleConfig
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
            return Element.GetParametrs();
        }

        List<string> GetConfig()
        {
            return Config.GetConfig();
        }
    }

    public interface IConfig
    {
        List<IConfigElement> GetConfigElements();
        List<string> GetConfig()
        {
            var conf = new List<string>();            
            foreach (var el in GetConfigElements())
            {
                conf.Add(el.Name);
            }
            return conf;
        }
    }

    public interface IConfigElement
    {
        string Name { get; set; }
    }

    public interface IElement
    {
        string Name { get; set; }
        List<IParametr> GetParametrs();
    }

    public interface IParametr
    {
        string Designation { get; set; }
        double Value { get; set; }
    }
}