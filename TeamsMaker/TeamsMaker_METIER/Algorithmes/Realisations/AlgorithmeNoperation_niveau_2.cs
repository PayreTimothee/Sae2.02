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
using TeamsMaker_METIER.Problemes;

namespace TeamsMaker_METIER.Algorithmes.Realisations
{
    public class AlgorithmeNoperation2 : Algorithme
    {
        private int n;
        private int max;
        
        public AlgorithmeNoperation2() // Constructeur par défaut
        {
            this.n = 2; // Nombre d'équipes à choisir
            this.max = 10; // Nombre maximum d'itérations
        }

        public AlgorithmeNoperation2(int n, int max)
        {
            this.n = n;
            this.max = max;
        }

        public override Repartition Repartir(JeuTest jeuTest)
        {
            Stopwatch sw = new Stopwatch(); //on crée un stopwatch pour mesurer le temps d'exécution de l'algorithme
            sw.Start();//On démarre le stopwatch
            Repartition repartition = new Heuristique1_niveau2().Repartir(jeuTest);//On utilise l'algorithme glouton pour créer une répartition initiale
            repartition.LancerEvaluation(new Probleme());
            
            for (int i = 0; i < max; i++) //On va faire n itérations pour trouver la meilleure répartition  
            {
                
                double MeilleurScore = repartition.Score; //On suppose que le meilleur score est le score de la répartition initiale
                Boolean estMeilleur = true; //On suppose que la répartition initiale est la meilleure répartition
               while (estMeilleur) //On boucle tant que la répartition est meilleure que la précédente
                {
                    repartition = déterminerRepartion(repartition, jeuTest);
                   Probleme probleme = new Probleme(); //On crée un nouveau problème pour évaluer la répartition
                    probleme = Probleme.ROLEPRINCIPAL;
                    repartition.LancerEvaluation(probleme);
                    if (MeilleurScore > repartition.Score && repartition.Score!= -1) //Si la nouvelle répartition est meilleure que la précédente et que le score n'est pas -1 (ce que la répartion est invalide)
                    {
                        MeilleurScore = repartition.Score; //On met à jour le meilleur score
                        estMeilleur = true; //On continue à chercher une meilleure répartition
                    }
                    else
                    {
                       estMeilleur = false; //On arrête de chercher une meilleure répartition
                    }
                }
            }
            sw.Stop();//On arrête le stopwatch
            this.TempsExecution = sw.ElapsedMilliseconds; //On enregistre le temps d'exécution de l'algorithme
            return repartition; //On retourne la répartition finale
        }

