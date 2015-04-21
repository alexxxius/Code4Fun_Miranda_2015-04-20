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
    [Category("Integration testing")]
    public class ItProblemTest
    {
        private static string[] fileBins = new string[] { "file1.Bin", "file2.Bin", "file3.Bin" };

        [SetUp]
        public void SetUp()
        {
            IList<string[,]> listValues = new List<String[,]>
                    {
                        new string[3, 2] {{"num_connections", "65"}, {"latency_ms", "70"}, {"bandwidth", "20"}},
                        new string[3, 2] {{"num_connections", "40"}, {"latency_ms", "30"}, {"bandwidth", "20"}},
                        new string[3, 2] {{"num_connections", "20"}, {"latency_ms", "20"}, {"bandwidth", "10"}}
                    };

            for (int i = 0; i < 3; i++)
            {
                //IFilesBinRepository filesBinRepository = new FilesBinRepository();

                FileStream fs = new FileStream(fileBins[0], FileMode.Create);


                BinaryFormatter formatter = new BinaryFormatter();
                try
                {
                    formatter.Serialize(fs, listValues.ElementAt(i));
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
        public void Bin2Tsv_returnReport()
        {
            
        }


        [TearDown]
        public void TearDown()
        {
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    File.Delete(fileBins[i]);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Impossibile cancellare i file: {0}", ex.Message);
                }

            }
        }
    }
}
