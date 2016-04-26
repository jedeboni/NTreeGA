using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTreeGA
{
    public static class Ambiente
    {
        public static int N;                                          // Tamanho dos pontos (externos)
        public static Ponto Origem = new Ponto(0, 0);                 // Ponto de Origem (sempre no 0,0)
        public static Ponto[] Destino = new Ponto[Preferences.NMAX];  // Pontos de Destino
        //
        public static double _XMax = 0.0;  
        public static double _XMin = 0.0;  
        public static double _YMax = 0.0;
        public static double _YMin = 0.0;
        //
        public static double EscalaX;
        public static double EscalaY;
        //

    }
}
