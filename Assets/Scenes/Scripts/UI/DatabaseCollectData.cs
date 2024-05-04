using Database;
using Microsoft.EntityFrameworkCore;
using Program;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scenes.Scripts.UI
{
    public class DatabaseCollectData
    {
        ExtrusionContext ec = new();

        private static readonly string DesignName = "Designation";
        private static readonly string ConfName = "CONFIGURATION";

        private Dictionary<string, List<Dictionary<string, object>>> datas = new();
        private Dictionary<string, List<Dictionary<string, object>>> database = new();

        private Dictionary<string, object> GetParametrs(IElement element)
        {
            var pars = new Dictionary<string, object>() { { DesignName, element.Name } };
            foreach (var pval in element.GetParametrs())
            {
                pars[pval.Designation] = pval.Value;
            }

            return pars;
        }

        private List<Dictionary<string, object>> GetParametrsList(List<IElement> element)
        {
            var valsConfs = new List<Dictionary<string, object>>();
            foreach (var e in element)
            {
                valsConfs.Add(GetParametrs(e));
            }

            return valsConfs;
        }

        private List<Dictionary<string, object>> GetParametrsList(List<IPossibleConfig> possConf)
        {
            var valsConfs = new List<Dictionary<string, object>>();
            foreach (var e in possConf)
            {
                var config = new MyList<string>();
                config.AddRange(e.Config.GetConfig());

                var pars = GetParametrs(e.Element);
                pars[ConfName] = config;

                valsConfs.Add(pars);
            }

            return valsConfs;
        }

        public DatabaseCollectData()
        {
            //var screwEls = ec.ScrewElements
            //    .Include(sp => sp.ScrewElementParametrValues)
            //    .ThenInclude(spv => spv.IdParametrNavigation).ToList<IElement>();

            //database["ScrewElements"] = GetParametrs(screwEls);

            var screwConfigs = ec.ScrewPossibleСonfigurations
                .Include(s => s.IdScrewNavigation)
                    .ThenInclude(s => s.ScrewParametrValues)
                    .ThenInclude(s => s.IdParametrNavigation)
                .Include(s => s.IdConfigurationNavigation)
                    .ThenInclude(s => s.ScrewElementInСonfigurations)
                    .ThenInclude(s => s.IdElementNavigation)
                    .ThenInclude(sp => sp.ScrewElementParametrValues)
                    .ThenInclude(spv => spv.IdParametrNavigation).ToList<IPossibleConfig>();

            //database["ScrewConfigs"] = GetParametrsList(screwConfigs);       
            
            //foreach (var confs in database["ScrewConfigs"])
            //{
            //    foreach (var pars in confs)
            //    {
            //        Debug.Log(pars.Key + " " + pars.Value);
            //        if (pars.Value is MyList<string>)
            //        {
            //            var p = (MyList<string>)pars.Value;
            //            foreach (var par in p)
            //            {
            //                Debug.Log(par);
            //            }
            //        }
            //    }
            //}

            var initData = new InitData();

            InitDatas(initData.cyl);
            InitDatas(initData.sect);
            InitDatas(initData.S_K.S);
            InitDatas(initData.data);
            InitDatas(initData.dop);
            InitDatas(initData.fluxData);
            InitDatas(initData.train);

            //datas["BodySection"] = datas["CYLINDER[]"];
            //datas["HeadSection"] = datas["SECTIONS[]"];
            //datas["ShnekSection"] = datas["SECT[]"];
            //datas["Pol"] = datas["DATA_"];

            datas["CYLINDER_CONF"] = new List<Dictionary<string, object>>()
            {
                new Dictionary<string, object>
                {
                    { DesignName, "cylConf1" }, { "CYLINDER_CONF", new MyList<string>() { "cyl1", "cyl2" } }
                },
                //new Dictionary<string, object>
                //{
                //    { DesignName, "sec2" }, { "CYLINDER_CONF", new MyList<int>() { 1, 0, 0, 1, 0 } }
                //},
                //new Dictionary<string, object>
                //{
                //    { DesignName, "sec3" }, { "CYLINDER_CONF", new MyList<int>() { 1, 0, 1, 1 } }
                //},
                //new Dictionary<string, object>
                //{
                //    { DesignName, "sec4" }, { "CYLINDER_CONF", new MyList<int>() { 1, 0, 0 } }
                //},
            };

            datas["SECTIONS_CONF"] = new List<Dictionary<string, object>>()
            {
                new Dictionary<string, object>
                {
                    { DesignName, "sectionConf1" }, { "SECTIONS_CONF", new MyList<string>() { "S_K.S1", "S_K.S2", "S_K.S3", "S_K.S4" } },
                },
                //new Dictionary<string, object>
                //{
                //    { DesignName, "head2" }, { "SECTIONS_CONF", new MyList<int>() { 0, 1, 1 } },
                //},
                //new Dictionary<string, object>
                //{
                //    { DesignName, "head2" }, { "SECTIONS_CONF", new MyList<int>() { 1, 0, 0 } },
                //},
            };

            datas["SECT_CONF"] = new List<Dictionary<string, object>>()
            {
                new Dictionary<string, object>
                {
                    { DesignName, "sectConf1" }, { "SECT_CONF", new MyList<string>() { "sect1", "sect2", "sect3" } },
                },
                //new Dictionary<string, object>
                //{
                //    { DesignName, "s2" }, { "SECT_CONF", new MyList<int>() { 0, 1, 1 } },
                //},
                //new Dictionary<string, object>
                //{
                //    { DesignName, "s2" }, { "SECT_CONF", new MyList<int>() { 1, 0, 0 } },
                //},
            };
        }

        public List<Dictionary<string, object>> GetDatas(Dictionary<string, List<string>> needsData)
        {
            int count = int.MaxValue;
            foreach (var needsFields in needsData)
            {
                count = Math.Min(count, datas[needsFields.Key].Count);
            }

            List<Dictionary<string, object>> res = new();
            for (int i = 0; i < count; i++)
            {
                res.Add(new());
            }

            foreach (var needsFields in needsData)
            {
                for (int i = 0; i < count; ++i)
                {
                    foreach (var needsField in needsFields.Value)
                    {
                        res[i][needsField] = datas[needsFields.Key][i][needsField];
                    }
                }
            }

            return res;
        }

        void InitDatas<T>(T[] fields)
        {
            string fieldName = typeof(T[]).ToString()[6..];
            datas[fieldName] = GetFields(fields);
        }
        
        void InitDatas<T>(T fields)
        {
            string fieldName = typeof(T).ToString()[6..];
            datas[fieldName] = new() { GetFields(fields) };
        }

        List<Dictionary<string, object>> GetFields<T>(T[] data)
        {
            var vals = new List<Dictionary<string, object>>();
            foreach (var field in data)
            {
                vals.Add(GetFields(field));
            }

            return vals;
        }

        Dictionary<string, object> GetFields<T>(T data)
        {
            var vals = new Dictionary<string, object>();
            var fields = typeof(T).GetFields();
            foreach (var fild in fields)
            {
                vals[fild.Name] = fild.GetValue(data);
            }

            return vals;
        }
    }
}
