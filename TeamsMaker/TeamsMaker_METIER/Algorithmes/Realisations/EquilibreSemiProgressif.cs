﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TeamsMaker_METIER.Algorithmes.Outils;
using TeamsMaker_METIER.JeuxTest;
using TeamsMaker_METIER.Personnages;

namespace TeamsMaker_METIER.Algorithmes.Realisations
{
    public class EquilibreSemiProgressif : Algorithme
    {
        /// <summary>
        /// Algorithme de répartition semi-progressif qui répartit les personnages en équipes équilibrées
        /// </summary>
        /// <param name="jeuTest"> Jeu de test utilisé </param>
        /// <return> Répartition contenant les équipe de 4 personnages</returns>
        public override Repartition Repartir(JeuTest jeuTest)
        {
            //Création du chronomètre pour mesurer le temps d'exécution
            Stopwatch stopwatch = new Stopwatch();

            // Démarrage du chronomètre
            stopwatch.Start();

            //Initialisation de la liste des personnages
            Personnage[] personnages = jeuTest.Personnages;

            // Tri des personnages par niveau principal croissant
            Array.Sort(personnages, new ComparateurPersonnageParNiveauPrincipal());

            //Initialisation de la liste des personnages restants et de la répartition
            List<Personnage> personnagesRestants = new List<Personnage>(personnages);
            Repartition repartition = new Repartition(jeuTest);

            for (int i = 0; i <= personnages.Length; i += 4)
            {
                // Créer une nouvelle équipe
                Equipe equipe = new Equipe();

                // Initialiser une liste pour stocker les membres de l'équipe
                List<Personnage> membresEquipe = new List<Personnage>();

                // Ajouter le premier personnage à l'équipe
                //membresEquipe.Add(personnagesRestants[0]); ///////////////////

                if (personnagesRestants.Count > 0)
                {
                    membresEquipe.Add(personnagesRestants[0]);

                    // Supprimer le premier personnage de la liste des personnages restants
                    personnagesRestants.RemoveAt(0);
                }
                //Sinon on sort de la boucle pour éviter les boucles infini
                else
                {
                    break;
                }
                
                if (personnagesRestants.Count > 0)
                {
                    // Ajouter le dernier personnage à l'équipe
                    membresEquipe.Add(personnagesRestants[personnagesRestants.Count - 1]);

                    // Supprimer le dernier personnage de la liste des personnages restants
                    personnagesRestants.RemoveAt(personnagesRestants.Count - 1);
                }
                //Sinon on sort de la boucle pour éviter les boucles infini
                else
                {
                    break;
                }


                while (membresEquipe.Count < 4)
                {
                    // Trouver le meilleur candidat pour l'équipe
                    Personnage? meilleurCandidat = null;

                    // Initialiser la proximité minimale à une valeur très élevée
                    double meilleureProximite = double.MaxValue;

                    // Parcourir tous les personnages restants pour trouver le meilleur candidat
                    foreach (Personnage personnage in personnagesRestants)
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
                        personnagesRestants.Remove(meilleurCandidat);
                    }
                }

                // Ajouter tous les membres à l'équipe
                foreach (Personnage membre in membresEquipe)
                {
                    equipe.AjouterMembre(membre);
                }
                repartition.AjouterEquipe(equipe);
            }

            //Fin du chronomètre
            stopwatch.Stop();
            this.TempsExecution = stopwatch.ElapsedMilliseconds;

            return repartition;
        }
    }
}

