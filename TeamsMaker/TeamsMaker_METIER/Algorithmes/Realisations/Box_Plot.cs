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
    public class Box_Plot : Algorithme
    {
        ///<author> PAYRE Timothée </author>
        /// <summary>
        /// Algorithme de répartition des personnages en séparant les personnages en quartiles selon leur niveau principal, puis en formant des équipes équilibrées à partir de ces quartiles.
        /// </summary>
        /// <param name="jeuTest"> Jeu de test utilisé </param>
        /// <returns> Répartition contenant les équipes de 4 personnages </returns>
        public override Repartition Repartir(JeuTest jeuTest)
        {

                Personnage[] personnages = jeuTest.Personnages;//Récupère le tableau des personnages du jeu de test
                Array.Sort(personnages, new ComparateurPersonnageParNiveauPrincipal());//Fait un trie croissant 

                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

            int total = personnages.Length;//récupère la taille de la liste (le nombre de personnage)
                int quartileSize = total / 4;//Calcule la taille d'un quartile

                // Création des listes pour chaque quartile
                List<Personnage> q1 = new List<Personnage>();
                List<Personnage> q2 = new List<Personnage>();
                List<Personnage> q3 = new List<Personnage>();
                List<Personnage> q4 = new List<Personnage>();

                for (int i = 0; i < total; i++)// ajoute les personnages dans les différents quartiles 
                {
                    if (i < quartileSize)
                        q1.Add(personnages[i]);
                    else if (i < 2 * quartileSize)
                        q2.Add(personnages[i]);
                    else if (i < 3 * quartileSize)
                        q3.Add(personnages[i]);
                    else
                        q4.Add(personnages[i]);
                }

                Repartition repartition = new Repartition(jeuTest);//nouvelle repartition

                while (q1.Count > 0 && q2.Count > 0 && q3.Count > 0 && q4.Count > 0)// Fait les équipes tant qu'il y a assez de personnage
                {
                    Equipe equipe = new Equipe();
                    equipe.AjouterMembre(q1[0]); q1.RemoveAt(0);
                    equipe.AjouterMembre(q2[0]); q2.RemoveAt(0);
                    equipe.AjouterMembre(q3[0]); q3.RemoveAt(0);
                    equipe.AjouterMembre(q4[0]); q4.RemoveAt(0);
                    repartition.AjouterEquipe(equipe);
                }

                stopwatch.Stop();
                this.TempsExecution = stopwatch.ElapsedMilliseconds;

                return repartition;
        }
    }
}

