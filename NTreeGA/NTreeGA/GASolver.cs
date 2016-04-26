using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTreeGA
{
    class GASolver
    {
        
         public String GAInitPopulation(ref Individuo[] Populacao)
         {
             Individuo _Auxiliar = new Individuo();
             for (int i = 1; i <= GAParameters.GA_POPSIZE; i++)
             {
                 Populacao[i] = _Auxiliar.geraAleatorio();
                 //Populacao[i].setFitness(Populacao[i].CalcFitness());

             }
             //
             SortElite(ref Populacao);
             //
             return ("G = " + GAParameters.GA_GCount.ToString("000") +
                      " Best Fit = " + Populacao[1].getFitness().ToString("00.00"));
 
         }
         
        

         public void SortElite(ref Individuo[] _pop)
         {
             // Ordena a Elite e coloca no inicio da população

             int EliteSize = (int)(GAParameters.GA_POPSIZE * GAParameters.GA_ELITRATE);
             Individuo auxiliar;
             for (int i = 1; i < EliteSize; i++)
             {
                 int iBest = i;
                 double vBest = _pop[i].getFitness();
                 for (int j = i; j < GAParameters.GA_POPSIZE; j++)
                 {
                     if (vBest > _pop[j].getFitness())  // pegar o menor valor de ajuste (minimima distância)
                     {
                         vBest = _pop[j].getFitness();
                         iBest = j;
                     }
                 }
                 auxiliar = _pop[i];
                 _pop[i] = _pop[iBest].copy();
                 _pop[iBest] = auxiliar;

             }
         }

         public String  RunGeneration(ref Individuo[] _pop)
         {
             // RUN ONE STEP
             //
             int EliteSize = (int)(GAParameters.GA_POPSIZE * GAParameters.GA_ELITRATE);
             for (int i = EliteSize; i < (GAParameters.GA_POPSIZE); i++)
             {
                 int iPai = 1 + (int)(EliteSize * GAParameters.rand.NextDouble());
                 int iMae = 1 + (int)(EliteSize * GAParameters.rand.NextDouble());
                 int corte = (int)(Ambiente.N * GAParameters.rand.NextDouble());

                 // Cross over entre os pais geram um filho
                 //
                 Individuo filho = _pop[iPai].copy();
                 //
                 for (int ic = corte; ic < Ambiente.N; ic++)
                 {
                     filho.setItem(ic, _pop[iMae].getItem(ic));
                 }
                 // Realiza uma mutação de acordo com a taxa definida
                 if (GAParameters.rand.NextDouble() < GAParameters.GA_MUTATIONRATE)
                 {
                     int _id = (int)(Ambiente.N * GAParameters.rand.NextDouble());
                     Individuo mutante = new Individuo();
                     mutante = mutante.geraAleatorio();
                     filho.setItem(_id, mutante.getItem(_id));
                 }
                 //
                 filho.setFitness(filho.CalcFitness());
                 _pop[i] = filho;
                 //_pop[i].setFitness(_pop[i].CalcFitness());
             }
             GAParameters.GA_GCount++;
             SortElite(ref _pop);

             return ("G = " + GAParameters.GA_GCount.ToString("000") +
                      " Best Fit = " + _pop[1].getFitness().ToString("00.00"));
             
         }


         public String RunMultipleGenerations(ref Individuo[] _pop)
         {
             // RUN ALL AT ONCE

             int _RepeatFit = 0;
             double _LastBestFit = 0;
             String linha;
             while ((GAParameters.GA_GCount <= GAParameters.GA_MAXGENERATION) && (_RepeatFit <= GAParameters.GA_GSTOP))
             {
                 linha = RunGeneration(ref _pop);
                 if (_pop[1].getFitness() == _LastBestFit)  // Conta o número que o BestFit se repete.
                 {
                     _RepeatFit++;
                 }
                 else
                 {
                     _RepeatFit = 0;
                     _LastBestFit = _pop[1].getFitness();
                 }

             }
                    return ("G = " + GAParameters.GA_GCount.ToString("000") +
                      " Best Fit = " + _pop[1].getFitness().ToString("00.00"));

         }


    }
}
