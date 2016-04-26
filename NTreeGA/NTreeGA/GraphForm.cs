using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Collections;

namespace NTreeGA
{
    
    public class GraphForm : System.Windows.Forms.Form
    {
        public Individuo solucao;      // Dados a serem plotados
        public int N;
        //
        public double XMax, XMin, YMax, YMin;   // Valores máx, min dos Pontos
        public String Descricao;                // Study description
        public double EscalaX, EscalaY;

        public int    Gsize  = 650;
        public double FGsize = 650.0;
        public Boolean drawDist=false; 
        public Boolean drawCover=false; 
//

        Graphics g;
      

        private System.ComponentModel.Container components;

        public void setDimensions()
        {
             XMin = Ambiente._XMin;
             XMax = Ambiente._XMax;
             YMin = Ambiente._YMax;
             YMax = Ambiente._YMin;
             EscalaX = 0.97*FGsize / (XMax - XMin);
             EscalaY = 0.97*FGsize / (YMax - YMin);

 //            Console.WriteLine(EscalaX.ToString() + " " + EscalaY.ToString());

        }

        public void setDescricao(String text)
        {

            this.Descricao = text;

        }

        public void setDrawDist(Boolean vDraw){

            this.drawDist = vDraw;

        }

        public void setDrawCover(Boolean vDraw)
        {

            this.drawCover = vDraw;

        }

        public void setIndividuo(Individuo _sol, int _N)
        {
            this.solucao = _sol;
            N = _N;
        }




        public GraphForm()
        {
            InitializeComponent();
            CenterToScreen();
            SetStyle(ControlStyles.ResizeRedraw, true);
        }

        protected override void Dispose(bool disposing)
        {

            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }

            base.Dispose(disposing);

        }

        private void InitializeComponent()
        {
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(Gsize,Gsize);
            this.Text = "Monitor Solution";
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            //this.BackColor = Color.DarkGray;
 
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.MainForm_PaintDist);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.MainForm_PaintPontos);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.MainForm_Texto);
  
            g = this.CreateGraphics();  

        }

        private void MainForm_Texto(object sender, System.Windows.Forms.PaintEventArgs e)
        {
           float x = (float)(EscalaX * (XMax - XMin)-250);
           float y = (float)(EscalaY * (YMax - YMin)-75);

            g.DrawString( Descricao,
                          new Font("Arial", 9),
                          new SolidBrush(Color.DarkGray), x, y);

        }



        private void MainForm_PaintPontos(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            Ponto o = Ambiente.Origem;
            float x = (float)(EscalaX * (o.x - XMin) - 2.0);
            float y = (float)(EscalaY * (o.y - YMin) - 2.0);
            g.DrawEllipse(new Pen(Color.Black, 4), x, y, 2, 2);

            for (int i = 0; i <= N; i++)
            {                  
                    Ponto p = solucao.getItem(i);
                    if (p.cluster != 0)
                    {
                        x = (float)(EscalaX * (p.x - XMin) - 2.0);
                        y = (float)(EscalaY * (p.y - YMin) - 2.0);
                        g.DrawEllipse(new Pen(Color.Red, 4), x, y, 2, 2);
                    }

            }
            for (int i = 1; i <= N; i++)
            {
                    Ponto p = Ambiente.Destino[i];
                    x = (float)(EscalaX * (p.x - XMin) - 2.0);
                    y = (float)(EscalaY * (p.y - YMin) - 2.0);
                    g.DrawEllipse(new Pen(Color.Blue, 4), x, y, 2, 2);

            }


        }



        private void MainForm_PaintDist(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            // Desenha sempre da Origem ao I0
            Ponto O = Ambiente.Origem;
            Ponto I0 = solucao.getItem(0);
            int x1 = (Int32)(EscalaX * (O.x - XMin));
            int y1 = (Int32)(EscalaY * (O.y - YMin));
            int x2 = (Int32)(EscalaX * (I0.x - XMin));
            int y2 = (Int32)(EscalaY * (I0.y - YMin));
            g.DrawLine(new Pen(Color.Black, 3), x1, y1, x2, y2);

//
            for (int i = 1; i <= N; i++)
            {
                // Só desenha os pontos usados

                Ponto In = solucao.getItem(i);
                /*
                if (In.cluster != 0)
                {
                    x1 = (Int32)(EscalaX * (I0.x - XMin));
                    y1 = (Int32)(EscalaY * (I0.y - YMin));
                    x2 = (Int32)(EscalaX * (In.x - XMin));
                    y2 = (Int32)(EscalaY * (In.y - YMin));
                    g.DrawLine(new Pen(Color.DarkGray, 1), x1, y1, x2, y2);
                }
                */
                // Liga o destino ao ponto mais próximo
                Ponto Dn = Ambiente.Destino[i];
                int iMin = solucao.iMinDistance(Dn);
                In = solucao.getItem(iMin);
                x1 = (Int32)(EscalaX * (Dn.x - XMin));
                y1 = (Int32)(EscalaY * (Dn.y - YMin));
                x2 = (Int32)(EscalaX * (In.x - XMin));
                y2 = (Int32)(EscalaY * (In.y - YMin));
                g.DrawLine(new Pen(Color.Black, 1), x1, y1, x2, y2);
                x1 = (Int32)(EscalaX * (I0.x - XMin));
                y1 = (Int32)(EscalaY * (I0.y - YMin));
                x2 = (Int32)(EscalaX * (In.x - XMin));
                y2 = (Int32)(EscalaY * (In.y - YMin));
                g.DrawLine(new Pen(Color.Black, 1), x1, y1, x2, y2);

 

             }
 
        }

                private void MainForm_Resize(object sender, System.EventArgs e)
        {
        }
    }
}
