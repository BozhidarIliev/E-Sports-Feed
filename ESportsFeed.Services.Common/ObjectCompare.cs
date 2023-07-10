using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ESportsFeed.Services.Common
{
    public static class ObjectCompare
    {
        public static bool CompareList(dynamic object1, dynamic object2)
        {
            bool match = false;
            List<dynamic> e1 = new List<dynamic>();
            e1.AddRange(object1);
            List<dynamic> e2 = new List<dynamic>();
            e2.AddRange(object2);
            int countFirst, countSecond;
            countFirst = e1.Count;
            countSecond = e2.Count;
            if (countFirst == countSecond)
            {
                countFirst = e1.Count - 1;
                while (countFirst > -1)
                {
                    match = false;
                    countSecond = e2.Count - 1;
                    while (countSecond > -1)
                    {
                        match = Compare(e1[countFirst], e2[countSecond]);
                        if (match)
                        {
                            e2.Remove(e2[countSecond]);
                            countSecond = -1;
                        }
                        if (match == false && countSecond == 0)
                        {
                            return false;
                        }
                        countSecond--;
                    }
                    countFirst--;
                }
            }
            else
            {
                return false;
            }
            return match;
        }
        public static bool Compare(object e1, object e2)
        {
            bool flag = true;
            foreach (PropertyInfo propObj1 in e1.GetType().GetProperties())
            {
                var propObj2 = e2.GetType().GetProperty(propObj1.Name);
                if (propObj1.PropertyType.Name.Equals("List`1"))
                {
                    List<dynamic> objList1 = new List<object>() {
                    propObj1.GetValue(e1, null)
                };
                    List<dynamic> objList2 = new List<object>() {
                    propObj2.GetValue(e2, null)
                };
                    if (!CompareList(objList1[0], objList2[0]))
                    {
                        return false;
                    }
                }
                else if (!(propObj1.GetValue(e1, null).Equals(propObj2.GetValue(e2, null))))
                {
                    flag = false;
                    return flag;
                }
            }
            return flag;
        }
    }
}
