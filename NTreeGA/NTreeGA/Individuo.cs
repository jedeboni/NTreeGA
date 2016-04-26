using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTreeGA
{
    public class Individuo
    {
        // Esta classe define o DNA de um individuo (solução) 
        // O indiviuo deste problema é ...definir

        public Ponto[] internos = new Ponto[Preferences.NMAX]; // <<<<< rever o tamanho da lista >>>>>
        public double fitness;                    // Valor do ajuste do individuo (valor carregado na sacola)
        //
        public Individuo()
        {
        }


        public Individuo geraAleatorio()
        {
            int N = Ambiente.N;
            Individuo feto = new Individuo();
            for (int i = 0; i <= N; i++)
            {
                double x = Ambiente._XMin + GAParameters.rand.NextDouble()*Ambiente.EscalaX;
                double y = Ambiente._YMin + GAParameters.rand.NextDouble()*Ambiente.EscalaY;
                feto.internos[i] = new Ponto(x, y);
                //Console.WriteLine("Gerou x = " + x.ToString("0.0") + "  y = " + y.ToString("0.0"));
   
            }
            feto.setFitness(feto.CalcFitness());
            return feto;
        }

        public Individuo copy()
        {
           int N = Ambiente.N;
           Individuo clone = new Individuo();
           for (int i = 0; i <= N; i++)
            {
                clone.setItem(i, this.internos[i]);
            }
            clone.setFitness(this.fitness);
            return clone;
        }

        public void Escala(double _MinimoX, double _MaximoX, double _MinimoY, double _MaximoY)
        {
            Ambiente._XMax = _MaximoX;
            Ambiente._YMin = _MinimoY;
            Ambiente._XMin = _MinimoX;
            Ambiente._YMax = _MaximoY;
            // 
            Ambiente.EscalaX = (double)(Ambiente._XMax - Ambiente._XMin);
            Ambiente.EscalaY = (double)(Ambiente._YMax - Ambiente._YMin);

        }

 
        public int iMinDistance(Ponto p)
        {
            // select the closest interno to 
            int iMin = 0;
            double minD = p.Distance(internos[0]);
            for (int k = 1; k <= Ambiente.N; k++)
            {
                if (minD > p.Distance(internos[k]))
                {
                    iMin = k;
                    minD = p.Distance(internos[k]);
                }
                internos[iMin].cluster = 1;
            }
            return iMin;
        }

        public double CalcFitness()
        {
            int N = Ambiente.N;
            double myFitness = 0.0;
            
            // Liga a origem ao ponto de i0 (sempre)
            myFitness = Ambiente.Origem.Distance(internos[0]);

            // Zera os clusters internos (todos) para analisar de novo
            for (int i=1; i <= N; i++)
            {
                internos[i].cluster = 0;                            
            }
            for (int i = 1; i <= N; i++)
            {
                    // Escolhe e liga o destino o centro mais proximo que vira um cluster
                    int iMin = iMinDistance(Ambiente.Destino[i]);
                    myFitness = myFitness + internos[iMin].Distance(Ambiente.Destino[i]);
            }
            for (int i = 1; i <= N; i++)
            {
                // Liga os pontos internos usados (clusters) ao i0
                if (internos[i].cluster != 0)
                {
                    myFitness = myFitness + internos[0].Distance(internos[i]);
                }
            }

            //
            fitness = myFitness;  // salva o fitness para não ficar calculando toda hora
            return myFitness;
        }

        public double getFitness()
        {
            return fitness;
        }

        public void setFitness(double _NewFitness)
        {
            fitness = _NewFitness;
        }

        public Ponto getItem(int _id)
        {
            return (internos[_id]);
        }

        public void setItem(int _i, Ponto _p)
        {
            this.internos[_i] = _p;
        }

    }
}
