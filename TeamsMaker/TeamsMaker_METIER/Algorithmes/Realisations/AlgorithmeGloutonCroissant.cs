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
    public class AlgorithmeGloutonCroissant : Algorithme
    {
        /// <summary>
        /// Algorithme glouton croissant pour répartir les personnages d'un jeu de test en équipes de 4 membres.
        /// </summary>
        /// <param name="jeuTest"> Jeu de test utilisé </param>
        /// <returns> Répartition contenant les équipes de 4 personnages </returns>
        public override Repartition Repartir(JeuTest jeuTest)
        {
            //Initialisation d'un tableau contenant les personnages
            Personnage[] personnages = jeuTest.Personnages;

            // Tri des personnages par niveau principal en ordre croissant
            Array.Sort(personnages, new ComparateurPersonnageParNiveauPrincipal());

            // Initialisation de la répartition
            Repartition repartition = new Repartition(jeuTest);

            // Création du chronomètre pour mesurer le temps d'exécution
            Stopwatch stopwatch = new Stopwatch();

            //Start du chronomètre
            stopwatch.Start();
            for (int i = 0; i <= personnages.Length-4; i+=4)
            {
                
                Equipe equipe  = new Equipe();
                for(int j = i;j < i +4; j++)
                {
                    equipe.AjouterMembre(personnages[j]);

                }
                repartition.AjouterEquipe(equipe);
            }

            //Stop du chronomètre
            this.TempsExecution = stopwatch.ElapsedMilliseconds;
            stopwatch.Stop();
            
            return repartition;
            
        }
    }
}
