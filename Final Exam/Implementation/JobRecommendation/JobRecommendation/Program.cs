using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.IO;

namespace JobRecommendation
{
    class Program
    {
        static void Main(string[] args)
        {

            Parser parser = new Parser();
            string file = @"C:\Users\harsh\Desktop\Exam\AOBD\JobRecommendation\JobRecommendation\Data.txt";

            string jsonFile = parser.readFile(file);

            JObject json = JObject.Parse(jsonFile);

            ArrayList skills = new ArrayList();
            ArrayList jobTitle = new ArrayList();
            ArrayList company = new ArrayList();
            ArrayList education = new ArrayList();

            skills = parser.getList("Skills", json);
            jobTitle = parser.getList("Job-Title", json);
            company = parser.getList("Company", json);
            education = parser.getList("Qualification", json);
            /*
            Console.WriteLine("11");
            parser.generateCSV("Skills", @"C:\Users\harsh\Desktop\Exam\AOBD\JobRecommendation\JobRecommendation\Skill1.csv", skills);
            Console.WriteLine("21");
            parser.generateCSV("Job-Title", @"C:\Users\harsh\Desktop\Exam\AOBD\JobRecommendation\JobRecommendation\JobTitle1.csv", jobTitle);
            Console.WriteLine("31");
            parser.generateCSV("Company", @"C:\Users\harsh\Desktop\Exam\AOBD\JobRecommendation\JobRecommendation\Company1.csv", company);
            Console.WriteLine("41");
            parser.generateCSV("Qualification", @"C:\Users\harsh\Desktop\Exam\AOBD\JobRecommendation\JobRecommendation\Education1.csv", education);
            */
            bool[,] userjob = new bool[json["json"].Count(), jobTitle.Count];
            userjob = parser.generateMatrix(json, jobTitle, "Job-Title");
                        
            bool[,] userskill = new bool[json["json"].Count(), skills.Count];
            userskill = parser.generateMatrix(json, skills, "Skills");
            
            bool[,] usercompany = new bool[json["json"].Count(), company.Count];
            usercompany = parser.generateMatrix(json, company, "Company");

            bool[,] userEducation = new bool[json["json"].Count(), education.Count];
            userEducation = parser.generateMatrix(json, education, "Qualification");

            Console.WriteLine("Matrix Creation Complete");
            /*
            Console.WriteLine("Matrix Writing Start");

            parser.matrixToCSV(userjob, @"C:\Users\harsh\Desktop\Exam\AOBD\JobRecommendation\JobRecommendation\UserJobTitleMatrix1.csv");
            parser.matrixToCSV(userskill, @"C:\Users\harsh\Desktop\Exam\AOBD\JobRecommendation\JobRecommendation\UserSkillMatrix1.csv");
            parser.matrixToCSV(usercompany, @"C:\Users\harsh\Desktop\Exam\AOBD\JobRecommendation\JobRecommendation\UserCompanyMatrix1.csv");
            parser.matrixToCSV(userEducation, @"C:\Users\harsh\Desktop\Exam\AOBD\JobRecommendation\JobRecommendation\UserEducationMatrix1.csv");

            Console.WriteLine("Matrix Writing Complete");
            */
            ProfileMatch profile = new ProfileMatch();
            ArrayList l = new ArrayList();

            ArrayList original = new ArrayList();

            int getMatchFor = 90;

            Console.WriteLine("User ID: " + getMatchFor + " (Original User Skills) \n");
            original = profile.getSkillFromID(getMatchFor, userskill, skills);
            for (int i = 0; i < original.Count; i++)
            {
                Console.WriteLine(original[i]);
            }
            Console.WriteLine("\n\n");

            int userID = profile.profileMatch(getMatchFor, userjob);
            Console.WriteLine("User ID: " + userID + " (Job Title Match) \n");
            l = profile.getSkillFromID(userID,userskill,skills);
            for (int i = 0;i<original.Count;i++)
            {
                if (l.Contains(original[i]))
                {
                    l.Remove(original[i]);
                }
            }
            Console.WriteLine("Suggested Skills Based On Job Title Match \n");
            for (int i = 0; i < l.Count; i++)
            {
                Console.WriteLine(l[i]);
            }
            Console.WriteLine("\n\n");

            userID = profile.profileMatch(getMatchFor, usercompany);
            Console.WriteLine("User ID: " + userID + " (Company Match) \n");
            l = profile.getSkillFromID(userID, userskill, skills);
            for (int i = 0; i < original.Count; i++)
            {
                if (l.Contains(original[i]))
                {
                    l.Remove(original[i]);
                }
            }
            Console.WriteLine("Suggested Skills Based On Company Match \n");
            for (int i = 0; i < l.Count; i++)
            {
                Console.WriteLine(l[i]);
            }
            Console.WriteLine("\n\n");

            userID = profile.profileMatch(getMatchFor, userskill);
            Console.WriteLine("User ID: " + userID + " (Skills Match) \n");
            l = profile.getSkillFromID(userID, userskill, skills);
            for (int i = 0; i < original.Count; i++)
            {
                if (l.Contains(original[i]))
                {
                    l.Remove(original[i]);
                }
            }
            
            Console.WriteLine("Suggested Skills Based On Skills Match \n");
            for (int i = 0; i < l.Count; i++)
            {
                Console.WriteLine(l[i]);
            }
            Console.WriteLine("\n\n");

            Console.ReadLine();

        }
    }
}