        private Repartition déterminerRepartion(Repartition inital,JeuTest jeuTest)
        {
            //timer
            n = this.n;
            //initalisation de la repartion
            Repartition repartition = inital; // On initialise la répartition avec la répartition initiale
            repartition.LancerEvaluation(new Probleme());
            
            List<Equipe> repartionEquipe = new List<Equipe>();
            int nombreEquipe = 0;
            foreach (Equipe equipe in repartition.Equipes)
            {
                nombreEquipe++;
                repartionEquipe.Add(equipe);
            }
            Random random = new Random();
            //on va choisir un nombre d'équipe aléatoire entre 1 et le nombre d'équipe moins les n équipes choisies
            Boolean estValide = false;
            int[] equipes = new int[n];
            do
            {
                for (int i = 0; i < n; i++)
                {
                    equipes[i] = random.Next(0, nombreEquipe); // On choisit un nombre d'équipe aléatoire entre 0 et le nombre d'équipe
                }
                estValide = true; // On suppose que l'équipe est valide
                for (int i = 0; i < n; i++)
                {
                    if (equipes[i] >= nombreEquipe || equipes[i] < 0) // Si l'équipe choisie est en dehors des limites
                    {
                        estValide = false; // L'équipe n'est pas valide
                        break;
                    }
                    for (int j = 0; j < n; j++)
                    {
                        if (i != j && equipes[i] == equipes[j]) // Si deux équipes sont identiques
                        {
                            estValide = false; // L'équipe n'est pas valide
                            break;
                        }
                    }
                }

            }
            while (estValide == false); // On boucle jusqu'à ce que l'on trouve une équipe valide
            
            List<Equipe> equipesChoisies = new List<Equipe>();
            List<Equipe> equipesNonChoisies = new List<Equipe>(); //sera utilisée pour les équipes non choisies et pour créer la repartition finale
            List<Personnage> personnagesSansEquipe = new List<Personnage>(repartition.PersonnagesSansEquipe); // Liste des personnages qui ne sont pas dans une équipe

            for (int i = 0; i < nombreEquipe; i++)
            {
                if (equipes.Contains(i))
                {
                    equipesChoisies.Add(repartionEquipe[i]);
                }
                else
                {
                    equipesNonChoisies.Add(repartionEquipe[i]);
                }
            }
            List<Personnage> dps = new List<Personnage>(); // Liste des dps choisis dans les équipes choisies
            List<Personnage> tanck = new List<Personnage>();//Liste des tanck choisis dans les équipes choisies
            List<Personnage> support = new List<Personnage>(); // Liste des supports choisis dans les équipes choisies

            foreach (Equipe equipe in equipesChoisies)
            {
                foreach (Personnage personnage in equipe.Membres)
                {
                    if (personnage.RolePrincipal == Role.DPS)
                    {
                        dps.Add(personnage); // On ajoute le personnage à la liste des dps
                    }
                    else if (personnage.RolePrincipal == Role.TANK)
                    {
                        tanck.Add(personnage); // On ajoute le personnage à la liste des tanck
                    }
                    else if (personnage.RolePrincipal == Role.SUPPORT)
                    {
                        support.Add(personnage); // On ajoute le personnage à la liste des personnages sans équipe
                    }
                }
            }

            equipesChoisies.Clear(); // On vide la liste des équipes choisies pour la suite de l'algorithme

           
            for (int i = 1; i < n + 1; i++)
            {
                // On va créer une nouvelle équipe avec les personnages choisis
                Equipe nouvelleEquipe = new Equipe();
                // On va choisir les personnages de l'équipe en en calulant la moyenne des niveaux
                //on initalise la moyenne des niveaux à 0
                double moyenneNiveau = 0;
                double ecart = 100; // Ecart entre le niveau du personnage choisi et la moyenne des niveaux
                Personnage personnageChoisi = null; // Personnage choisi pour l'équipe
                for (int j = 1; j<= 2; j++) // On choisit 2 dps
                {
                    foreach (Personnage personnage in dps)
                    {
                        double nouvelleMoyenneNiveau = ((moyenneNiveau * nouvelleEquipe.Membres.Count() )+ personnage.LvlPrincipal) / (nouvelleEquipe.Membres.Count()+1); // On calcule la nouvelle moyenne des niveaux
                        double nouveauecart = Math.Abs(nouvelleMoyenneNiveau - 50); // On calcule l'écart entre la nouvelle moyenne et 50
                        if(ecart>nouveauecart)
                            {
                            ecart = nouveauecart; // On met à jour l'écart
                            personnageChoisi = personnage; // On met à jour le personnage choisi
                        }
                        
                    }
                    moyenneNiveau = ((moyenneNiveau * nouvelleEquipe.Membres.Count()) + personnageChoisi.LvlPrincipal) / (nouvelleEquipe.Membres.Count() + 1); // On met à jour la moyenne des niveaux
                    nouvelleEquipe.AjouterMembre(personnageChoisi); // On ajoute le personnage choisi à l'équipe
                    dps.Remove(personnageChoisi);
                }
                
                ecart = 100; // On réinitialise l'écart pour le prochain personnage
                personnageChoisi = null; // On réinitialise le personnage choisi
                foreach (Personnage personnage in tanck) // On cherche le meileur dps
                {
                    double nouvelleMoyenneNiveau = ((moyenneNiveau * nouvelleEquipe.Membres.Count()) + personnage.LvlPrincipal) / (nouvelleEquipe.Membres.Count() + 1); // On calcule la nouvelle moyenne des niveaux
                    double nouveauecart = Math.Abs(nouvelleMoyenneNiveau - 50); // On calcule l'écart entre la nouvelle moyenne et 50
                    if(ecart > nouveauecart) // Si l'écart est plus petit que l'écart précédent
                    {
                        ecart = nouveauecart; // On met à jour l'écart
                        personnageChoisi = personnage; // On met à jour le personnage choisi
                    }
                }
                moyenneNiveau = ((moyenneNiveau * nouvelleEquipe.Membres.Count()) + personnageChoisi.LvlPrincipal) / (nouvelleEquipe.Membres.Count() + 1); // On met à jour la moyenne des niveaux
                nouvelleEquipe.AjouterMembre(personnageChoisi); // On ajoute le personnage choisi à l'équipe
                tanck.Remove(personnageChoisi); // On retire le personnage choisi de la liste des tanck

                ecart = 100; // On réinitialise l'écart pour le prochain personnage
                personnageChoisi = null;
                foreach(Personnage personnage in support)
                {
                    double nouvelleMoyenneNiveau = ((moyenneNiveau * nouvelleEquipe.Membres.Count()) + personnage.LvlPrincipal) / (nouvelleEquipe.Membres.Count() + 1); // On calcule la nouvelle moyenne des niveaux
                    double nouveauecart = Math.Abs(nouvelleMoyenneNiveau - 50); // On calcule l'écart entre la nouvelle moyenne et 50
                    if(ecart > nouveauecart) // Si l'écart est plus petit que l'écart précédent
                    {
                        ecart = nouveauecart; // On met à jour l'écart
                        personnageChoisi = personnage; // On met à jour le personnage choisi
                    }
                }
                moyenneNiveau = ((moyenneNiveau * nouvelleEquipe.Membres.Count()) + personnageChoisi.LvlPrincipal) / (nouvelleEquipe.Membres.Count() + 1); // On met à jour la moyenne des niveaux
                nouvelleEquipe.AjouterMembre(personnageChoisi); // On ajoute le personnage choisi à l'équipe
                support.Remove(personnageChoisi); // On retire le personnage choisi de la liste des supports

                // On ajoute la nouvelle équipe à la liste des équipes choisies
                equipesChoisies.Add(nouvelleEquipe);
            }
            
            
            //On va créer la nouvelle répartition avec les équipes choisies et les équipes choisies

            Repartition nouvelleRepartition = new Repartition(jeuTest);
            foreach (Equipe equipe in equipesChoisies)
            {
                nouvelleRepartition.AjouterEquipe(equipe);
            }
            foreach (Equipe equipe in equipesNonChoisies)
            {
                nouvelleRepartition.AjouterEquipe(equipe);
            }
            Probleme probleme = new Probleme(); // On crée un nouveau problème pour évaluer la répartition
            probleme = Probleme.ROLEPRINCIPAL; // On choisit le problème à évaluer
            nouvelleRepartition.LancerEvaluation(probleme);
            repartition.LancerEvaluation(probleme); // On évalue la répartition initiale
            if (nouvelleRepartition.Score < repartition.Score)
            {
                repartition = nouvelleRepartition; // On remplace la répartition initiale par la nouvelle répartition si elle est meilleure
            }
            
            
            return repartition;
        }

       


    }

}
