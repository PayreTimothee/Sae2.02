using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamsMaker_METIER.Algorithmes
{
    /// <summary>
    /// Liste des noms d'algorithmes
    /// </summary>
    public enum NomAlgorithme
    {
        GLOUTONCROISSANT,
        EXTREMEENPREMIER,
        EQUILIBREPROGRESSIF,
        NSWAP,
        NOPT,
        NOPT2,
        EQUILIBRESEMIPROGRESSIF,
        ROLEPRINCIPALHEURISTIQUE1,
        BOXPLOT,
        EQUILIBREPROGRESSIFNIVEAU2,
        NSWAPNIVEAU2,
        MOYENNE3,
        NSWAPNIVEAU3
    }


    public static class NomAlgorithmeExt
    {
        /// <summary>
        /// Affichage du nom de l'algorithme
        /// </summary>
        /// <param name="algo">NomAlgorithme</param>
        /// <returns>La chaine de caractères à afficher</returns>
        public static string Affichage(this NomAlgorithme algo)
        {
            string res = "Algorithme non nommé :(";
            switch(algo)
            {
                case NomAlgorithme.GLOUTONCROISSANT: res = "Algorithme glouton croissant"; break;
                case NomAlgorithme.EXTREMEENPREMIER: res = "Algorithme extreme en premier"; break;
                case NomAlgorithme.EQUILIBREPROGRESSIF: res = "Equilibre progressif"; break;
                case NomAlgorithme.NSWAP: res = "N-Swap"; break;
                case NomAlgorithme.NOPT: res = "N-Opt"; break;
                case NomAlgorithme.NOPT2: res = "N-Opt niveau 2"; break;
                case NomAlgorithme.EQUILIBRESEMIPROGRESSIF: res = "Equilibre semi-progressif"; break;
                case NomAlgorithme.ROLEPRINCIPALHEURISTIQUE1: res = "Heuristique simple avec rôle principal"; break;
                case NomAlgorithme.BOXPLOT: res = "Algorithme Boxplot"; break;
                case NomAlgorithme.EQUILIBREPROGRESSIFNIVEAU2: res = "Equilibre progressif niveau 2"; break;
                case NomAlgorithme.NSWAPNIVEAU2: res = "N-Swap niveau 2"; break;
                case NomAlgorithme.MOYENNE3: res = "Moyenne niveau 3"; break;
                case NomAlgorithme.NSWAPNIVEAU3: res = "N-Swap niveau 3"; break;
            }
            return res;
        }
    }
}
