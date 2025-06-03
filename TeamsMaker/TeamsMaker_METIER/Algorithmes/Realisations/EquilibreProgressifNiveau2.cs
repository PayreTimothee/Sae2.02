using System.Diagnostics;
using TeamsMaker_METIER.Algorithmes.Outils;
using TeamsMaker_METIER.JeuxTest;
using TeamsMaker_METIER.Personnages;
using TeamsMaker_METIER.Personnages.Classes;
using TeamsMaker_METIER.Problemes;

namespace TeamsMaker_METIER.Algorithmes.Realisations
{
    internal class EquilibreProgressifNiveau2 : Algorithme
    {
        ///<author> LAMBERT Hugo </author>
        /// <summary>
        /// Algorithme Equilibre progressif adapté avec les rôles (niveau2)
        /// </summary>
        /// <param name="jeuTest"> jeu de test utilisé </param>
        /// <returns> Toutes les équipes de 4 personnages </returns>
        public override Repartition Repartir(JeuTest jeuTest)
        {
            //Initialisation de la liste des personnages
            Personnage[] personnages = jeuTest.Personnages;

            //Initialisation de la liste des personnages restants et de la répartition
            List<Personnage> personnagesRestants = new List<Personnage>(personnages);
            Repartition repartition = new Repartition(jeuTest);

            //Création du chronomètre pour mesurer le temps d'exécution
            Stopwatch stopwatch = new Stopwatch();
            // Démarrage du chronomètre
            stopwatch.Start();

            // Initialisation des listes pour les rôles
            List<Personnage> listTank = new List<Personnage>();
            List<Personnage> listDps = new List<Personnage>();
            List<Personnage> listSupport = new List<Personnage>();

            //Ajout dans les différentes listes les personnages en fonction de leur rôle
            foreach (Personnage personnage in personnagesRestants)
            {
                if (personnage.RolePrincipal == Role.TANK)
                {
                    listTank.Add(personnage);
                }
                else if (personnage.RolePrincipal == Role.DPS)
                {
                    listDps.Add(personnage);
                }
                else if (personnage.RolePrincipal == Role.SUPPORT)
                {
                    listSupport.Add(personnage);
                }
            }

            // Tri des listes par niveau principal
            Array.Sort(listTank.ToArray(), new ComparateurPersonnageParNiveauPrincipal());
            Array.Sort(listDps.ToArray(), new ComparateurPersonnageParNiveauPrincipal());
            Array.Sort(listSupport.ToArray(), new ComparateurPersonnageParNiveauPrincipal());


            for (int i = 0; i < personnages.Length - 4; i += 4)
            {
                // Créer une nouvelle équipe
                Equipe equipe = new Equipe();

                // Initialiser une liste pour stocker les membres de l'équipe
                List<Personnage> membresEquipe = new List<Personnage>();

                //Ajout du tank le plus faible puis suppression de la liste pour éviter les doublons
                if (listTank.Count > 0)
                {
                    membresEquipe.Add(listTank[0]);
                    listTank.RemoveAt(0);
                }

                //Ajout du support le plus fort puis suppression de la liste pour éviter les doublons
                if (listSupport.Count > 0)
                {
                    membresEquipe.Add(listSupport[listSupport.Count - 1]);
                    listSupport.RemoveAt(listSupport.Count - 1);
                }

                while (membresEquipe.Count < 4)
                {
                    // Trouver le meilleur candidat pour l'équipe
                    Personnage? meilleurCandidat = null;

                    // Initialiser la proximité minimale à une valeur très élevée
                    double meilleureProximite = double.MaxValue;

                    // Parcourir tous les personnages DPS restants pour trouver le meilleur candidat
                    foreach (Personnage personnage in listDps)
                    {
                        // Calculer la moyenne actuelle des niveaux principaux des membres de l'équipe
                        double moyenneActuelle = 0;
                        if (membresEquipe.Count > 0)
                        {
                            double sommeNiveaux = 0;
                            foreach (Personnage membre in membresEquipe)
                            {
                                sommeNiveaux += membre.LvlPrincipal;
                            }
                            moyenneActuelle = sommeNiveaux / membresEquipe.Count;
                        }

                        // Calculer la nouvelle moyenne si le personnage a été ajouté
                        double nouvelleMoyenne = (moyenneActuelle * membresEquipe.Count + personnage.LvlPrincipal) / (membresEquipe.Count + 1);

                        //Calcule de la valeur absolue de la nouvelle moyenne par rapport à 50
                        double proximite = Math.Abs(50 - nouvelleMoyenne);

                        // Vérifier si le personnage est un meilleur candidat
                        if (proximite < meilleureProximite)
                        {
                            meilleureProximite = proximite;
                            meilleurCandidat = personnage;
                        }
                    }

                    // Si un meilleur candidat a été trouvé, l'ajouter à l'équipe
                    if (meilleurCandidat != null)
                    {
                        membresEquipe.Add(meilleurCandidat);
                        listDps.Remove(meilleurCandidat);
                    }
                    // Sinon, on sort de la boucle pour éviter les boucles infinies
                    else
                    {
                        break;
                    }
                }

                // Ajouter tous les membres à l'équipe
                foreach (Personnage membre in membresEquipe)
                {
                    equipe.AjouterMembre(membre);
                }

                // Vérifier si il y a 4 personnages dans l'équipe et si elle est valide pour le problème ROLEPRINCIPAL
                if (equipe.Membres.Length == 4 && equipe.EstValide(Probleme.ROLEPRINCIPAL))
                {
                    repartition.AjouterEquipe(equipe);
                }
            }

            //Fin du chronomètre
            stopwatch.Stop();
            this.TempsExecution = stopwatch.ElapsedMilliseconds;

            return repartition;
        }
    }
}

