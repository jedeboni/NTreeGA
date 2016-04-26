using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace NTreeGA
{
    public class Ponto

    {
      public double x, y; // UTM Coordinates (projected coordinates) 
      public double peso; // weigth of the Point
      public int cluster; // cluster that the point belongs
 
    
      public Ponto(double a, double b)
      {
          x = a;
          y = b;
          peso = 1.0;
          cluster = 0;
       
        }

        public Ponto(double a, double b, double p, int c)
        {
            x = a;
            y = b;
            peso = p;
            cluster = c;
        }

        public Ponto(double a, double b, double p)
        {
            x = a;
            y = b;
            peso = p;
            cluster = 0;
        }

	    public double Distance(Ponto a, Ponto b)
	    {
            double d = Math.Sqrt((b.y-a.y)*(b.y-a.y)+(b.x-a.x)*(b.x-a.x));
            return d;
	    }

        public double Distance(Ponto b)
        {
            double d = Math.Sqrt((b.y - this.y) * (b.y - this.y) + (b.x -this.x) * (b.x - this.x));
            return d;
        }

        public double PDistance(Ponto a, Ponto b)
        {
            return peso * Distance(a, b);
        }

        public double PDistancia(Ponto b)
        {
            return peso * Distance(b);
        }

        public Ponto[] Copy(int _N, Ponto[] _fonte)
        {
            Ponto[] _data = new Ponto[Preferences.NMAX];
            for (int i = 0; i <= _N; i++)
            {
                _data[i] = new Ponto(_fonte[i].x, _fonte[i].y, _fonte[i].peso, _fonte[i].cluster);
            }
                return _data;
        }

        public Ponto[] ReadCSVFile(String _fileName, ref int _N, ref System.Windows.Forms.ProgressBar pb)
        {
            // Leitura dos dados dos pontos de um arquivo CSV. 
            // Estrutura do CSV
            // (separador ";")
            // Header na primeira linha
            // UTMX, UTMY, Peso, #Cluster
            // ... N linhas
            //
            Ponto[] _data = new Ponto[Preferences.NMAX];
            try
            {
                var reader = new StreamReader(File.OpenRead(_fileName));

                _N = 0;
                var line = reader.ReadLine();
                String[] header = line.Split(Preferences._delimiter);
                var pbCounter = 0;
                pb.Value = 0;
                // 

                while (!reader.EndOfStream)
                {
                    //                    if ((N % 50) == 0) { LogWriter.Add2Log("."); };  // a dot every 50 points read.
                    line = reader.ReadLine();
                    var values = line.Split(Preferences._delimiter);
                    _N = _N + 1;
                    _data[_N] = new Ponto(Double.Parse(values[0]),
                                     Double.Parse(values[1]),
                                     Double.Parse(values[2]),
                                     Int16.Parse(values[3]));
                    //
                    if (!(pb == null))
                    {
                        if ((_N % 100) == 0)
                        { // progress bar every 100 pts until 10000
                            pbCounter++;
                            if (pbCounter <= 100)
                            {
                                pb.Value = pbCounter;
                            }
                        };
                    }

                    if (_N == 1)
                    {
                        Ambiente._XMax = _data[1].x;
                        Ambiente._YMax = _data[1].y;
                        Ambiente._XMin = _data[1].x;
                        Ambiente._YMin = _data[1].y;
                    }
                    else
                    {
                        if (_data[_N].x > Ambiente._XMax) { Ambiente._XMax = _data[_N].x; };
                        if (_data[_N].x < Ambiente._XMin) { Ambiente._XMin = _data[_N].x; };
                        if (_data[_N].y > Ambiente._YMax) { Ambiente._YMax = _data[_N].y; };
                        if (_data[_N].y < Ambiente._YMin) { Ambiente._YMin = _data[_N].y; };
                    }
                }
            }
            catch (IOException e)
            {
                MessageBox.Show("Error while reading file\n"+_fileName+"\n"+e.ToString(),
                                "ERRO ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (!(pb == null)) { pb.Value = 100;  };
            return _data;
        }



        public void WriteCSVFile(String _fileName, Ponto[] _pts, int _N, ref System.Windows.Forms.ProgressBar pb)
        {
            try
            {
                // Criar arquivo de saida com Pontos e Clusters

                if (!File.Exists(_fileName))
                {
                    File.Create(_fileName).Close();
                }
                else
                {
                    File.Delete(_fileName);
                    File.Create(_fileName).Close();

                }
                StringBuilder sb = new StringBuilder();
                // Write the header
                String FirstLine = "X_COORD" + Preferences._delimiter +
                                   "Y_COORD" + Preferences._delimiter +
                                   "WEIGHT"  + Preferences._delimiter  + 
                                   "CLUSTER";
                Console.WriteLine(FirstLine);
                sb.AppendLine(FirstLine);
                int pbCounter = 0;
                pb.Value = 0;
                // Write the data Points
                for (int i = 1; i <= _N; i++)
                {
                    String CSVLine =  _pts[i].x.ToString() + Preferences._delimiter
                                    + _pts[i].y.ToString() + Preferences._delimiter
                                    + _pts[i].peso + Preferences._delimiter
                                    + _pts[i].cluster;
                    sb.AppendLine(CSVLine);
                     // progress bar every 100 pts until 10000
                    if ((_N % 100) == 0) { // progress bar every 100 pts until 10000
                        pbCounter++;
                        if (pbCounter <= 100)
                        {
                            pb.Value = pbCounter;
                        }
                    };  

                }
                File.AppendAllText(_fileName, sb.ToString());
                File.Exists(_fileName);
                pb.Value = 100;

            }

            catch (IOException ex)
            {
                MessageBox.Show("Error while writing file\n" + _fileName + "\n" + ex.ToString(),
                                "ERRO ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        public Ponto[] GenerateRandomClusters(Ponto[] _pts, int _N, int _K, int NRandom)
        {
            //
            //   Selects a random set of Clusters getins 3 random points in the 
            //   point set.
            //
            Random rand = new Random();
            Ponto[] _data = new Ponto[Preferences.NMAX];
            for (int j = 1; j <= _K; j++)
            {
                double somaX = 0.0;
                double somaY = 0.0;
                for (int k = 1; k <= NRandom; k++)
                    {
                        int rP = 1 + (Int32)((_N - 1) * rand.NextDouble());
                        somaX = somaX + _pts[rP].x;
                        somaY = somaY + _pts[rP].y;
                    }
                    double NRanFloat = (double) NRandom;
                    _data[j] = new Ponto(somaX / NRanFloat, somaY / NRanFloat);
                }

              return _data;
            }

        public void WriteGeoJSonFile()
        {
            // TO BE DEVELOPED IN THE FUTURE, I THINK
        }
    }
}
