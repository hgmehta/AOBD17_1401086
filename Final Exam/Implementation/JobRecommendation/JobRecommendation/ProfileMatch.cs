using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobRecommendation
{
    class ProfileMatch
    {
        int totalCout = 0;
        public int profileMatch(int userID,bool[,] matrix)
        {
            int matchCount = 0;
            int user = -1;
            int max = matchCount;
            ArrayList usersdata = new ArrayList();

            usersdata = getUsersData(userID, matrix);

            for (int k = 0; k < usersdata.Count; k++)
            {
                ArrayList userList = new ArrayList();
                ArrayList data = new ArrayList();
                userList = getUser((int)usersdata[k], matrix);
                if (userList.Contains(userID))
                {
                    userList.Remove(userID);
                }
                for (int i = 0; i < userList.Count; i++)
                {
                    matchCount = 0;
                    data = getUsersData(i, matrix);
                    for (int j = 0; j < data.Count; j++)
                    {
                        if (usersdata.Contains(data[j]))
                        {
                            matchCount++;
                        }
                    }
                    if (matchCount >= max)
                    {
                        max = matchCount;
                        user = (int)userList[i];
                    }
                }
            }
            return user;
        }

        public ArrayList getUsersData(int userID, bool[,] matrix)
        {
            ArrayList l = new ArrayList();
            for (int i = 0; i < matrix.GetLength(1); i++)
            {
                if (matrix[userID, i])
                {
                    l.Add(i);
                }
            }
            return l;
        }

        public ArrayList getUser(int ID,bool[,] matrix)
        {
            ArrayList l = new ArrayList();
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                if (matrix[i, ID])
                {
                    l.Add(i);
                }
            }
            return l;
        }

        public ArrayList getSkillFromID(int userID,bool[,] matrix, ArrayList skillList)
        {
            ArrayList l = new ArrayList();
            for (int i = 0; i < matrix.GetLength(1); i++)
            {
                if (matrix[userID,i])
                {
                    l.Add(skillList[i]);
                }
            }
            return l;
        }
    }
}
