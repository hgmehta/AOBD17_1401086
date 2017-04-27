using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json.Linq;

namespace JobRecommendation
{
    class Parser
    {
        public string readFile(string filepath)
        {
            string file = "";
            using (StreamReader r = new StreamReader(filepath))
            {
                file = r.ReadToEnd();
            }

            return file;
        }

        public string reFormatString(string data)
        {
            string[] stringToReplace = { "&&", ",", "|", ";", "\n", "\u2022", "\uf0d8", "\u25a1", "\u002a", "\u27a2" };
            for (int i = 0; i < stringToReplace.Length; i++)
            {
                data = data.Replace(stringToReplace[i], ",");
            }
            return data;
        }

        public string getData(JObject json,int index,string key)
        {
            string data = (string)json["json"][index][key];
            return data;
        }

        public ArrayList getList(string key, JObject json)
        {
            ArrayList values = new ArrayList();
            for (int i = 0; i < json["json"].Count(); i++)
            {
                string skill = getData(json,i,key);//(string)json["json"][i][key];
                string[] stringToSplit = {","};
                string[] stringToSplit1 = { "&&" };
                if (key != "Qualification")
                {
                    skill = reFormatString(skill);
                }
                
                char[] charsToTrim = { '\n', ' ', '\u2022', '\u25cf', '-', ':' };

                string[] split = skill.Split(key != "Qualification" ? new[] { ","} :new[] { "&&"}, StringSplitOptions.None);

                for (int j = 0; j < split.Length; j++)
                {
                    string s = split[j].Trim(charsToTrim);
                    int x = 0;
                    DateTime y;
                    if (s != "" && !Int32.TryParse(s,out x) && !DateTime.TryParse(s,out y))
                    {
                        if (!values.Contains(s))
                        {
                            values.Add(s);
                        }
                    }
                }
            }
            return values;
        }

        public void generateCSV(string key,string filepath,ArrayList values)
        {
            string data = key;
            for (int i = 0; i < values.Count; i++)
            {
                data += "\n" + values[i];
            }

            File.WriteAllText(filepath, data);
        }

        public bool[,] generateMatrix(JObject json,ArrayList list,string key)
        {
            int totalUser = json["json"].Count();
            bool[,] matrix = new bool[totalUser, list.Count];

            for (int i = 0; i < totalUser; i++)
            {
                string data = getData(json,i,key);
                for (int j = 0; j < list.Count; j++)
                {
                    if (data.Contains(list[j].ToString()))
                    {
                        matrix[i,j] = true;
                    }
                    else
                    {
                        matrix[i, j] = false;
                    }
                }
            }

            return matrix; 
        }

        public void matrixToCSV(bool[,] matix, string filepath)
        {
            System.IO.StreamWriter f = new System.IO.StreamWriter(filepath);
            for (int i = 0; i < matix.GetLength(0); i++)
            {
                string data = "";
                for (int j = 0; j < matix.GetLength(1); j++)
                {
                    string s =  matix[i, j] ? "1" : "0";
                    data += s + ",";
                }
                f.WriteLine(data);
            }
            f.Close();
            //File.WriteAllText(filepath, data);
        }
    }
}
