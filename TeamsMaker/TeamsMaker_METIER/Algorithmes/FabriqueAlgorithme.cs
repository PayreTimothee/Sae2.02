﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamsMaker_METIER.Algorithmes.Realisations;
using TeamsMaker_METIER.Problemes;

namespace TeamsMaker_METIER.Algorithmes
{
    /// <summary>
    /// Fabrique des algorithmes
    /// </summary>
    public class FabriqueAlgorithme
    {
        #region --- Propriétés ---
        /// <summary>
        /// Liste des noms des algorithmes
        /// </summary>
        public string[] ListeAlgorithmes => Enum.GetValues(typeof(NomAlgorithme)).Cast<NomAlgorithme>().ToList().Select(nom => nom.Affichage()).ToArray();
        #endregion

        #region --- Méthodes ---
        /// <summary>
        /// Fabrique d'algorithme en fonction du nom de l'algorithme
        /// </summary>
        /// <param name="nomAlgorithme">Nom de l'algorithme</param>
        /// <returns></returns>
        public Algorithme? Creer(NomAlgorithme nomAlgorithme)
        {
            Algorithme res = null;
            switch(nomAlgorithme)
            {
                case NomAlgorithme.GLOUTONCROISSANT: res = new AlgorithmeGloutonCroissant(); break;
                case NomAlgorithme.GLOUTONDECROISSANT: res = new AlgorithmeGloutonDecroissant(); break;
                case NomAlgorithme.EXTREMEENPREMIER: res = new Extreme_en_premier(); break;
                case NomAlgorithme.EQUILIBREPROGRESSIF: res = new Equilibre_progressif(); break;
                case NomAlgorithme.NSWAP: res = new N_Swap(); break;
                case NomAlgorithme.NSWAPAMELIORER: res = new NSwapAmeliore(); break;
                case NomAlgorithme.EQUILIBRESEMIPROGRESSIF: res = new EquilibreSemiProgressif(); break;
                case NomAlgorithme.ROLEPRINCIPALHEURISTIQUE1: res = new Heuristique1_niveau2(); break;
                   case NomAlgorithme.BOXPLOT: res = new Box_Plot(); break;
            }
            return res;
        }
        #endregion
    }
}
