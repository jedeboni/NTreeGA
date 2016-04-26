using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTreeGA
{
    public static class Preferences
    {
        // Variáveis Globais
        public static int NMAX = 10000;                 // Initial large vector sizes
        public static double VeryBigDouble = 9.99E99;    // Initial values for iteractions
        public static Int32 VeryBigInteger = 9999999;    //

        // Parâmetros de Controle do Processo Iterativo
        public static double _delta = 0.01;                // Precisão desejada da iteração;
        public static int _itemMax = 100;                 // Número máximo de iterações
        public static char _delimiter = ';';              // Delimitador dos arquivos CSV
        public static String _logPath = "C:/GASOLVER/";   // Diretório de escrita do LOG
        public static int _rndStart = 1;                  // Numero de médias para o chute inicial
        
    }
}
