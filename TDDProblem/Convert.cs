using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Markup;

namespace TDDProblem
{
    public class Convert
    {
        private readonly IFilesRepository _filesRepository;

        public Convert(IFilesRepository filesRepository)
        {
            _filesRepository = filesRepository;
        }

        public static IList<string> FromMatrixToTsv(string[,] arrayValues)
        {
            IList<string> tsvValues = new List<string>();
            for (int i = 0; i < 3; i++)
            {
                tsvValues.Add(string.Format("{0}\t{1}", arrayValues[i, 0], arrayValues[i, 1]));
            }

            return tsvValues;
        }

        public Report BinToTsv(String pathBinFiles, String pathTsvFiles)
        {
            IList<Tuple<string, string[,]>> tuples = _filesRepository.LoadFilesFromPath(pathBinFiles);
            
            // ReSharper disable once LoopCanBeConvertedToQuery
            IList<string[,]> values = new List<string[,]>();
            foreach (var tupla in tuples)
            {
                IList<string> tsvStrings = Convert.FromMatrixToTsv(tupla.Item2);
                if (!_filesRepository.SaveTsvFiles(pathTsvFiles, tupla.Item1, tsvStrings))
                {
                    throw new Exception("Impossibile salvare i file tsv!");
                }
                values.Add(tupla.Item2);
            }

            return new Report(values);
        }
    }
}