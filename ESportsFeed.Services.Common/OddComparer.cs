using ESportsFeed.Data.Models;
using System;

namespace ESportsFeed.Services.Common
{
    public class OddComparer : IEqualityComparer<Odd>
    {
        public bool Equals(Odd x, Odd y)
        {
            return x.Value == y.Value;
        }

        public int GetHashCode(Odd x)
        {
            return x.Value.GetHashCode();
        }
    }
}