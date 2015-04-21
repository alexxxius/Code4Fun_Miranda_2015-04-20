using System;
using System.Collections.Generic;

namespace TDDProblem
{
    public class Convert
    {
        private readonly IFilesBinRepository _filesBinRepository;

        public Convert(IFilesBinRepository filesBinRepository)
        {
            _filesBinRepository = filesBinRepository;
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
            IList<string[,]> values = _filesBinRepository.LoadFilesFromPath(pathBinFiles);

            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var value in values)
            {
                IList<string> tsvStrings = Convert.FromMatrixToTsv(value);
                if (!_filesBinRepository.Save(pathTsvFiles, tsvStrings))
                {
                    throw new Exception("Impossibile salvare i file tsv!");
                }
            }

            return new Report(values);
        }
    }
}