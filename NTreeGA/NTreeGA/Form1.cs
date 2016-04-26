using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace NTreeGA
{
    public partial class NTreeGA : Form
    {

        //
        private Individuo[] Populacao = new Individuo[Preferences.NMAX];
        //
        private int N; // Number of points
        //
        //private Individuo singleOne = new Individuo();
        private Individuo theBest = new Individuo();
      
        public NTreeGA()
        {
            InitializeComponent();
        }


        public void EntradaListaCSV(String file)
        {
            // Leitura dos dados dos pontos iniciais dos Clusters de um arquivo CSV. 
            // Estrutura do CSV
            // (separador ";")
            // Header na primeira linha
            // UTMX, UTMY, Capacidade, #Cluster, Fixo
            // ... K linhas
            //
            try
            {
                var reader = new StreamReader(File.OpenRead(file));
                N = 0;
                var line = reader.ReadLine();
                var headerCluster = line.Split(Preferences._delimiter); //  not used
                //
                New2Log("Reading File " + file + "\n");
                //
                while (!reader.EndOfStream)
                {
                    //                   if ((N % 50) == 0) { Add2Log("."); };  // a dot every 50 points
                    line = reader.ReadLine();
                    var values = line.Split(Preferences._delimiter);
                    N = N + 1;
                    //                   Descricao         PESO	             VALOR
                    //Litens[N] = new Item(values[0], double.Parse(values[1]), double.Parse(values[2]));
                }
            }
            catch (IOException e)
            {
                MessageBox.Show("Error while reading file\n" + file + "\n" + e.ToString(),
                                "ERRO ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        public String NewRTFLog(String _logMessage, String _rtfLog)
        {
            return (_rtfLog + "\n" + _logMessage);
        }


        public void New2Log(String texto)
        {
            rtfLOG.Text = rtfLOG.Text + "\n" + texto;
            rtfLOG.Invalidate();
            rtfLOG.Update();
            rtfLOG.Refresh();
        }


        public void ClearLog(String texto)
        {
            rtfLOG.Text = texto;
            rtfLOG.Invalidate();
            rtfLOG.Update();
            rtfLOG.Refresh();
        }

        private void readToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // File > Read Data

            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            // Set filter options and filter index.
            //  openFileDialog1.InitialDirectory = DirectoryTxt.Text;
            openFileDialog1.Filter = "CSV Files (.csv)|*.csv|All Files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.Multiselect = true;

            // Call the ShowDialog method to show the dialog box.
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK) // Test result.
            {
                //  Get FileName
                txtDataFile.Text = openFileDialog1.FileName;
                String fileName = openFileDialog1.FileName;
                //  Read Poins 
                Ponto AuxPonto = new Ponto(0, 0);
                Ponto[] Lista;
                int N1 = 0;
                //
                // Define os destinos e o numero de pontos N
                Lista = AuxPonto.ReadCSVFile(fileName, ref N1, ref progressBar);
                Ambiente.Origem = new Ponto(Lista[1].x, Lista[1].y);
                for (int i = 2; i <= N1; i++)
                {
                    Ambiente.Destino[i - 1] = new Ponto(Lista[i].x, Lista[i].y);
                }
                Ambiente.N = N1 - 1;
                theBest.Escala(Ambiente._XMin, Ambiente._XMax, Ambiente._YMin, Ambiente._YMax);
                //
                rtfLOG.Text = NewRTFLog("Open " + fileName, rtfLOG.Text);
                rtfLOG.Text = NewRTFLog("  N = " + Ambiente.N.ToString("0000"), rtfLOG.Text);
                rtfLOG.Text = NewRTFLog("  EscalaX = " + (Ambiente.EscalaX).ToString("0000000.00"), rtfLOG.Text);
                rtfLOG.Text = NewRTFLog("  EscalaY = " + (Ambiente.EscalaY).ToString("0000000.00"), rtfLOG.Text);
                //
            }

        }

        private void setRandomIndividuoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            theBest = theBest.geraAleatorio();
            rtfLOG.Text = NewRTFLog("  Gerado Indiviuo Aleatório =  " , rtfLOG.Text);
            rtfLOG.Text = NewRTFLog("         Fitness  =  " + theBest.getFitness().ToString("0000000.00"), rtfLOG.Text);
        }

        private void drawInidviduoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GraphForm gf = new GraphForm();
            gf.setDrawDist(true);
            gf.setDimensions();
            gf.setIndividuo(theBest, Ambiente.N);
            gf.setDescricao(" Teste de um Individuo Aleatório");
            gf.Show();
  

        }

        private void setRandomPopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // SetUp > Random Population
            GASolver ga = new GASolver();
            String linha =  ga.GAInitPopulation(ref Populacao);
            this.theBest = Populacao[1];
            rtfLOG.Text = NewRTFLog("  Gerando Populacao Aleatório =  ", rtfLOG.Text);
            rtfLOG.Text = NewRTFLog("         Best Fit  =  " + theBest.getFitness().ToString("0000000.00"), rtfLOG.Text);
            rtfLOG.Text = NewRTFLog("      " + linha, rtfLOG.Text);

        }

        private void drawBestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Draw > Draw TheBest
            GraphForm gf = new GraphForm();
            gf.setDrawDist(true);
            gf.setDimensions();
            gf.setIndividuo(theBest, Ambiente.N);
            gf.setDescricao(" Teste de um Individuo Aleatório");
            gf.Show();
  

        }

        private void singleGenetarionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Run > Single Generation
            GASolver ga = new GASolver();
            String linha = ga.RunGeneration(ref Populacao);
            this.theBest = Populacao[1];
            rtfLOG.Text = NewRTFLog("  Executando uma Geraçao", rtfLOG.Text);
            rtfLOG.Text = NewRTFLog("      "+linha, rtfLOG.Text);


        }

        private void runToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // Run > Multiple Generation
            GASolver ga = new GASolver();
            String linha = ga.RunMultipleGenerations(ref Populacao);
            this.theBest = Populacao[1];
            rtfLOG.Text = NewRTFLog("  Executando Multiplas Geraçao", rtfLOG.Text);
            rtfLOG.Text = NewRTFLog("      " + linha, rtfLOG.Text);


        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Menu: File > Exit
            Application.Exit();
        }
    }
}
