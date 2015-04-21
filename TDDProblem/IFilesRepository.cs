using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TDDProblem
{
    public interface IFilesRepository
    {
        IList<Tuple<string, string[,]>> LoadFilesFromPath(string path);
        bool SaveTsvFiles(string path,string fileNAme, IList<string> tsvStrings);

    }
}
