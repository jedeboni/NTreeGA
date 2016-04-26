using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTreeGA
{
    public static class GAParameters
    {
        //
        // Parametros para o GA
        public static int GA_POPSIZE = 512;           // Tamanho da população
        public static double GA_ELITRATE = 0.10;
        public static double GA_MUTATIONRATE = 0.20;
        public static int GA_MAXGENERATION = 512;
        public static int GA_GCount = 1;
        public static int GA_GSTOP = 5;
        //
        public static Random rand = new Random();
        //

    }
}
