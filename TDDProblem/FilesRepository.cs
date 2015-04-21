using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace TDDProblem
{
    public class FilesRepository : IFilesRepository
    {

        public IList<Tuple<string, string[,]>> LoadFilesFromPath(string path)
        {
            IList<Tuple<string, string[,]>> listValues = new List<Tuple<string, string[,]>>();

            foreach (var filePath in Directory.GetFiles(path, "*.bin"))
            {
                FileStream fs = new FileStream(filePath, FileMode.Open);
                try
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    Tuple<String, string[,]> tupla = new Tuple<string, string[,]>(Path.GetFileNameWithoutExtension(filePath), (string[,])formatter.Deserialize(fs));
                    listValues.Add(tupla);
                }
                catch (SerializationException e)
                {
                    Console.WriteLine("Deserializzazione fallita: " + e.Message);
                    throw;
                }
                finally
                {
                    fs.Close();
                }
            }


            return listValues;
        }

        public bool SaveTsvFiles(string path, string fileName, IList<string> tsvStrings)
        {
            using (StreamWriter file = new StreamWriter(Path.Combine(path, fileName + ".tsv")))
            {
                try
                {
                    foreach (string tsvString in tsvStrings)
                    {
                        file.WriteLine(tsvString);
                    }
                }
                catch (Exception)
                {
                    return false;
                }
            }

            return true;
        }

        public static void CreateFileBinForTesting(IList<string[,]> listMatrixValues)
        {
            int i = 1;
            foreach (var value in listMatrixValues)
            {
                i++;
                FileStream fs = new FileStream("file" + i + ".bin", FileMode.Create);
                BinaryFormatter formatter = new BinaryFormatter();
                try
                {
                    formatter.Serialize(fs, value);
                    fs.Flush();
                }
                catch (SerializationException e)
                {
                    Console.WriteLine("Serializzazione fallita: {0}", e.Message);
                    throw;
                }
                finally
                {
                    fs.Close();
                }
            }
        }
    }
}
