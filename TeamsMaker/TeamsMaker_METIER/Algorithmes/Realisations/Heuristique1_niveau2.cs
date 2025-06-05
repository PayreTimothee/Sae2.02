using System.Diagnostics;
using TeamsMaker_METIER.JeuxTest;
using TeamsMaker_METIER.Personnages;
using TeamsMaker_METIER.Personnages.Classes;

namespace TeamsMaker_METIER.Algorithmes.Realisations
{
    public class Heuristique1_niveau2 : Algorithme
    {
        ///<author> LAMBERT Hugo </author>
        /// <summary>
        /// Créer des équipes de 4 personnages avec un SUPPORT, un TANK et deux DPS
        /// </summary>
        /// <param name="jeuTest"> jeu de test utilisé </param>
        /// <returns> Toutes les équipes de 4 personnages avec un SUPPORT, un TANK et deux DPS </returns>
        public override Repartition Repartir(JeuTest jeuTest)
        {
            // Initialisation de la liste des personnages disponibles, de la répartition et du chronomètre
            List<Personnage> disponibles = jeuTest.Personnages.ToList();
            Repartition repartition = new Repartition(jeuTest);

            // Création du chronomètre pour mesurer le temps d'exécution
            Stopwatch stopwatch = new Stopwatch();

            // Début du chronomètre
            stopwatch.Start();

            while (disponibles.Count >= 4)
            {
                // Création d'une nouvelle équipe
                Equipe equipe = new Equipe();

                // Initialisation des variables pour vérifier les rôles
                bool tank = false;
                bool support = false;
                int dps = 0;

                // Parcours de la liste des personnages disponibles en sens inverse
                for (int j = disponibles.Count - 1; j >= 0; j--)
                {
                    // Initialisation d'un personnage au j-ème index de la liste
                    Personnage personnage = disponibles[j];

                    // Si le personnage correspond au rôle TANK et qu'il n'y a pas déjà de TANK dans l'équipe
                    if (personnage.RolePrincipal == Role.TANK && tank == false)
                    {
                        equipe.AjouterMembre(personnage);
                        tank = true;
                        disponibles.RemoveAt(j);
                    }

                    // Si le personnage correspond au rôle SUPPORT et qu'il n'y a pas déjà de SUPPORT dans l'équipe
                    if (personnage.RolePrincipal == Role.SUPPORT && support == false)
                    {
                        equipe.AjouterMembre(personnage);
                        support = true;
                        disponibles.RemoveAt(j);
                    }

                    // Si le personnage correspond au rôle DPS et qu'il y a moins de 2 DPS dans l'équipe
                    if (personnage.RolePrincipal == Role.DPS && dps < 2)
                    {
                        equipe.AjouterMembre(personnage);
                        dps++;
                        disponibles.RemoveAt(j);
                    }
                }

                // Si l'équipe est complète, l'ajouter à la répartition
                if (equipe.Membres.Length == 4 )
                {
                    repartition.AjouterEquipe(equipe);
                }
            }

            // Fin du chronomètre
            this.TempsExecution = stopwatch.ElapsedMilliseconds;
            stopwatch.Stop();

            return repartition;
        }

        


    }
}
