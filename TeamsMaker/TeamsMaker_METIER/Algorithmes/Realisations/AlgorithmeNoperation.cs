using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamsMaker_METIER.Algorithmes.Outils;
using TeamsMaker_METIER.JeuxTest;
using TeamsMaker_METIER.Personnages;
using TeamsMaker_METIER.Problemes;

namespace TeamsMaker_METIER.Algorithmes.Realisations
{
    public class AlgorithmeNoperation : Algorithme
    {
        private int n;
        private int max;
        
        public AlgorithmeNoperation() // Constructeur par défaut
        {
            this.n = 2; // Nombre d'équipes à choisir
            this.max = 10; // Nombre maximum d'itérations
        }

        public AlgorithmeNoperation(int n, int max)
        {
            this.n = n;
            this.max = max;
        }

        public override Repartition Repartir(JeuTest jeuTest)
        {
            Stopwatch sw = new Stopwatch(); //on crée un stopwatch pour mesurer le temps d'exécution de l'algorithme
            sw.Start();//On démarre le stopwatch
            Repartition repartition = new AlgorithmeGloutonCroissant().Repartir(jeuTest);//On utilise l'algorithme glouton pour créer une répartition initiale
            repartition.LancerEvaluation(new Probleme());
            

            for (int i = 0; i < max; i++) //On va faire n itérations pour trouver la meilleure répartition  
            {
                double MeilleurScore = repartition.Score; //On suppose que le meilleur score est le score de la répartition initiale
                Boolean estMeilleur = true; //On suppose que la répartition initiale est la meilleure répartition
               while (estMeilleur) //On boucle tant que la répartition est meilleure que la précédente
                {
                    repartition = déterminerRepartion(repartition, jeuTest);
                    repartition.LancerEvaluation(new Probleme());
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

            List<Personnage> personnagesChoisis = new List<Personnage>(); // Liste des personnages choisis dans les équipes choisies

            foreach (Equipe equipe in equipesChoisies)
            {
                foreach (Personnage personnage in equipe.Membres)
                {
                    personnagesChoisis.Add(personnage);
                    personnagesSansEquipe.Remove(personnage); // On enlève les personnages choisis de la liste des personnages sans équipe car ils seront réutilisés pour créer de nouvelles équipes
                }
            }

            
            equipesChoisies.Clear(); // On vide la liste des équipes choisies pour la remplir avec les nouvelles équipes créées

            for (int i = 1; i < n + 1; i++)
            {
                // On va créer une nouvelle équipe avec les personnages choisis
                Equipe nouvelleEquipe = new Equipe();
                // On va choisir les personnages de l'équipe en en calulant la moyenne des niveaux
                //on initalise la moyenne des niveaux à 0
                double moyenneNiveau = 0;
                while (nouvelleEquipe.Membres.Count() <= 3)
                {
                    //on va parcourire les personnages choisis pour calculer la moyenne des niveaux
                    Personnage personnageChoisi = personnagesChoisis.First(); //on supose que le premier personnage est le meilleur
                    double ecratNiveau = 100; // on initialise à un écart impossible
                    foreach (Personnage personnage in personnagesChoisis)
                    {
                        double moyenneperso = ((moyenneNiveau * nouvelleEquipe.Membres.Count()) + personnage.LvlPrincipal) / (nouvelleEquipe.Membres.Count() + 1); // On calcule la moyenne avec le personnage à ajouter
                        double ecartNiveauPerso = Math.Abs(moyenneperso - 50); // On calcule l'écart entre le moyenne et 50
                        if (ecartNiveauPerso < ecratNiveau) // Si l'écart est plus petit que l'écart précédent
                        {
                            ecratNiveau = ecartNiveauPerso; // On met à jour l'écart
                            personnageChoisi = personnage; // On met à jour le personnage choisi
                        }

                    }
                    //on modifie la moyenne des niveaux de l'équipe
                    moyenneNiveau = ((moyenneNiveau * nouvelleEquipe.Membres.Count()) + personnageChoisi.LvlPrincipal) / (nouvelleEquipe.Membres.Count() + 1);
                    // On ajoute le personnage choisi à l'équipe
                    nouvelleEquipe.AjouterMembre(personnageChoisi);
                    // On enlève le personnage choisi de la liste des personnages choisis
                    personnagesChoisis.Remove(personnageChoisi);
                }
                // On ajoute la nouvelle équipe à la liste des équipes choisies
                equipesChoisies.Add(nouvelleEquipe);
            }
            
            // On va créer la nouvelle répartition avec les équipes choisies et les équipes choisies

            Repartition nouvelleRepartition = new Repartition(jeuTest);
            foreach (Equipe equipe in equipesChoisies)
            {
                nouvelleRepartition.AjouterEquipe(equipe);
            }
            foreach (Equipe equipe in equipesNonChoisies)
            {
                nouvelleRepartition.AjouterEquipe(equipe);
            }
            nouvelleRepartition.LancerEvaluation(new Probleme());

            if (nouvelleRepartition.Score < repartition.Score)
            {
                repartition = nouvelleRepartition; // On remplace la répartition initiale par la nouvelle répartition si elle est meilleure
            }
            
            
            return repartition;
        }

       


    }

}
