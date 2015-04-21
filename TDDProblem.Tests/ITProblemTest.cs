using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using TDDProblem;

namespace TDDProblem.Tests
{
    [TestFixture]
    public class ItProblemTest
    {
        private static readonly string[] FileBins = { "file1.bin", "file2.bin", "file3.bin" };

        [SetUp]
        public void SetUp()
        {
            IList<string[,]> listValues = new List<String[,]>
                    {
                        new string[3, 2] {{"num_connections", "65"}, {"latency_ms", "70"}, {"bandwidth", "20"}},
                        new string[3, 2] {{"num_connections", "40"}, {"latency_ms", "30"}, {"bandwidth", "20"}},
                        new string[3, 2] {{"num_connections", "20"}, {"latency_ms", "20"}, {"bandwidth", "10"}}
                    };

            Console.WriteLine("Creazione file temporanei..");
            for (int i = 0; i < 3; i++)
            {
                FileStream fs = new FileStream(FileBins[i], FileMode.Create);
                BinaryFormatter formatter = new BinaryFormatter();
                try
                {
                    formatter.Serialize(fs, listValues.ElementAt(i));
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


        [Test]
        [Category("Integration testing")]
        public void Bin2Tsv_ReturnReportAndFilesTsvCreated()
        {
            IFilesRepository filesRepository = new FilesRepository();
            Convert convert = new Convert(filesRepository);

            Report rpt = convert.BinToTsv(".", ".");

            //Assert
            Assert.AreEqual(50, rpt.TotalBandwith());
            Assert.AreEqual(40, rpt.AvgLatency());
            Assert.AreEqual(3, rpt.TsvFilesCreated);
        }


        [TearDown]
        public void TearDown()
        {
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    File.Delete(FileBins[i]);
                    Console.WriteLine("Cancellazione file temporanei..");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Impossibile cancellare i file: {0}", ex.Message);
                }

            }
        }
    }
}
