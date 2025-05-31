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
        GLOUTONDECROISSANT,
        EXTREMEENPREMIER,
        EQUILIBREPROGRESSIF,
        NSWAP,
        EQUILIBRESEMIPROGRESSIF,
        ROLEPRINCIPALHEURISTIQUE1,
        NSWAPAMELIORER,
        EQUILIBREPROGRESSIFNIVEAUX2
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
                case NomAlgorithme.GLOUTONDECROISSANT: res = "Algorithme glouton decroissant"; break;
                case NomAlgorithme.EXTREMEENPREMIER: res = "Algorithme extreme en premier"; break;
                case NomAlgorithme.EQUILIBREPROGRESSIF: res = "Equilibre progressif"; break;
                case NomAlgorithme.NSWAP: res = "N-Swap"; break;
                case NomAlgorithme.NSWAPAMELIORER: res = "N-Swap améliorer"; break;
                case NomAlgorithme.EQUILIBRESEMIPROGRESSIF: res = "Equilibre semi-progressif"; break;
                case NomAlgorithme.ROLEPRINCIPALHEURISTIQUE1: res = "Heuristique simple avec rôle principal"; break;
                case NomAlgorithme.EQUILIBREPROGRESSIFNIVEAUX2: res = "Equilibre progressif avec rôle (niveau2)"; break;
            }
            return res;
        }
    }
}
