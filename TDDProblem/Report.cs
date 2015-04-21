using System.Collections.Generic;
using System.Linq;

namespace TDDProblem
{
    public class Report
    {
        private readonly IList<string[,]> _listArrayValues;

        public Report(IList<string[,]> listArrayValues)
        {
            _listArrayValues = listArrayValues;
        }

        public int TotalBandwith()
        {
            return _listArrayValues.Sum(arrayValues => int.Parse(arrayValues[2, 1]));
        }

        public double AvgLatency()
        {
            return _listArrayValues.Average(arrayValues => int.Parse(arrayValues[1, 1]));
        }

        public int TsvFilesCreated
        {
            get { return _listArrayValues.Count(); }
        }
    }
}