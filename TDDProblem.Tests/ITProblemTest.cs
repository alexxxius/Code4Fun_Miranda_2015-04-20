using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace TDDProblem.Tests
{
    [TestFixture]
    //Integration test Class
    public class ItProblemTest
    {
        [SetUp]
        public void SetUp()
        {
            IList<Tuple<string, string[,]>> tuples = new List<Tuple<string, string[,]>>
                    {
                       new Tuple<string,string[,]>("file1.bin", new string[3, 2] {{"num_connections", "65"}, {"latency_ms", "70"}, {"bandwidth", "20"}}),
                       new Tuple<string,string[,]>("file2.bin", new string[3, 2] {{"num_connections", "40"}, {"latency_ms", "30"}, {"bandwidth", "20"}}),
                       new Tuple<string,string[,]>("file3.bin", new string[3, 2] {{"num_connections", "20"}, {"latency_ms", "20"}, {"bandwidth", "10"}})
                    };

            Console.WriteLine("Creazione file temporanei..");
            foreach (var value in tuples)
            {
                FileStream fs = new FileStream(value.Item1, FileMode.Create);
                BinaryFormatter formatter = new BinaryFormatter();
                try
                {
                    formatter.Serialize(fs, value.Item2);
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
                    fs.Dispose();
                }
            }
        }


        [Test]
        [Category("Integration testing")]
        public void Bin2Tsv_ReturnReportAndFilesTsvCreated()
        {
            //Arrange
            IFilesRepository filesRepository = new FilesRepository();
            Convert convert = new Convert(filesRepository);

            //Act
            Report rpt = convert.BinToTsv(".", ".");

            //Assert
            Assert.AreEqual(50, rpt.TotalBandwith());
            Assert.AreEqual(40, rpt.AvgLatency());
            Assert.AreEqual(3, rpt.TsvFilesCreated);
        }
        
        [TearDown]
        public void TearDown()
        {
            string[] filePaths = Directory.GetFiles(".", "file*");
            foreach (string filePath in filePaths)
            {
                try
                {
                    File.Delete(filePath);
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
