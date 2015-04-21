using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TDDProblem
{
    public interface IFilesBinRepository
    {
        IList<string[,]> LoadFilesFromPath(string path);
        bool Save(string path, IList<string> tsvStrings);

    }
}
