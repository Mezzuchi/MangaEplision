using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MangaEplision.Extensions
{
    public static class StringExt
    {
        public static int NthIndexOf(this string str,string st,int nth)
        {
            int n = 0;
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i].ToString() == st)
                {
                    n += 1;
                    if (n == nth)
                        return i;
                }
            }
            return 0;
        }
        public static int NthIndexOf(this string str, char ch, int nth)
        {
            return NthIndexOf(str, ch.ToString(), nth);
        }
    }
}
