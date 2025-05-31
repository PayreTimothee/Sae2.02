using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamsMaker_METIER.Algorithmes.Outils;
using TeamsMaker_METIER.JeuxTest;
using TeamsMaker_METIER.Personnages;
using TeamsMaker_METIER.Personnages.Classes;

namespace TeamsMaker_METIER.Algorithmes.Realisations
{
    public class Heuristique1_niveau2 : Algorithme
    {
        /// <summary>
        /// Créer des équipes de 4 personnages avec un SUPPORT, un TANK et deux DPS
        /// </summary>
        /// <param name="jeuTest"> jeu de test utilisé </param>
        /// <returns> Toutes les équipes de 4 personnages avec un SUPPORT, un TANK et deux DPS </returns>
        public override Repartition Repartir(JeuTest jeuTest)
        {
            //Initialisation de la liste des personnages disponibles, de la répartition et du chronomètre
            List<Personnage> disponibles = jeuTest.Personnages.ToList();
            Repartition repartition = new Repartition(jeuTest);

            // Création du chronomètre pour mesurer le temps d'exécution
            Stopwatch stopwatch = new Stopwatch();

            //Début du chronomètre
            stopwatch.Start();

            while (disponibles.Count >= 4)
            {
                //Création d'une nouvelle équipe
                Equipe equipe = new Equipe();

                //Initalisation des variables pour vérifier les rôles
                bool tank = false;
                bool support = false;
                int dps = 0;

                //Parcours de la liste des personnages disponibles à partir du premier personnage
                for (int j = 0; j < disponibles.Count; j++)
                {
                    //Initialisation d'un personnage au j-ème index de la liste
                    Personnage personnage = disponibles[j];

                    //Si le personnage de la liste correspond au role TANK et qu'il n'y a pas déjà de TANK dans l'équipe, on l'ajoute dans la liste
                    if (personnage.RolePrincipal == Role.TANK && tank != true)
                    {
                        equipe.AjouterMembre(personnage);
                        tank = true;
                        disponibles.RemoveAt(j);
                    }

                    //Si le personnage de la liste correspond au role SUPPORT et qu'il n'y a pas déjà de SUPPORT dans l'équipe, on l'ajoute dans la liste
                    if (personnage.RolePrincipal == Role.SUPPORT && support != true)
                    {
                        equipe.AjouterMembre(personnage);
                        support = true;
                        disponibles.RemoveAt(j);
                    }

                    //Si le personnage de la liste correspond au role DPS et qu'il y a moins de 2 DPS dans l'équipe, on l'ajoute dans la liste
                    if (personnage.RolePrincipal == Role.DPS && dps < 2)
                    {
                        equipe.AjouterMembre(personnage);
                        dps++;
                        disponibles.RemoveAt(j);
                    }
                }
                
                //Si il y a au moins un TANK, un SUPPORT et deux DPS, on ajoute l'équipe à la répartition
                if (equipe.Membres.Length == 4)
                {
                    repartition.AjouterEquipe(equipe);
                }
            }

            //Fin du chronomètre
            this.TempsExecution = stopwatch.ElapsedMilliseconds;
            stopwatch.Stop();

            return repartition;
        }

    }
}
