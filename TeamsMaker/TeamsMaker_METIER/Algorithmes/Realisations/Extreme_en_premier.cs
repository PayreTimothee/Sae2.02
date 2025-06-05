using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamsMaker_METIER.Algorithmes.Outils;
using TeamsMaker_METIER.JeuxTest;
using TeamsMaker_METIER.Personnages;

namespace TeamsMaker_METIER.Algorithmes.Realisations
{
    /// <summary>
    /// Algorithme qui répartit les personnages en équipes de 4 en prenant les 2 plus faibles et les 2 plus forts à chaque itération.
    /// </summary>
    public class Extreme_en_premier : Algorithme
    {
        ///<author> LAMBERT Hugo </author>
        /// <summary>
        /// Répartit les personnages d'un jeu de test en équipes de 4 en prenant les 2 plus faibles et les 2 plus forts à chaque itération.
        /// </summary>
        /// <param name="jeuTest"> Jeu de test utilisé </param>
        /// <returns> Repartition contenant les équipes de 4 personnages </returns>
        public override Repartition Repartir(JeuTest jeuTest)
        {
            //Initialisation d'un tableau contenant les personnages
            Personnage[] personnages = jeuTest.Personnages;

            //Tri des personnages par niveau principal
            Array.Sort(personnages, new ComparateurPersonnageParNiveauPrincipal());

            // Initialisation de la répartition
            Repartition repartition = new Repartition(jeuTest);

            // Création du chronomètre pour mesurer le temps d'exécution
            Stopwatch stopwatch = new Stopwatch();

            // Démarrage du chronomètre
            stopwatch.Start();

            //Index des personnages les plus forts et les plus faibles
            int plus_fort = personnages.Length;

            // Index des personnages les moins forts
            int moins_fort = 0;

            for (int i = 0; i <= personnages.Length - 4; i+=4)
            {

                Equipe equipe = new Equipe();
                for (int j = moins_fort; j < moins_fort + 2; j++) 
                {
                    equipe.AjouterMembre(personnages[j]);
                }
                for (int k = plus_fort - 1 ; k >= plus_fort - 2; k--)
                {
                    equipe.AjouterMembre(personnages[k]);
                }

                //Augmente les index
                moins_fort = moins_fort + 2;
                plus_fort = plus_fort - 2;

                //Ajout de l'équipe à la répartition
                repartition.AjouterEquipe(equipe);
            }
            // Stop du chronomètre
            this.TempsExecution = stopwatch.ElapsedMilliseconds;
            stopwatch.Stop();
            return repartition;
        }
    }
}
