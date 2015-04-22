using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Collections;
using System.Runtime.InteropServices;
using Moq;


namespace TDDProblem.Tests
{
    [TestFixture]
    public class TddProblemTest
    {
        [TestCaseSource(typeof(TddProblemTest.TestCaseFactory), "TestCaseMatrixValues")]
        public void FromMatrixToTsv_AfterFileBinRead_ReturnEqual(string[,] arrayValues)
        {
            //Arrange
            IEnumerable expectedTsvStrings = new List<String>()
            {
                String.Format("{0}\t{1}", "num_connections", 65), 
                String.Format("{0}\t{1}", "latency_ms", 70),
                String.Format("{0}\t{1}", "bandwidth", 20)
            };

            //Act
            IList<String> actualTsvStrings = Convert.FromMatrixToTsv(arrayValues);

            //Assert
            CollectionAssert.AreEqual(expectedTsvStrings, actualTsvStrings);
        }

        [TestCaseSource(typeof(TddProblemTest.TestCaseFactory), "TestCaseListMatrixValues")]
        public void CalculateTotalBandWidth_AfterReadFilesBin_Return50(List<string[,]> listMatrixValues)
        {
            //Arrange
            Report rpt = new Report(listMatrixValues);

            //Act
            int totalBandwith = rpt.TotalBandwith();

            //Assert
            Assert.AreEqual(50, totalBandwith);
        }

        [TestCaseSource(typeof(TddProblemTest.TestCaseFactory), "TestCaseListMatrixValues")]
        public void CalculateAvgLatency_AfterReadFilesBin_Return40(List<string[,]> listMatrixValues)
        {
            //Arrange
            Report rpt = new Report(listMatrixValues);

            //Act
            double avgLatency = rpt.AvgLatency();

            //Assert
            Assert.AreEqual(40, avgLatency);
        }


        [Test]
        public void BinToTsv_WhenNotAllFilesBinAreSaved_Throw()
        {
            //Arrange
            var mockIFilesBinRepository = new Mock<IFilesRepository>();

            mockIFilesBinRepository.Setup(s => s.LoadFilesFromPath(It.IsAny<string>())).Returns(new List<Tuple<string, String[,]>>
            {
               new Tuple<string, string[,]>("filebin1.bin", new string[3, 2] {{"num_connections", "65"}, {"latency_ms", "70"}, {"bandwidth", "20"}}),
               new Tuple<string, string[,]>("filebin1.bin", new string[3, 2] {{"num_connections", "40"}, {"latency_ms", "30"}, {"bandwidth", "20"}}),
               new Tuple<string, string[,]>("filebin1.bin", new string[3, 2] {{"num_connections", "20"}, {"latency_ms", "20"}, {"bandwidth", "10"}})
            });

            mockIFilesBinRepository.Setup(s => s.SaveTsvFiles(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IList<string>>())).Returns(false);
            //Act
            var ex = Assert.Catch(() =>
              {
                  Convert convert = new Convert(mockIFilesBinRepository.Object);
                  convert.BinToTsv(It.IsAny<string>(), It.IsAny<string>());
              });

            //Assert
            StringAssert.Contains("Impossibile salvare i file tsv!", ex.Message);
        }

        [Test]
        public void BinToTsv_AfterFileBinRead_ReturnReport()
        {
            //Arrange
            var mockIFilesBinRepository = new Mock<IFilesRepository>();

            mockIFilesBinRepository.Setup(s => s.LoadFilesFromPath(It.IsAny<string>())).Returns(new List<Tuple<string, String[,]>>
            {
               new Tuple<string, string[,]>("filebin1.bin", new string[3, 2] {{"num_connections", "65"}, {"latency_ms", "70"}, {"bandwidth", "20"}}),
               new Tuple<string, string[,]>("filebin1.bin", new string[3, 2] {{"num_connections", "40"}, {"latency_ms", "30"}, {"bandwidth", "20"}}),
               new Tuple<string, string[,]>("filebin1.bin", new string[3, 2] {{"num_connections", "20"}, {"latency_ms", "20"}, {"bandwidth", "10"}})
            });
            mockIFilesBinRepository.Setup(s => s.SaveTsvFiles(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IList<string>>())).Returns(true);

            //Act
            Convert convert = new Convert(mockIFilesBinRepository.Object);
            Report rpt = convert.BinToTsv(It.IsAny<string>(), It.IsAny<string>());


            //Assert
            Assert.AreEqual(50, rpt.TotalBandwith());
            Assert.AreEqual(40, rpt.AvgLatency());
            Assert.AreEqual(3, rpt.TsvFilesCreated);
        }


        public class TestCaseFactory
        {
            public static IEnumerable TestCaseMatrixValues
            {
                get
                {
                    yield return new TestCaseData(new string[3, 2] { { "num_connections", "65" }, { "latency_ms", "70" }, { "bandwidth", "20" } });
                }
            }

            public static IEnumerable TestCaseListMatrixValues
            {
                get
                {
                    var list = new List<String[,]>
                    {
                        new string[3, 2] {{"num_connections", "65"}, {"latency_ms", "70"}, {"bandwidth", "20"}},
                        new string[3, 2] {{"num_connections", "40"}, {"latency_ms", "30"}, {"bandwidth", "20"}},
                        new string[3, 2] {{"num_connections", "20"}, {"latency_ms", "20"}, {"bandwidth", "10"}}
                    };

                    yield return new TestCaseData(list);
                }
            }

            
        }
    }
}
