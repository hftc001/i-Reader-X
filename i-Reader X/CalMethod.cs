using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace i_Reader_X
{
    internal class CalMethod
    {
        /// <summary>
        /// 荧光计算
        /// </summary>
        /// <param name="fluodata">荧光数据</param>
        /// <returns></returns>
        public static string CalFluo(List<double> fluodata)
        {
            try
            {
               // Console.WriteLine("进入执行");
                var dataCount = fluodata.Count;
                var point = dataCount / 20;
                var cx = 0;
                var tx = 0;
                double cy = 0;
                double ty = 0;
                var tcspan = dataCount * 90 / 180;
                for (var i = dataCount / 10; i < dataCount * 9 / 10; i++)
                {
                    if (fluodata[i] > cy)
                    {
                        cx = i;
                        cy = fluodata[i];
                    }
                }
                if (cx > dataCount / 2 & cy >= fluodata[cx + 1])
                {
                    tx = cx;
                    ty = cy;
                    cx = 0;
                    cy = 0;
                    for (var i = Math.Max(0, tx - tcspan - 2 * point); i < tx - tcspan + 2 * point; i++)
                    {
                        if (fluodata[i] > cy)
                        {
                            cy = fluodata[i];
                            cx = i;
                        }
                    }
                    if (cx == Math.Max(0, tx - tcspan - 2 * point) | cx == tx - tcspan + 2 * point - 1)
                    {
                        cx = tx - tcspan;
                        cy = fluodata[cx];
                    }
                }
                else if (cx < dataCount / 2 & cy >= fluodata[cx - 1])
                {
                    for (var i = cx + tcspan - 2 * point; i < Math.Min(cx + tcspan + 2 * point, fluodata.Count); i++)
                    {
                        if (fluodata[i] > ty)
                        {
                            ty = fluodata[i];
                            tx = i;
                        }
                    }
                    if (cx == tx - tcspan - 2 * point | cx == Math.Min(cx + tcspan + 2 * point, fluodata.Count) - 1)
                    {
                        tx = cx + tcspan;
                        ty = fluodata[tx];
                    }
                }
                double sumC = 0;
                double sumT = 0;
                for (var i = -point; i < point; i++)
                {
                    sumC += fluodata[cx + i];
                    sumT += fluodata[tx + i];
                }
                double base1 = 0;
                double base2 = 0;
                double base3 = 0;
                double base4 = 0;
                for (var i = cx + point * 2; i < cx + point * 3; i++)
                {
                    base2 += fluodata[i];
                }
                for (var i = tx - point * 3; i < tx - point * 2; i++)
                {
                    base3 += fluodata[i];
                }
                if (cx - point * 2 - Math.Max(0, cx - point * 3) < 1)
                {
                    base1 = base2;
                }
                else
                {
                    for (var i = Math.Max(0, cx - point * 3); i < cx - point * 2; i++)
                    {
                        base1 += fluodata[i];
                    }
                    base1 = base1 / (cx - point * 2 - Math.Max(0, cx - point * 3)) * point;
                }
                if (Math.Min(dataCount - 1, tx + point * 3) - tx - point * 2 < 1)
                {
                    base4 = base3;
                }
                else
                {
                    for (var i = tx + point * 2; i < Math.Min(dataCount - 1, tx + point * 3); i++)
                    {
                        base4 += fluodata[i];
                    }
                    base4 = base4 / (Math.Min(dataCount - 1, tx + point * 3) - tx - point * 2) * point;
                }
                var sumCBase = sumC - base1 - base2;
                var sumTBase = sumT - base3 - base4;
                var ii = 1;
                while (sumCBase < 0 & ii < 11)
                {
                    sumCBase += (Math.Max(base2, base1) - Math.Min(base2, base1)) * ii / 10;
                    ii++;
                }
                ii = 1;
                while (sumTBase < 0 & ii < 11)
                {
                    sumTBase += (Math.Max(base4, base3) - Math.Min(base4, base3)) * ii / 10;
                    ii++;
                }
                var strOd = "C(%1,%2); T(%3,%4); SumCBase(%5); SumTBase(%6)";
                strOd = strOd.Replace("%1", cx.ToString());
                strOd = strOd.Replace("%2", cy.ToString("F2"));
                strOd = strOd.Replace("%3", tx.ToString());
                strOd = strOd.Replace("%4", ty.ToString("F2"));
                strOd = strOd.Replace("%5", sumCBase.ToString("F2"));
                strOd = strOd.Replace("%6", sumTBase.ToString("F2"));
                return strOd;
            }
            catch (Exception)
            {
                return "Error:非正常测试片";
            }
        }
    }
}
