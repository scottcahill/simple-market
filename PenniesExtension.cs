using System;

namespace SmWeb.Utils
{
    public static class PenniesExtension
    {
        public static long? ToPennies(this Decimal ob)
        {
            //return Convert.ToInt32(Math.Ceiling(ob));

            long numBeforeDecimal = (System.Int64)System.Math.Truncate(ob);
            long numAfter = (System.Int64)System.Math.Truncate((ob - numBeforeDecimal) * 100);
            return (numBeforeDecimal * 100) + numAfter;
        }

        public static Decimal? FromPennies(this long ob)
        {
            return (ob / 100);
        }
    }
}
