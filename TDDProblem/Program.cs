using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace TDDProblem
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Generazione file bin.. (Premi invio per continuare...)");
            Console.ReadLine();

            var listValues = GenerationMatrixValues();

            CreateFileBinForExample(listValues);
            Console.WriteLine("/------------------------------------------------------------------------------\\");
            Console.WriteLine("Generati {0} file bin", listValues.Count);
            Console.WriteLine("");
            Console.WriteLine("Conversione dei file bin->tsv e output report.. (Premi invio per continuare...)");
            Console.ReadLine();

            Convert convert = new Convert(new FilesRepository());

            if (Directory.Exists("TsvRepository"))
                Directory.Delete("TsvRepository", true);
            Report rpt = convert.BinToTsv("BinRepository", "TsvRepository");

            Console.WriteLine("Average latency: {0}", rpt.AvgLatency());
            Console.WriteLine("Total bandwith: {0}", rpt.TotalBandwith());

            Console.WriteLine("Premi invio per chiudere...");
            Console.ReadLine();
        }

        private static IList<string[,]> GenerationMatrixValues()
        {
            IList<string[,]> listValues = new List<string[,]>();

            Random rnd = new Random();
            var iGeneration = rnd.Next(3, 8);

            for (int i = 0; i < iGeneration; i++)
            {
                var value = new string[3, 2]
                {
                    {"num_connections", rnd.Next(1, 100).ToString()},
                    {"latency_ms", rnd.Next(1, 100).ToString()},
                    {"bandwidth", rnd.Next(1, 100).ToString()}
                };
                listValues.Add(value);
            }
            return listValues;
        }

        private static void CreateFileBinForExample(IList<string[,]> listMatrixValues)
        {
            if (Directory.Exists("BinRepository"))
                Directory.Delete("BinRepository", true);
            Directory.CreateDirectory("BinRepository");

            int i = 0;
            foreach (var value in listMatrixValues)
            {
                i++;
                FileStream fs = new FileStream(Path.Combine("BinRepository", "file" + i + ".bin"), FileMode.Create);
                BinaryFormatter formatter = new BinaryFormatter();
                try
                {
                    formatter.Serialize(fs, value);
                    fs.Flush();
                    Console.WriteLine("Nome File: {0}", Path.GetFileName(fs.Name));
                    Console.WriteLine("{0}: {1}, {2}: {3}, {4}: {5}", value[0, 0], value[0, 1], value[1, 0], value[1, 1], value[2, 0], value[2, 1]);
                    Console.WriteLine("");
                }
                catch (SerializationException e)
                {
                    Console.WriteLine("Serializzazione fallita: {0}", e.Message);
                    throw;
                }
                finally
                {
                    fs.Close();
                    fs.Dispose();


                }
            }
        }
    }
}
