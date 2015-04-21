using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TDDProblem
{
    public class FilesBinRepository : IFilesBinRepository
    {
        public IList<string[,]> LoadFilesFromPath(string path)
        {
            throw new NotImplementedException();
        }

        public bool Save(string path, IList<string> tsvStrings)
        {
            throw new NotImplementedException();
        }
    }
}
