using Program;
using System;
using System.Collections.Generic;
using System.Linq;
using Types;

namespace Assets.Scenes.Scripts.UI
{

    public class DatabaseCollectData
    {
        private Dictionary<string, List<Dictionary<string, object>>> datas = new();

        public DatabaseCollectData()
        {
            var initData = new InitData();
            var initFields = typeof(InitData).GetFields();
            foreach (var initF in initFields)
            {
                string fieldName = initF.FieldType.Name;
                datas[fieldName] = new();
                if (initF.FieldType.IsArray)
                {
                    var propValArray = (Array)initF.GetValue(initData);
                    foreach (var propVal in propValArray)
                    {
                        var vals = new Dictionary<string, object>();
                        var filds = propVal.GetType().GetFields();
                        foreach (var fild in filds)
                        {
                            vals[fild.Name] = fild.GetValue(propVal);
                        }

                        datas[fieldName].Add(vals);
                    }
                }
                else
                {
                    var fieldVal = initF.GetValue(initData);
                    var vals = new Dictionary<string, object>();
                    var filds = fieldVal.GetType().GetFields();
                    foreach (var fild in filds)
                    {
                        vals[fild.Name] = fild.GetValue(fieldVal);
                    }
                    datas[fieldName].Add(vals);
                }
            }

            datas["BodySection"] = datas["CYLINDER[]"];
            datas["HeadSection"] = datas["SECTIONS[]"];
            datas["ShnekSection"] = datas["SECT[]"];
            datas["Pol"] = datas["DATA_"];

            datas["Body"] = new List<Dictionary<string, object>>()
            {
                new Dictionary<string, object>
                {
                    { "Designation", "sec1" }, { "CYLINDER_CONF", new MyList<int>() { 0, 1 } }, 
                    { "Lam_k", datas["DATA_"].First()["Lam_k"] }
                },
                new Dictionary<string, object>
                {
                    { "Designation", "sec2" }, { "CYLINDER_CONF", new MyList<int>() { 1, 0, 0, 1, 0 } },
                    { "Lam_k", datas["DATA_"].First()["Lam_k"] }
                },
                new Dictionary<string, object>
                {
                    { "Designation", "sec3" }, { "CYLINDER_CONF", new MyList<int>() { 1, 0, 1, 1 } },
                    { "Lam_k", datas["DATA_"].First()["Lam_k"] }
                },
                new Dictionary<string, object>
                {
                    { "Designation", "sec4" }, { "CYLINDER_CONF", new MyList<int>() { 1, 0, 0 } },
                    { "Lam_k", datas["DATA_"].First()["Lam_k"] }
                },
            };

            datas["Head"] = new List<Dictionary<string, object>>()
            {
                new Dictionary<string, object>
                {
                    { "Designation", "head1" }, { "SECTIONS_CONF", new MyList<int>() { 0, 1 } },
                    { "Lam_k", datas["DATA_"].First()["Lam_k"] }
                },
            };
        }

        public List<Dictionary<string, object>> GetDatas(string dataNames)
        {
            return datas[dataNames];
        }

        /*
                l[0]["Types.CYLINDER"] = new MyList<int>() { 0, 1, 0, 1 };
                l[1]["Types.CYLINDER"] = new MyList<int>() { 1, 0, 0, 0 };
                l[2]["Types.CYLINDER"] = new MyList<int>() { 1, 1, 1, 1 };
                l[3]["Types.CYLINDER"] = new MyList<int>() { 1, 0, 2, 1 };
                l[3]["Designation"] = "sec4";
         */
    }
}
